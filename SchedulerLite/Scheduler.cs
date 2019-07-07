using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SchedulerLite
{
    public class Scheduler
    {
        public static bool Enabled { get; private set; } = true;

        public static bool IsRunning { get; private set; } = true;

        private static List<Schedule> schedules = new List<Schedule>();

        private static Timer timer = new Timer(s=>WorkCallBack(), null, -1, -1);

        private static Schedule nextPeriod = null;

        private static HashSet<(Schedule, Task)> running = new HashSet<(Schedule, Task)>();

        private static void WorkCallBack()
        {
            Run(nextPeriod);
            UpdateTimer();
        }

        private static void Run(Schedule schedule)
        {
            var nextRun = GetNextTimeToRun(schedule.StartTime, schedule.Interval);
            if (nextRun != null)
                schedule.StartTime = nextRun.Value;
            else
                schedules.Remove(schedule);

            lock (running)
            {
                if (running.Any(t => ReferenceEquals(t.Item1, schedule)))
                    return;
            }

            (Schedule, Task) tuple = (null, null);

            var task = new Task(() =>
            {               
                try
                {
                    schedule.ExecuteAsync().Wait();
                }
                catch (Exception ex)
                {
                    try
                    {
                        schedule.OnExceptionAsync(ex).Wait();
                    }
                    catch (Exception innerEx)
                    {
                        //todo:记录异常信息
                    }
                }
                finally
                {
                    lock (running)
                    {
                        running.Remove(tuple);
                    }
                }
            }, TaskCreationOptions.PreferFairness);

            tuple = (schedule, task);

            lock (running)
            {
                running.Add(tuple);
            }

            task.Start();

        }

        private static void UpdateTimer()
        {
            if (!Enabled) return;

            var nextToRun = schedules.OrderBy(s => s.StartTime).FirstOrDefault();
            if (nextToRun == null)
            {
                timer.Change(-1, -1);
            }
            else
            {
                nextPeriod = nextToRun;
                var interval = nextToRun.StartTime - DateTime.Now;

                if (interval <= TimeSpan.Zero)
                {
                    WorkCallBack();
                }
                else
                {
                    timer.Change(interval, interval);
                }
            }

        }

        private static DateTime? GetNextTimeToRun(DateTime date, TimeSpan interval)
        {
            var span = (DateTime.Now - date).Ticks;
            if (span < 0) return date;
            if (interval <= TimeSpan.Zero) return null;
            return new DateTime(date.Ticks + span + interval.Ticks - span % interval.Ticks, date.Kind);
        }

        public static void WaitForShutdown()
        {
            AppDomain.CurrentDomain.ProcessExit += async (s, e) => await Exit();
            Console.CancelKeyPress += async (s, e) => await Exit();
            Thread.Sleep(Timeout.Infinite);
        }

        public static void StartNew(Schedule schedule, bool executeAtStartTime = true) 
        {
            if (!executeAtStartTime)
                schedule.StartTime = GetNextTimeToRun(schedule.StartTime, schedule.Interval) ?? throw new ArgumentException("任务无法调度");

            schedules.Add(schedule);
            UpdateTimer();
        }

        public async static Task Exit()
        {
            if (!IsRunning) return;
            IsRunning = false;

            Enabled = false;
            timer.Change(-1, -1);

            await Task.Run(() =>
            {
                var tasks = new Task[0];
                do
                {
                    lock (running)
                    {
                        tasks = running.Select(t => t.Item2).ToArray();
                    }
                    Task.WaitAll(tasks);
                } while (tasks.Any());
                IsRunning = true;
            });
        }
    }
}
