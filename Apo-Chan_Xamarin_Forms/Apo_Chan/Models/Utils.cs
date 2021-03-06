﻿using Apo_Chan.Items;
using System;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace Apo_Chan.Models
{
    public static class Utils
    {
        /*===========================================================================*/
        /**
         * check if "end" DateTime is after "start" DateTime
         *
         * @param startDate Start date of appointment
         * @param startTime Start time of appointment
         * @param endDate End date of appointment
         * @param endTime End time of appointment
         * @return true if the end DateTime later than the start DateTime.
         */
        //
        public static bool CheckDateTimeContinuity(ReportItem report)
        {
            DateTime start = new DateTime(report.ReportStartDate.Year, report.ReportStartDate.Month, report.ReportStartDate.Day,
                report.ReportStartTime.Hours, report.ReportStartTime.Minutes, report.ReportStartTime.Seconds);

            DateTime end = new DateTime(report.ReportEndDate.Year, report.ReportEndDate.Month, report.ReportEndDate.Day,
                report.ReportEndTime.Hours, report.ReportEndTime.Minutes, report.ReportEndTime.Seconds);

            if (end.CompareTo(start) < 0)
            {
                end = start.Add(TimeSpan.FromHours(1));
                report.ReportEndDate = end.Date;
                report.ReportEndTime = end.TimeOfDay;
                return false;
            }
            return true;
        }

        public static void ConvertToLocalDateTime(ReportItem report)
        {
            report.ReportStartDate = report.ReportStartDate.ToUniversalTime().Date.Add(report.ReportStartTime).ToLocalTime();
            report.ReportEndDate = report.ReportEndDate.ToUniversalTime().Date.Add(report.ReportEndTime).ToLocalTime();

            report.ReportStartTime = report.ReportStartDate.TimeOfDay;
            report.ReportEndTime = report.ReportEndDate.TimeOfDay;
        }

        public static void ConvertToUtcDateTime(ReportItem report)
        {
            report.ReportStartDate
                = report.ReportStartDate.Date.Add(report.ReportStartTime).ToUniversalTime();
            report.ReportEndDate
                = report.ReportEndDate.Date.Add(report.ReportEndTime).ToUniversalTime();
            report.ReportStartTime = report.ReportStartDate.TimeOfDay;
            report.ReportEndTime = report.ReportEndDate.TimeOfDay;
        }

        public static string Base64FromStream(Stream stream)
        {
            byte[] bytedata = null;
            using (StreamReader reader = new StreamReader(stream))
            {
                    bytedata = Encoding.UTF8.GetBytes(reader.ReadToEnd());
            }
            return Convert.ToBase64String(bytedata);
        }
        //public static Image ImageFromBase64(string base64picture)
        //{
        //    if(base64picture == null) { return null; }
        //    byte[] imageBytes = Convert.FromBase64String(base64picture);
        //    return new Image
        //    {
        //        Source = ImageSource.FromStream(() => new MemoryStream(imageBytes))
        //    };
        //}

        public static void CopyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[16 * 1024];
            int read;
            while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, read);
            }
        }

        public static byte[] ReadStram(Stream input)
        {
            if(input is MemoryStream)
            {
                return (input as MemoryStream).ToArray();
            }
            using (MemoryStream ms = new MemoryStream())
            {
                CopyStream(input, ms);
                return ms.ToArray();
            }
        }
    }

    public static class UnixTime
    {
        private static readonly DateTime UNIX_EPOCH = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /*===========================================================================*/
        /**
         * 現在時刻からUnixTimeを計算する.
         *
         * @return UnixTime.
         */
        public static long Now()
        {
            return (FromDateTime(DateTime.UtcNow));
        }

        /*===========================================================================*/
        /**
         * UnixTimeからDateTimeに変換.
         *
         * @param [in] unixTime 変換したいUnixTime.
         * @return 引数時間のDateTime.
         */
        public static DateTime FromUnixTime(long unixTime)
        {
            return UNIX_EPOCH.AddSeconds(unixTime).ToLocalTime();
        }

        /*===========================================================================*/
        /**
         * 指定時間をUnixTimeに変換する.
         *
         * @param [in] dateTime DateTimeオブジェクト.
         * @return UnixTime.
         */
        public static long FromDateTime(DateTime dateTime)
        {
            double nowTicks = (dateTime.ToUniversalTime() - UNIX_EPOCH).TotalSeconds;
            return (long)nowTicks;
        }
    }

    public class AuthModel
    {
        public bool AdminFlg { get; set; }
        public string Label { get; set; }
    }

    public static class DebugUtil
    {
#if DEBUG
        public static void WriteLine(string message)
        {
            string dtString = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffffff");
            Debug.WriteLine($"[Debug]---[{dtString}]: {message}");

            //try
            //{
            //    throw new Exception("---------aaaa");
            //}
            //catch (Exception ex)
            //{
            //    StackTrace st = new StackTrace(ex, true);
            //    StackFrame[] sf = st.GetFrames();
            //    string aa = st.ToString();
            //    int bb = sf.Length;
            //    foreach (var item in sf)
            //    {
            //        string methodName = item.GetMethod().DeclaringType.FullName;
            //        Debug.WriteLine($"[Debug]---[{dtString}]: {methodName}");
            //    }
            //}
        }
        public static void WriteLine(string format, params object[] args)
        {
            string dtString = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffffff");
            Debug.WriteLine($"[Debug]---[{dtString}]: {String.Format(format, args)}");
        }
#else
        public static void WriteLine(string message)
        {
            return;
        }

        public static void WriteLine(string format, params object[] args)
        {
            return;
        }
#endif
    }
}
