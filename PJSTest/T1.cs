using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PJSCore;
using PJSMultiClock;

namespace PJSTest
{
    [TestClass]
    public class T1

    {
        private const string KeyNew = "TKey";
        private const string SomeVal = "SomeValue";

        [TestMethod]
        public void AppConfigWriteAndRead()
        {
            var cfg = new PJSCore.PjsConfig();
            cfg.SaveAppSetting(KeyNew, SomeVal);
            var cfgval = cfg.GetAppSetting(KeyNew);
            Assert.IsTrue(cfgval == SomeVal, "Config update and reread failed");

        }


        [TestMethod]
        public void AppTimeString()
        {
           var timeTools = new TimeTools();
            var dto = new DateTimeOffset(2015,8,8,15,2,3,new TimeSpan(0,0,0) );
           
        var str =   timeTools.TimeString( dto  );
          Debug.WriteLine(str);
          Assert.IsTrue(str == "15:02:03");
        }


        [TestMethod]
        public void TZIDList()
        {

            var tziList = TimeZoneInfo.GetSystemTimeZones();
            foreach (var timeZoneInfo in tziList)
            {
                Debug.WriteLine(timeZoneInfo.Id + " -> " + timeZoneInfo.StandardName);
            }
        }

        [TestMethod]
        public void GetTimeSpanTest()
        {

            var timeTools = new TimeTools();
            var ts = timeTools.GetTimeSpan("09:00:00");
            Debug.WriteLine(ts);
            Assert.AreEqual(new TimeSpan(9,0,0),ts );

        }
        [TestMethod]
        public void GetDTOTest()
        {
            var now = DateTimeOffset.Now;
            var timeTools = new TimeTools();
            var tzId = "W. Europe Standard Time";
            var dto = timeTools.GetDTO(tzId, "09:00:00");
            Debug.WriteLine(dto);
            

        }
        [TestMethod]
        public void GetDTTest()
        {
            var now = DateTime.UtcNow;
            var timeTools = new TimeTools();
           
            var dt = timeTools.GetDT( "09:00:00");
            Debug.WriteLine(dt);


        }

        [TestMethod]
        public void GetDiffTest()
        {
            var now = DateTime.UtcNow;
            var timeTools = new TimeTools();
            var tzIdFrom = "W. Europe Standard Time";
            var tzIdTo = ClockModel.Const.AUS;
            var diff = timeTools.GetTimeZoneDiff(tzIdFrom, tzIdTo);
            
            Debug.WriteLine(diff);
            Assert.IsTrue(diff.Hours >= 8 && diff.Hours <= 11);

        }
    }
}
