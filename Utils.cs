// Copyright (c) Microsoft Corporation\r
// The Microsoft Corporation licenses this file to you under the MIT license.\r
// See the LICENSE file in the project root for more information.

using System.Windows;
using Wox.Plugin;

namespace Community.PowerToys.Run.Plugin.Timestamp
{
    public static class Utils
    {
        private static readonly DateTime UnixTimeBegin = new(1970, 1, 1, 0, 0, 0, 0);
        private static readonly string DateFormat = "yyyy-MM-dd HH:mm:ss";

        public static string GetNowTimeStamp()
        {
            TimeSpan ts = DateTime.Now - TimeZoneInfo.ConvertTimeFromUtc(UnixTimeBegin, TimeZoneInfo.Local);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }

        public static string GetNowDate()
        {
            return DateTime.Now.ToString(DateFormat);
        }

        public static string ParseDate(string dateString)
        {
            return Convert.ToInt64((DateTime.Parse(dateString) - TimeZoneInfo.ConvertTimeFromUtc(UnixTimeBegin, TimeZoneInfo.Local)).TotalSeconds).ToString();
        }

        public static string FormatDate(long unixTime)
        {
            if (unixTime is >= 1_000_000_000 and < 10_000_000_000)
            {
                // do nothing
            }
            else if (unixTime is >= 1_000_000_000_000 and < 10_000_000_000_000)
            {
                unixTime /= 1000;
            }
            else
            {
                throw new FormatException();
            }

            return (TimeZoneInfo.ConvertTimeFromUtc(UnixTimeBegin, TimeZoneInfo.Local) + new TimeSpan(unixTime * TimeSpan.TicksPerSecond)).ToString(DateFormat);
        }

        static void SetClipboardText(string s)
        {
            Clipboard.SetDataObject(s);
        }

        public static Result ToResult(string content, string iconPath)
        {
            return new Result
            {
                Title = content,
                SubTitle = "将结果复制到剪贴板",
                IcoPath = iconPath,
                Action = (e) =>
                {
                    SetClipboardText(content);
                    return true;
                }
            };
        }
    }
}