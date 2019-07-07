using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SchedulerLite
{
    public abstract class Schedule : IWork
    {
        public DateTime StartTime { get; set; }

        public TimeSpan Interval { get; set; }

        public abstract Task ExecuteAsync();

        public virtual Task OnExceptionAsync(Exception ex)
        {
            return Task.FromResult(0);
        }
    }
}
