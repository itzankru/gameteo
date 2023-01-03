namespace ItZnak.Infrastruction.Extentions
{
    public static class DateTimeExtentions{
        static readonly DateTime s_dt1970 = new(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        static DateTimeExtentions() { }

        public static double ToUnixTime(this DateTime current)
        {
            return (current - s_dt1970).TotalMilliseconds;
        }
        public static DateTime FromUnixTime(this double unixTimeStamp)
        {
            return s_dt1970.AddMilliseconds( unixTimeStamp ).ToLocalTime();
        }
    }
}