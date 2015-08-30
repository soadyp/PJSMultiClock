using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PJSMultiClock
{
    public class ClockInfo: INotifyPropertyChanged
    {

        private DateTime _refTime;
        private string _tzId;

        public ClockInfo(string clockId, string tzId, DateTime refTime, string reftimeTzId)
        {

            ClockId = clockId;
            _tzId = tzId;
            TimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(tzId);
            if (reftimeTzId != tzId)
            {
                var refTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(reftimeTzId);
                _refTime = TimeZoneInfo.ConvertTime(refTime, refTimeZoneInfo,TimeZoneInfo );
            }
            else
            {
                _refTime = refTime;
            }
        }

        public string ClockId { get; set; }
        public TimeZoneInfo TimeZoneInfo { get;  }
        public string TzId { get { return _tzId; } set { _tzId = value; OnPropertyChanged(); } }
        public string TzName { get; set; }
        
        public DateTimeOffset Now { get; set; }
        public string NowString { get; set; }
       
        public DateTime RefTime { get { return _refTime; }
                                  set { _refTime = value; OnPropertyChanged(); } }


        public void UpdateRefTime(DateTime updatedRefDt)
        {
            _refTime = updatedRefDt;
        }

        public event PropertyChangedEventHandler PropertyChanged;


        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}