using System;

namespace DateLite
{
    public static class DateHelper
    {
        
        /// <summary>
        /// 获取时间的文字描述
        /// </summary>
        /// <param name="time">时间</param>
        /// <returns>文字描述</returns>
        public static string DateDescription(this DateTime time)
        {
            var now = DateTime.Now;
            if (time.CompareTo(now) > 0)
            {
                return "in the future";
            }

            var totalSeconds = now.Subtract(time).TotalSeconds;

            if (totalSeconds <= 60 * 3)
            {
                return "just now";
            }
            else if (totalSeconds <= 60 * 5)
            {
                return "5 minutes ago";
            }
            else if (totalSeconds <= 60 * 10)
            {
                return "10 minutes ago";
            }
            else if (totalSeconds <= 60 * 15)
            {
                return "15 minutes ago";
            }
            else if (totalSeconds <= 60 * 30)
            {
                return "30 minutes ago";
            }
            else if (totalSeconds <= 60 * 60)
            {
                return "1 hour ago";
            }
            else if (totalSeconds <= 60 * 60 * 24)
            {
                return (int)(totalSeconds / 3600) + " hours ago";
            }
            else if (totalSeconds <= 60 * 60 * 24 * 30)
            {
                return (int)(totalSeconds / 3600 / 24) + " days ago";
            }
            else if (totalSeconds < 60 * 60 * 24 * 30 * 12)
            {
                return (int)(totalSeconds / 3600 / 24 / 30) + " months ago";
            }
            else
            {
                return (int)(totalSeconds / 3600 / 24 / 30 / 12) + " years ago";
            }
        }

        /// <summary>
        /// 日期时间标准字符串
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string DateTimeString(this DateTime time)
        {
            return time.ToString(DateFormat.DATE_TIME);
        }

        /// <summary>
        /// 日期标准字符串
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string DateString(this DateTime time)
        {
            return time.ToString(DateFormat.DATE);
        }

        /// <summary>
        /// 时间标准字符串
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string TimeString(this DateTime time)
        {
            return time.ToString(DateFormat.TIME);
        }
    }
}


