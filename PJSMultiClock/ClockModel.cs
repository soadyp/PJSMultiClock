using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using PJSCore;
using PJSMultiClock.Annotations;

namespace PJSMultiClock
{
    public class ClockModel : INotifyPropertyChanged
    {
        public struct Const
        {
            public const string Clock1 = "Clock1";
            public const string Clock2 = "Clock2";
            public const string Clock3 = "Clock3";
            public const string Clock4 = "Clock4";
            public const string Clock5 = "Clock5";
            public const string Clock6 = "Clock6";
            public const string Clock7 = "Clock7";
            public const string Clock8 = "Clock8";
            public const string Clock9 = "Clock9";
           
            public const string DefaultTime = "DefaultTime";
            public const string DefaultTimeValue = "09:00:00";

            public const string PST = "Pacific Standard Time";
            public const string MST = "Mountain Standard Time";
            public const string EST = "Eastern Standard Time";
            public const string AST = "Atlantic Standard Time";
            public const string UTC = "UTC";
            public const string GMT = "GMT Standard Time";
            public const string AUS = "AUS Eastern Standard Time";
            public const string CHINA = "China Standard Time";
            public const string INDIA = "India Standard Time";
            public const string EUR = "W. Europe Standard Time";
        }

        public ClockModel()
        {

         
            GetDefaultClocks();
           
        }


        public void UpdateClockTimeZone(ref ClockInfo clockInfo,DateTime refDt, TimeZoneInfo refDtInfo)
        {
            var tzi = TimeZoneInfo.FindSystemTimeZoneById(clockInfo.TzId);
            if (tzi == null)
            {
                return;
            }
            clockInfo = CreateClockInfo(clockInfo.ClockId, tzi, refDt, refDtInfo);
            
        }

        private void GetDefaultClocks() {
            var pjsConfig = new PjsConfig();
            var timeTools = new TimeTools();



            var defaultDT = DateTime.UtcNow;
            var defaultTime = pjsConfig.GetAppSetting(Const.DefaultTime);
            if (string.IsNullOrEmpty(defaultTime) )
            {
                defaultTime = Const.DefaultTimeValue;
            }

            defaultDT = timeTools.GetDT(defaultTime);

            ThisPc = CreateClockInfo("ThisPC", TimeZoneInfo.Local, defaultDT, TimeZoneInfo.Local);
            ThisPc.PropertyChanged += ProcessClockRefTimeChange;

            var tzi = GetTimeZoneInfoDefault(Const.Clock1, Const.AUS);
            Clock1 = CreateClockInfo(Const.Clock1,tzi, defaultDT, TimeZoneInfo.Local);
            Clock1.PropertyChanged += ProcessClockRefTimeChange;

            tzi = GetTimeZoneInfoDefault(Const.Clock2, Const.GMT);
            Clock2 = CreateClockInfo(Const.Clock2, tzi, defaultDT, TimeZoneInfo.Local);
            Clock2.PropertyChanged += ProcessClockRefTimeChange;

            tzi = GetTimeZoneInfoDefault(Const.Clock3, Const.EST);
            Clock3 = CreateClockInfo(Const.Clock3, tzi, defaultDT, TimeZoneInfo.Local);
            Clock3.PropertyChanged += ProcessClockRefTimeChange;

            tzi = GetTimeZoneInfoDefault(Const.Clock4, Const.MST);
            Clock4 = CreateClockInfo(Const.Clock4, tzi, defaultDT, TimeZoneInfo.Local);
            Clock4.PropertyChanged += ProcessClockRefTimeChange;

            tzi = GetTimeZoneInfoDefault(Const.Clock5, Const.PST);
            Clock5 = CreateClockInfo(Const.Clock5, tzi, defaultDT, TimeZoneInfo.Local);
            Clock5.PropertyChanged += ProcessClockRefTimeChange;

            tzi = GetTimeZoneInfoDefault(Const.Clock6, Const.INDIA);
            Clock6 = CreateClockInfo(Const.Clock6, tzi, defaultDT, TimeZoneInfo.Local);
            Clock6.PropertyChanged += ProcessClockRefTimeChange;

            tzi = GetTimeZoneInfoDefault(Const.Clock7, Const.CHINA);
            Clock7 = CreateClockInfo(Const.Clock6, tzi, defaultDT, TimeZoneInfo.Local);
            Clock7.PropertyChanged += ProcessClockRefTimeChange;

            tzi = GetTimeZoneInfoDefault(Const.Clock8, Const.AST);
            Clock8 = CreateClockInfo(Const.Clock8, tzi, defaultDT, TimeZoneInfo.Local);
            Clock8.PropertyChanged += ProcessClockRefTimeChange;

            tzi = GetTimeZoneInfoDefault(Const.Clock9, Const.EUR);
            Clock9 = CreateClockInfo(Const.Clock9, tzi, defaultDT, TimeZoneInfo.Local);
            Clock9.PropertyChanged += ProcessClockRefTimeChange;

        }

         
        public void UpdateClockTime(ClockInfo clockInfo)
        {
            if (clockInfo?.TimeZoneInfo == null)
            {
                return;
            }
            clockInfo.Now = TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, clockInfo.TimeZoneInfo);
            var timeTools = new TimeTools();
            clockInfo.NowString = timeTools.TimeString(clockInfo.Now);
        }
        private TimeZoneInfo GetTimeZoneInfoDefault(string whichClock, string defaultTzId)
        {
            var pjsConfig = new PjsConfig();
            var tzId = pjsConfig.GetAppSetting(whichClock);
            if (string.IsNullOrEmpty(tzId))
            {
                tzId = defaultTzId;
            }
            var tzi = TimeZoneInfo.FindSystemTimeZoneById(tzId);
            return tzi;
        }

