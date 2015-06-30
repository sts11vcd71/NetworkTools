using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TfsWatchDog.Bus;

namespace TfsWatchDog.Tests
{
    [TestClass]
    public class TfsWatchDogDevTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            var ip = new ItemsHistoryProcessor();
            ip.ConnectTfs();
            ip.GetEverythingHistoricallyForScope("qqqqq", "", "");
        }

        [TestMethod]
        public void TestMethod2()
        {
            var ip = new ItemsHistoryProcessor();
            ip.ConnectTfs();
            ip.GetAllPossibleColumns();
        }

        [TestMethod]
        public void BurnDown()
        {
            var ip = new ItemsHistoryProcessor();
            ip.ConnectTfs();
            ip.BurnDownChart();
        }

        public void BurnDown2()
        {

        }
    }
}
