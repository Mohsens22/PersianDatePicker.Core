using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Arash;

namespace PersianDateTests
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void Test1()
        {

            System.Globalization.PersianCalendar pc = new System.Globalization.PersianCalendar();
            var z = pc.ToDateTime(1, 1, 1, 0, 0, 0, 0);
            for (uint i = 0; i <= (DateTime.MaxValue - z).Days; i++)
            {
                int y, m, d;
                PersianDate.yearMonthDay(i, out y, out m, out d);
                var td = pc.AddDays(z, (int)i);
                int yy = pc.GetYear(td), mm = pc.GetMonth(td), dd = pc.GetDayOfMonth(td);
                if (yy != y || dd != d || mm != m || PersianDate.days(yy, mm, dd) != i || !PersianDate.IsValid(y, m, d))
                    Assert.Fail(string.Format("this failed: {0}: {1}/{2}/{3},  {4}", i, y, m, d, PersianDate.days(y, m, d)));
            }

        }
        [TestMethod]
        public void DayOfYearTests()
        {
            System.Globalization.PersianCalendar pc = new System.Globalization.PersianCalendar();
            var z = pc.ToDateTime(1, 1, 1, 0, 0, 0, 0);
            for (uint i = 0; i <= (DateTime.MaxValue - z).Days; i++)
            {
                PersianDate pd = new PersianDate(i);
                if (pc.GetDayOfYear(pd.ToDateTime()) != pd.DayOfYear)
                    Assert.Fail(string.Format("this failed: {0}: {1}, {2}, {3}", i, pd.ToString(), pd.DayOfYear, pc.GetDayOfYear(pd.ToDateTime())));
            }
        }

        [TestMethod]
        public void DayOfWeekTests()
        {
            System.Globalization.PersianCalendar pc = new System.Globalization.PersianCalendar();

            List<PersianDate> l = new List<PersianDate>();
            for (var pd = PersianDate.MinValue; pd <= new PersianDate(DateTime.MaxValue); pd = pd.AddDays(1))
            {
                var d = pc.ToDateTime(pd.Year, pd.Month, pd.Day, 0, 0, 0, 0);
                if (d.DayOfWeek != pd.DayOfWeek)
                    l.Add(pd);
            }
            Assert.IsTrue(l.Count == 0);
        }

        [TestMethod]
        public void ParseTests()
        {
            for (var pd = PersianDate.MinValue; pd <= new PersianDate(10000, 1, 1); pd = pd.AddDays(1))
            {
                Assert.AreEqual(pd, PersianDate.Parse(pd.ToString()));
            }

            PersianDate p;
            Assert.IsFalse(PersianDate.TryParse("12/11/432", out p));
            Assert.IsFalse(PersianDate.TryParse("1390/1/0", out p));
            Assert.IsFalse(PersianDate.TryParse("1344/8/31", out p));
            Assert.IsFalse(PersianDate.TryParse("0/12/29", out p));

        }


        [TestMethod]
        public void ToStringTests()
        {
            string result = "";
            var today = DateTime.Today;
            var persianToday = PersianDate.Today;

            result += "\r\nToString: " + persianToday.ToDateTime().ToString();
            result += "\r\nToShortDateString: " + persianToday.ToDateTime().ToShortDateString();
            result += "\r\nToLongDateString: " + persianToday.ToDateTime().ToLongDateString();
            result += "\r\n---";
            result += "\r\nToString: " + new PersianDate(today).ToString();
            result += "\r\nToLongDateString: " + new PersianDate(today).ToLongDateString();
            result += "\r\nToLongDateString: " + persianToday.ToLongDateString();
            result += "\r\nMonthAsPersianMonth: " + new PersianDate(today).MonthAsPersianMonth;

            Assert.Inconclusive(result);
        }


    }
}