        private ClockInfo CreateClockInfo(string clockId, TimeZoneInfo timeZoneInfo, DateTime refDt, TimeZoneInfo refTimeZoneInfo)
        {
            var timeTools = new TimeTools();

            var clockInfo = new ClockInfo(clockId, timeZoneInfo?.Id, refDt, refTimeZoneInfo?.Id);
            if (timeZoneInfo == null)
            {
                return clockInfo;
            }
            clockInfo.TzName = timeZoneInfo.DisplayName;
            clockInfo.Now =  TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, timeZoneInfo);
            clockInfo.NowString = timeTools.TimeString( clockInfo.Now);
           
            return clockInfo;
        }

        public void UpdateClocks()
        {
            this.UpdateClockTime(ThisPc);
            this.UpdateClockTime(Clock1);
            this.UpdateClockTime(Clock2);
            this.UpdateClockTime(Clock3);
            this.UpdateClockTime(Clock4);
            this.UpdateClockTime(Clock5);
            this.UpdateClockTime(Clock6);
            this.UpdateClockTime(Clock7);
            this.UpdateClockTime(Clock8);
            this.UpdateClockTime(Clock9);
          
            
        }

        public delegate void ModelRefTimeUpdateHandler(object sender, EventArgs e);
        public event ModelRefTimeUpdateHandler OnRefTimeUpdated;

        private bool _refTimeUpdInProgress ;
        public void ProcessClockRefTimeChange(object sender, PropertyChangedEventArgs e)
        {
            if (_refTimeUpdInProgress)
            {
                return;
            }
            _refTimeUpdInProgress = true;
            var senderClock = (ClockInfo) sender;

            ThisPc.UpdateRefTime( TimeZoneInfo.ConvertTime(senderClock.RefTime, senderClock.TimeZoneInfo, ThisPc.TimeZoneInfo));

            Clock1.UpdateRefTime(TimeZoneInfo.ConvertTime(ThisPc.RefTime, ThisPc.TimeZoneInfo, Clock1.TimeZoneInfo));
            Clock2.UpdateRefTime(TimeZoneInfo.ConvertTime(ThisPc.RefTime, ThisPc.TimeZoneInfo, Clock2.TimeZoneInfo));
            Clock3.UpdateRefTime(TimeZoneInfo.ConvertTime(ThisPc.RefTime, ThisPc.TimeZoneInfo, Clock3.TimeZoneInfo));
            Clock4.UpdateRefTime(TimeZoneInfo.ConvertTime(ThisPc.RefTime, ThisPc.TimeZoneInfo, Clock4.TimeZoneInfo));
            Clock5.UpdateRefTime(TimeZoneInfo.ConvertTime(ThisPc.RefTime, ThisPc.TimeZoneInfo, Clock5.TimeZoneInfo));
            Clock6.UpdateRefTime(TimeZoneInfo.ConvertTime(ThisPc.RefTime, ThisPc.TimeZoneInfo, Clock6.TimeZoneInfo));
            Clock7.UpdateRefTime(TimeZoneInfo.ConvertTime(ThisPc.RefTime, ThisPc.TimeZoneInfo, Clock7.TimeZoneInfo));
            Clock8.UpdateRefTime(TimeZoneInfo.ConvertTime(ThisPc.RefTime, ThisPc.TimeZoneInfo, Clock8.TimeZoneInfo));
            Clock9.UpdateRefTime(TimeZoneInfo.ConvertTime(ThisPc.RefTime, ThisPc.TimeZoneInfo, Clock9.TimeZoneInfo));

            OnRefTimeUpdated?.Invoke(this, new EventArgs());
            _refTimeUpdInProgress = false;
        }

        public TimeZoneInfo SelectedTimeZoneInfo { get; set; }

        public ObservableCollection<TimeZoneInfo> TimeZoneInfos { get; set; }


        private ClockInfo _thisPc;

        private ClockInfo _clock1;

        private ClockInfo _clock2;

        private ClockInfo _clock3;

        private ClockInfo _clock4;

        private ClockInfo _clock5;

        private ClockInfo _clock6;

        private ClockInfo _clock7;

        private ClockInfo _clock8;

        private ClockInfo _clock9;

       
      

        public ClockInfo ThisPc
        {
            get { return _thisPc; }
            set
            {
                _thisPc = value;
                OnPropertyChanged();
            }
        }

        public ClockInfo Clock1
        {
            get { return _clock1; }
            set
            {
                _clock1 = value;
                OnPropertyChanged();
            }
        }

        public ClockInfo Clock2
        {
            get { return _clock2; }
            set
            {
                _clock2 = value;
                OnPropertyChanged();
            }
        }

        public ClockInfo Clock3
        {
            get { return _clock3; }
            set
            {
                _clock3 = value;
                OnPropertyChanged();
            }
        }

        public ClockInfo Clock4
        {
            get { return _clock4; }
            set
            {
                _clock4 = value;
                OnPropertyChanged();
            }
        }

        public ClockInfo Clock5
        {
            get { return _clock5; }
            set
            {
                _clock5 = value;
                OnPropertyChanged();
            }
        }

        public ClockInfo Clock6
        {
            get { return _clock6; }
            set
            {
                _clock6 = value;
                OnPropertyChanged();
            }
        }

        public ClockInfo Clock7
        {
            get { return _clock7; }
            set
            {
                _clock7 = value;
                OnPropertyChanged();
            }
        }

        public ClockInfo Clock8
        {
            get { return _clock8; }
            set
            {
                _clock8 = value;
                OnPropertyChanged();
            }
        }

        public ClockInfo Clock9
        {
            get { return _clock9; }
            set
            {
                _clock9 = value;
                OnPropertyChanged();
            }
        }

        


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}