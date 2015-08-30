using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using PJSCore;

namespace PJSMultiClock
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public  ClockModel Model;
        private bool _init;

        public MainWindow()
        {
            InitializeComponent();
            _init = true;
            Model = new ClockModel();


            this.DataContext = Model;
         

            Model.TimeZoneInfos = new ObservableCollection<TimeZoneInfo>( TimeZoneInfo.GetSystemTimeZones().ToList());

            //            clock1
            tZId1.ItemsSource = Model.TimeZoneInfos;
            tZId1.DisplayMemberPath = "Id";
            tZId1.SelectedValuePath = "Id";
            tZId1.SelectedValue = Model.Clock1?.TzId;
          
            
            //            clock2
            tZId2.ItemsSource = Model.TimeZoneInfos;
            tZId2.DisplayMemberPath = "Id";
            tZId2.SelectedValuePath = "Id";
            tZId2.SelectedValue = Model.Clock2?.TzId;
            
            //            clock3
            tZId3.ItemsSource = Model.TimeZoneInfos;
            tZId3.DisplayMemberPath = "Id";
            tZId3.SelectedValuePath = "Id";
            tZId3.SelectedValue = Model.Clock3?.TzId;

            //            clock4
            tZId4.ItemsSource = Model.TimeZoneInfos;
            tZId4.DisplayMemberPath = "Id";
            tZId4.SelectedValuePath = "Id";
            tZId4.SelectedValue = Model.Clock4?.TzId;

            //            clock5
            tZId5.ItemsSource = Model.TimeZoneInfos;
            tZId5.DisplayMemberPath = "Id";
            tZId5.SelectedValuePath = "Id";
            tZId5.SelectedValue = Model.Clock5?.TzId;

            //            clock6
            tZId6.ItemsSource = Model.TimeZoneInfos;
            tZId6.DisplayMemberPath = "Id";
            tZId6.SelectedValuePath = "Id";
            tZId6.SelectedValue = Model.Clock6?.TzId;

            //            clock7
            tZId7.ItemsSource = Model.TimeZoneInfos;
            tZId7.DisplayMemberPath = "Id";
            tZId7.SelectedValuePath = "Id";
            tZId7.SelectedValue = Model.Clock7?.TzId;
            
            //            clock8
            tZId8.ItemsSource = Model.TimeZoneInfos;
            tZId8.DisplayMemberPath = "Id";
            tZId8.SelectedValuePath = "Id";
            tZId8.SelectedValue = Model.Clock8?.TzId;
            
            //            clock9
            tZId9.ItemsSource = Model.TimeZoneInfos;
            tZId9.DisplayMemberPath = "Id";
            tZId9.SelectedValuePath = "Id";
            tZId9.SelectedValue = Model.Clock9?.TzId;

            Model.OnRefTimeUpdated += RefTimeChanged;


            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();
            _init = false;
        }

        public void RefTimeChanged(object sender, EventArgs e)
        {
            tpNow.Value = Model.ThisPc.RefTime;
            tp1.Value = Model.Clock1.RefTime;
            tp2.Value = Model.Clock2.RefTime;
            tp3.Value = Model.Clock3.RefTime;
            tp4.Value = Model.Clock4.RefTime;
            tp5.Value = Model.Clock5.RefTime;
            tp6.Value = Model.Clock6.RefTime;
            tp7.Value = Model.Clock7.RefTime;
            tp8.Value = Model.Clock8.RefTime;
            tp9.Value = Model.Clock9.RefTime;
        }

        public void TimeZoneIdChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_init)
            {
                return;
            }

            var pjsCfg = new PjsConfig();
            TimeZoneInfo newTimeZoneInfo = (TimeZoneInfo) e.AddedItems[0];

            var cbName = sender.GetType().GetProperty("Name").GetValue(sender, null).ToString();

            ClockInfo whichclock;
            switch (cbName)
            {
                case "tZId1":
                    whichclock = Model.Clock1;
                    whichclock.TzId = newTimeZoneInfo.Id;
                    Model.UpdateClockTimeZone(ref whichclock ,Model.ThisPc.RefTime, Model.ThisPc.TimeZoneInfo);
                    Model.Clock1 = whichclock;
                    pjsCfg.SaveAppSetting(ClockModel.Const.Clock1, newTimeZoneInfo.Id);
                    break;

                case "tZId2":
                    whichclock = Model.Clock2;
                    whichclock.TzId = newTimeZoneInfo.Id;
                    Model.UpdateClockTimeZone(ref whichclock, Model.ThisPc.RefTime, Model.ThisPc.TimeZoneInfo);
                    Model.Clock2 = whichclock;
                    pjsCfg.SaveAppSetting(ClockModel.Const.Clock2, newTimeZoneInfo.Id);
                    break;
                case "tZId3":
                    whichclock = Model.Clock3;
                    whichclock.TzId = newTimeZoneInfo.Id;
                    Model.UpdateClockTimeZone(ref whichclock, Model.ThisPc.RefTime, Model.ThisPc.TimeZoneInfo);
                    Model.Clock3 = whichclock;
                    pjsCfg.SaveAppSetting(ClockModel.Const.Clock3, newTimeZoneInfo.Id);
                    break;
                case "tZId4":
                    whichclock = Model.Clock4;
                    whichclock.TzId = newTimeZoneInfo.Id;
                    Model.UpdateClockTimeZone(ref whichclock, Model.ThisPc.RefTime, Model.ThisPc.TimeZoneInfo);
                    Model.Clock4 = whichclock;
                    pjsCfg.SaveAppSetting(ClockModel.Const.Clock4, newTimeZoneInfo.Id);
                    break;
                case "tZId5":
                    whichclock = Model.Clock5;
                    whichclock.TzId = newTimeZoneInfo.Id;
                    Model.UpdateClockTimeZone(ref whichclock, Model.ThisPc.RefTime, Model.ThisPc.TimeZoneInfo);
                    Model.Clock5 = whichclock;
                    pjsCfg.SaveAppSetting(ClockModel.Const.Clock5, newTimeZoneInfo.Id);
                    break;
                case "tZId6":
                    whichclock = Model.Clock6;
                    whichclock.TzId = newTimeZoneInfo.Id;
                    Model.UpdateClockTimeZone(ref whichclock, Model.ThisPc.RefTime, Model.ThisPc.TimeZoneInfo);
                    Model.Clock6 = whichclock;
                    pjsCfg.SaveAppSetting(ClockModel.Const.Clock6, newTimeZoneInfo.Id);
                    break;
                case "tZId7":
                    whichclock = Model.Clock7;
                    whichclock.TzId = newTimeZoneInfo.Id;
                    Model.UpdateClockTimeZone(ref whichclock, Model.ThisPc.RefTime, Model.ThisPc.TimeZoneInfo);
                    Model.Clock7 = whichclock;
                    pjsCfg.SaveAppSetting(ClockModel.Const.Clock7, newTimeZoneInfo.Id);
                    break;
                case "tZId8":
                    whichclock = Model.Clock8;
                    whichclock.TzId = newTimeZoneInfo.Id;
                    Model.UpdateClockTimeZone(ref whichclock, Model.ThisPc.RefTime, Model.ThisPc.TimeZoneInfo);
                    Model.Clock8 = whichclock;
                    pjsCfg.SaveAppSetting(ClockModel.Const.Clock8, newTimeZoneInfo.Id);
                    break;
                case "tZId9":
                    whichclock = Model.Clock9;
                    whichclock.TzId = newTimeZoneInfo.Id;
                    Model.UpdateClockTimeZone(ref whichclock, Model.ThisPc.RefTime, Model.ThisPc.TimeZoneInfo);
                    Model.Clock9 = whichclock;
                    pjsCfg.SaveAppSetting(ClockModel.Const.Clock9, newTimeZoneInfo.Id);
                    break;
                default:
                      break;    
            }
        }

        void timer_Tick(object sender, EventArgs e)
        {
            Model.UpdateClocks();
            this.timeNow.Text = Model.ThisPc.NowString;
            this.tzNow1.Text = Model.Clock1?.NowString;
            this.tzNow2.Text = Model.Clock2?.NowString;
            this.tzNow3.Text = Model.Clock3?.NowString;
            this.tzNow4.Text = Model.Clock4?.NowString;
            this.tzNow5.Text = Model.Clock5?.NowString;
            this.tzNow6.Text = Model.Clock6?.NowString;
            this.tzNow7.Text = Model.Clock7?.NowString;
            this.tzNow8.Text = Model.Clock8?.NowString;
            this.tzNow9.Text = Model.Clock9?.NowString;
          
            

        }


       
    }
}
