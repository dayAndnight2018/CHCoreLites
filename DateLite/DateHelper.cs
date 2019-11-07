using System;
using System.Collections.Generic;
using System.Text;

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
                return "未来";
            }

            double totalSeconds = now.Subtract(time).TotalSeconds;
            string str = null;

            if (totalSeconds <= 60 * 3)
            {
                str = "刚刚";
            }
            else if (totalSeconds <= 60 * 5)
            {
                str = "5分钟前";
            }
            else if (totalSeconds <= 60 * 10)
            {
                str = "10分钟前";
            }
            else if (totalSeconds <= 60 * 15)
            {
                str = "15分钟前";
            }
            else if (totalSeconds <= 60 * 30)
            {
                str = "半小时前";
            }
            else if (totalSeconds <= 60 * 60)
            {
                str = "1小时前";
            }
            else if (totalSeconds <= 60 * 60 * 24)
            {
                str = (int)(totalSeconds / 3600) + "小时前";
            }
            else if (totalSeconds <= 60 * 60 * 24 * 30)
            {
                str = (int)(totalSeconds / 3600 / 24) + "天前";
            }
            else if (totalSeconds < 60 * 60 * 24 * 30 * 12)
            {
                str = (int)(totalSeconds / 3600 / 24 / 30) + "个月前";
            }
            else
            {
                str = (int)(totalSeconds / 3600 / 24 / 30 / 12) + "年前";
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
            return time != null ? time.ToString("yyyy-MM-dd HH:mm:ss") : String.Empty;
        }

        /// <summary>
        /// 日期标准字符串
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string DateString(this DateTime time)
        {
            return time != null ? time.ToString("yyyy-MM-dd") : String.Empty;
        }

        /// <summary>
        /// 时间标准字符串
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string TimeString(this DateTime time)
        {
            return time != null ? time.ToString("HH:mm:ss") : String.Empty;
        }
    }
}


