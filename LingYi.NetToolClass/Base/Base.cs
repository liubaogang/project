using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LingYi.NetToolClass.Base
{
    public class Base
    {
        /// <summary>
        /// http://www.cnblogs.com/EasonJim/p/6140846.html
        /// Unix时间戳转换为DateTime
        /// </summary>
        public static DateTime ConvertToDateTime(string timestamp)
        {
            DateTime time = DateTime.MinValue;
            //精确到毫秒
            //时间戳转成时间
            DateTime start = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            try
            {
                time = timestamp.Length == 10 ? start.AddSeconds(long.Parse(timestamp)) : start.AddMilliseconds(long.Parse(timestamp));
            }
            catch
            {
                return start;//转换失败
            }
            return time;
        }

        /// <summary>
        /// DateTime转换为Unix时间戳
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string ConvertTimestamp(DateTime time)
        {
            double intResult = 0;
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            intResult = (time - startTime).TotalMilliseconds;
            return Math.Round(intResult, 0).ToString();
        }
    }
}
