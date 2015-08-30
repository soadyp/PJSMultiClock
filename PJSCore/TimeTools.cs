using System;
using System.ComponentModel;
using System.Globalization;

namespace PJSCore
{
    public class TimeTools
    {

        public TimeSpan GetTimeSpan(string time)
        {
        var ts =  TimeSpan.Parse(time);
            return ts;
        }
        public DateTimeOffset GetDTO(string tzId, string time)
        {
            var tz = TimeZoneInfo.FindSystemTimeZoneById(tzId);
            var ts = GetTimeSpan(time);
            var adjustedTs = new TimeSpan(ts.Hours - tz.GetUtcOffset(DateTime.UtcNow).Hours ,
                                          ts.Minutes - tz.GetUtcOffset(DateTime.UtcNow).Minutes, 
                                          ts.Seconds );

            var dto = new DateTimeOffset(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day,adjustedTs.Hours,adjustedTs.Minutes,adjustedTs.Seconds, new TimeSpan());
            dto = TimeZoneInfo.ConvertTime(dto,tz);

            return dto;
        }

        public DateTime GetDT(string time)
        {
            
            var ts = GetTimeSpan(time);
            
            var dt = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day, ts.Hours, ts.Minutes, ts.Seconds);
            
            return dt;
        }

        public DateTime ShiftDateTime(DateTime dt, string inTzId, string toTzId)
        {
            var tzFrom = TimeZoneInfo.FindSystemTimeZoneById(inTzId);
            var tzTo = TimeZoneInfo.FindSystemTimeZoneById(toTzId);
            return TimeZoneInfo.ConvertTime(dt, tzFrom, tzTo);
        }

        public TimeSpan GetTimeZoneDiff(string fromTzId, string toTzId)
        {
            var tzFrom = TimeZoneInfo.FindSystemTimeZoneById(fromTzId);
            var tzTo = TimeZoneInfo.FindSystemTimeZoneById(toTzId);

            var diff = tzTo.GetUtcOffset(DateTime.UtcNow) - tzFrom.GetUtcOffset(DateTime.UtcNow);
            return diff;
        }
        public string TimeString(DateTimeOffset dto)
        {
           
           
            return dto.ToString("HH:mm:ss", CultureInfo.InvariantCulture);

        }
      
    }
}