using System;
using System.Runtime.InteropServices;

namespace WebsiteService.MonitorTerminal.Utils
{
    public class TimeUtil
    {
        //设置系统时间的API函数
        [DllImport("Kernel32.dll")]
        static extern bool SetSystemTime(ref SystemTime sysTime);
        [DllImport("Kernel32.dll")]
        static extern bool SetLocalTime(ref SystemTime sysTime);
        [DllImport("Kernel32.dll")]
        static extern void GetSystemTime(ref SystemTime sysTime);
        [DllImport("Kernel32.dll")]
        static extern void GetLocalTime(ref SystemTime sysTime);


        [StructLayout(LayoutKind.Sequential)]
        private struct SystemTime
        {
            public short year;
            public short month;
            public short dayOfWeek;
            public short day;
            public short hour;
            public short minute;
            public short second;
            public short milliseconds;
        }
        /// <summary>
        /// 设置系统时间
        /// </summary>
        /// <param name="dt">需要设置的时间</param>
        /// <returns>返回系统时间设置状态，true为成功，false为失败</returns>
        public static bool SetDate(DateTime dt)
        {
            SystemTime st = new SystemTime {
                year = (short)dt.Year,
                month = (short)dt.Month,
                dayOfWeek = (short)dt.DayOfWeek,
                day = (short)dt.Day,
                hour = (short)dt.Hour,
                minute = (short)dt.Minute,
                second = (short)dt.Second,
                milliseconds = (short)dt.Millisecond
            };
            bool rt = SetLocalTime(ref st);
            return rt;
        }

    }
}