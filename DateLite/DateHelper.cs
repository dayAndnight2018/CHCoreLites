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
            DateTime now = DateTime.Now;
            if(time.CompareTo(now)>0)
            {
                return "in the future";
            }

            double totalSeconds = now.Subtract(time).TotalSeconds;
            string str = String.Empty;

            if (totalSeconds <= 60 * 3)
            {
                str = "just now";
            }
            else if (totalSeconds <= 60 * 5)
            {
                str = "5 minutes ago";
            }
            else if (totalSeconds <= 60 * 10)
            {
                str = "10 minutes ago";
            }
            else if (totalSeconds <= 60 * 15)
            {
                str = "15 minutes ago";
            }
            else if (totalSeconds <= 60 * 30)
            {
                str = "30 minutes ago";
            }
            else if (totalSeconds <= 60 * 60)
            {
                str = "1 hour ago";
            }
            else if (totalSeconds <= 60 * 60 * 24)
            {
                str = (int)(totalSeconds / 3600) + " hours ago";
            }
            else if (totalSeconds <= 60 * 60 * 24 * 30)
            {
                str = (int)(totalSeconds / 3600 / 24) + " days ago";
            }
            else if (totalSeconds < 60 * 60 * 24 * 30 * 12)
            {
                str = (int)(totalSeconds / 3600 / 24 / 30) + " months ago";
            }
            else
            {
                str = (int)(totalSeconds / 3600 / 24 / 30 / 12) + " years ago";
            }
            return str;
        }

        /// <summary>
        /// 日期时间标准字符串
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string DateTimeString(this DateTime time)
        {
            return time.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 日期标准字符串
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string DateString(this DateTime time)
        {
            return time.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// 时间标准字符串
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string TimeString(this DateTime time)
        {
            return time.ToString("HH:mm:ss");
        }
    }
}


