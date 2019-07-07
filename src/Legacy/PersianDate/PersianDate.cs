using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Arash
{
    //this attribute enables converting to/from this type in wpf and other designing environments
    [TypeConverter(typeof(PersianDateConverter))]

    public struct PersianDate : IComparable<PersianDate>
    {
        const int period33y = 365 * 33 + 8;

        const int p33p1 = 366;
        const int p33p2 = 365 * 20 + 4;
        const int p33p3 = 366;
        const int p33p4 = 365 * 11 + 2;

        const int offset = 365 * 22 + 7;
        const int offsety = 22;
        const int period4y = 365 * 4 + 1;

        internal static void yearMonthDay(uint x, out int y, out int m, out int d)
        {
            int a = (int)(x / period33y);
            y = a * 33;
            a = (int)(x % period33y);
            int b;
            if (a < p33p1)
            {
                b = 0;
                monthAndDay(a, out m, out d);
            }
            else if (a < p33p1 + p33p2)
            {
                int rem = (a - p33p1) % period4y;
                b = 1 + (a - p33p1) / period4y * 4 + yearInP4y(rem);
                monthAndDay(rem - 365 * yearInP4y(rem), out m, out d);
            }
            else if (a < p33p1 + p33p2 + p33p3)
            {
                b = 21;
                monthAndDay(a - p33p1 - p33p2, out m, out d);
            }
            else
            {
                int rem = (a - p33p1 - p33p2 - p33p3) % period4y;

                b = (a - p33p1 - p33p2 - p33p3) / period4y * 4 + 22 + yearInP4y(rem);
                monthAndDay(rem - 365 * yearInP4y(rem), out m, out d);
            }

            y += b + 1;

        }
        static void monthAndDay(int x, out int m, out int d)
        {
            if (x >= 366) throw new ArgumentException("X must be less than or equal to 365");
            if (x >= 31 * 6)
            {
                m = (x - 31 * 6) / 30 + 7;
                d = (x - 31 * 6) % 30 + 1;
            }
            else
            {
                m = x / 31 + 1;
                d = x % 31 + 1;
            }
        }
        static int yearInP4y(int x)
        {
            if (x > period4y) throw new ArgumentException("X must be less than or equal to period4y");
            int r = x / 365;
            return r < 4 ? r : 3;
        }




        //max date:"11759224/6/25"
        const int maxYear = 11759224, maxMonth = 6, maxDay = 25;
        public static readonly PersianDate MaxValue = new PersianDate(maxYear, maxMonth, maxDay);


        //min date: "1/1/1"
        const int minYear = 1, minMonth = 1, minDay = 1;
        public static readonly PersianDate MinValue = new PersianDate(minYear, minMonth, minDay);

        //3/21/0622 12:00:00 AM
        static DateTime firstDateDateTime = new DateTime(622, 3, 21);

        /// <summary>
        /// compares 2 dates with no validation
        /// </summary>
        static int comp(int y1, int m1, int d1, int y2, int m2, int d2)
        {
            if (y1 != y2) return y1 - y2;
            if (m1 != m2) return m1 - m2;
            return d1 - d2;
        }

        static void yearAndDaysInYear(uint x, out int y, out int ds)
        {
            int a = (int)(x / period33y);
            y = a * 33;
            a = (int)(x % period33y);
            int b;
            if (a < p33p1)
            {
                b = 0;
                ds = a;//monthAndDay(a, out m, out d);
            }
            else if (a < p33p1 + p33p2)
            {
                int rem = (a - p33p1) % period4y;
                b = 1 + (a - p33p1) / period4y * 4 + yearInP4y(rem);
                ds = rem - 365 * yearInP4y(rem);//monthAndDay(rem - 365 * yearInP4y(rem), out m, out d);
            }
            else if (a < p33p1 + p33p2 + p33p3)
            {
                b = 21;
                ds = a - p33p1 - p33p2;//monthAndDay(a - p33p1 - p33p2, out m, out d);
            }
            else
            {
                int rem = (a - p33p1 - p33p2 - p33p3) % period4y;

                b = (a - p33p1 - p33p2 - p33p3) / period4y * 4 + 22 + yearInP4y(rem);
                ds = rem - 365 * yearInP4y(rem);//monthAndDay(rem - 365 * yearInP4y(rem), out m, out d);
            }

            y += b + 1;

        }

        static int daysInYear(int m, int d)
        {
            if (m < 7) return (m - 1) * 31 + d - 1;
            return (31 * 6) + (m - 7) * 30 + d - 1;
        }
        internal static uint days(int y, int m, int d)
        {
            uint r;
            r = (uint)(y - 1) / 33 * period33y + (uint)daysInYear(m, d);
            int a = (y - 1) % 33;
            if (a == 0)
            {

            }
            else if (a <= 20)
            {
                r += (uint)p33p1 + (uint)((a - 1) / 4) * period4y + (uint)((a - 1) % 4) * 365;
            }
            else if (a <= 21)
            {
                r += p33p1 + p33p2;
            }
            else
            {
                r += (uint)(p33p1 + p33p2 + p33p3) + (uint)((a - 22) / 4) * period4y + (uint)((a - 22) % 4) * 365;
            }
            return r;
        }

        /// <summary>
        /// Checks whether the given date is a valid date
        /// </summary>
        public static bool IsValid(int year, int month, int day)
        {
            if (month < 1 || month > 12) return false;
            if (day < 1) return false;
            if (month < 7 && day > 31) return false;
            if (month >= 7 && day > 30) return false;
            if (month == 12 && day > 29 && !IsLeapYear(year)) return false;
            return true;
        }

        public static bool IsLeapYear(int year)
        {
            int r = year % 33;
            return (r == 1 || r == 5 || r == 9 || r == 13 || r == 17 || r == 22 || r == 26 || r == 30);
        }
        public static PersianDate Today
        {
            get
            {
                return new PersianDate(DateTime.Today);
            }
        }
        public static int DaysInMonth(int year, int month)
        {
            if (month < 1 || month > 12) throw new ArgumentOutOfRangeException("month", "Month must be between 1 and 12");
            if (month <= 6) return 31;
            if (month <= 11) return 30;
            if (IsLeapYear(year)) return 30;
            return 29;
        }
        /// <summary>
        /// Converts the specified string representation of a persian date to its equivalent PersianDate value.
        /// </summary>
        /// <param name="persianDateString"></param>
        /// <returns></returns>
        public static PersianDate Parse(string persianDateString)
        {
            string[] parts = persianDateString.Split('/');
            if (parts.Length != 3) throw new ArgumentException("The date string must be in the form y/m/d");
            int y, m, d;
            var style = System.Globalization.NumberStyles.AllowLeadingWhite |
                System.Globalization.NumberStyles.AllowTrailingWhite;
            try
            {
                y = int.Parse(parts[0], style);
                m = int.Parse(parts[1], style);
                d = int.Parse(parts[2], style);
                return new PersianDate(y, m, d);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("The date string must be in the form y/m/d", ex);
            }
        }
        /// <summary>
        /// Converts the specified string representation of a persian date to its equivalent PersianDate value.
        /// </summary>
        /// <param name="presianDateString"></param>
        /// <param name="result">If the conversion succeeds, this parameter will contain the PersianDate value 
        /// equivalent to the persian date specified in the first parameter, otherwise it will have the value of date 1/1/1 </param>
        /// <returns>true if the s parameter was converted successfully; otherwise, false.</returns>
        public static bool TryParse(string presianDateString, out PersianDate result)
        {
            try
            {
                result = Parse(presianDateString);
                return true;
            }
            catch
            {
                result = new PersianDate();
                return false;
            }
        }

        #region Comparison operators
        public static bool operator <(PersianDate x, PersianDate y)
        {
            return x.CompareTo(y) < 0;
        }
        public static bool operator <=(PersianDate x, PersianDate y)
        {
            return x.CompareTo(y) <= 0;
        }
        public static bool operator ==(PersianDate x, PersianDate y)
        {
            return x.n == y.n;
        }
        public static bool operator >=(PersianDate x, PersianDate y)
        {
            return x.CompareTo(y) >= 0;
        }
        public static bool operator >(PersianDate x, PersianDate y)
        {
            return x.CompareTo(y) > 0;
        }
        public static bool operator !=(PersianDate x, PersianDate y)
        {
            return x.n != y.n;
        }
        #endregion


        #region Arithmetic operators
        public static PersianDate operator +(PersianDate persianDate, int days)
        {
            long n = persianDate.n + days;
            try
            {
                return new PersianDate(checked((uint)n));
            }
            catch (OverflowException ex)
            {
                throw new OverflowException("The resulting date of the addition is outside of acceptable range.", ex);
            }
        }
        public static PersianDate operator -(PersianDate persianDate, int days)
        {
            return persianDate + (-days);
        }
        public static TimeSpan operator -(PersianDate x, PersianDate y)
        {
            long l = (long)x.n - y.n;
            return new TimeSpan(checked((int)l), 0, 0, 0);
        }
        #endregion

        uint n;//the only field, stores the number of days passed 1/1/1
        internal PersianDate(uint n)
        {
            this.n = n;
        }
        /// <summary>
        /// Initializes a new instance of the PersianDate structure set to the year,month, and day parameters.
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        public PersianDate(int year, int month, int day)
        {
            if (!IsValid(year, month, day))
                throw new ArgumentException(string.Format("The date ({0}/{1}/{2}) is not a valid date.", year, month, day));
            if (comp(year, month, day, minYear, minMonth, minDay) < 0)
                throw new ArgumentOutOfRangeException(string.Format("The date ({0}/{1}/{2}) is less than the minimun acceptable date", year, month, day));
            if (comp(year, month, day, maxYear, maxMonth, maxDay) > 0)
                throw new ArgumentOutOfRangeException(string.Format("The date ({0}/{1}/{2}) is greater than the maximum acceptable date", year, month, day));
            n = days(year, month, day);
        }
        /// <summary>
        /// Initializes a new instance of the PersianDate structure set to the persian date equivalent to the date specified in the dateTime parameter.
        /// </summary>
        /// <param name="dateTime"></param>
        public PersianDate(DateTime dateTime)
        {
            n = (uint)(dateTime.Date - firstDateDateTime).Days;
        }
        /// <summary>
        /// Gets the day of the month represented by this PersianDate instance.
        /// </summary>
        public int Day
        {
            get
            {
                int y, m, d;
                yearMonthDay(n, out y, out m, out d);
                return d;
            }
        }
        /// <summary>
        /// Gets the month represented by this PersianDate instance.
        /// </summary>
        public int Month
        {
            get
            {
                int y, m, d;
                yearMonthDay(n, out y, out m, out d);
                return m;
            }
        }
        /// <summary>
        /// Gets year represented by this PersianDate instance.
        /// </summary>
        public int Year
        {
            get
            {
                int y, m, d;
                yearMonthDay(n, out y, out m, out d);
                return y;
            }
        }
        /// <summary>
        /// Gets the month as PersianMonth represented by this PersianDate instance.
        /// </summary>
        public PersianMonth MonthAsPersianMonth
        {
            get
            {
                return (PersianMonth)Month;
            }
        }
        /// <summary>
        /// Gets the day of the week represented by this PersianDate instance.
        /// </summary>
        public DayOfWeek DayOfWeek
        {
            get
            {
                return (DayOfWeek)((n + 4) % 7);
            }
        }
        /// <summary>
        /// Gets the day of the week as PersianDayOfWeek represented by this PersianDate instance.
        /// </summary>
        public PersianDayOfWeek PersianDayOfWeek
        {
            get
            {
                return (PersianDayOfWeek)((n + 4) % 7);
            }
        }
        /// <summary>
        /// Gets the day of the year represented by this PersianDate instance.
        /// </summary>
        public int DayOfYear
        {
            get
            {
                int y, ds;
                yearAndDaysInYear(n, out y, out ds);
                return ds + 1;

            }
        }

        /// <summary>
        /// returns a new PersianDate resulting from adding the days specified to the current PersianDate
        /// </summary>
        /// <param name="days">number of days to be added to the current PersianDate</param>
        public PersianDate AddDays(int days)
        {
            return (this + days);
        }

        /// <summary>
        /// Converts the PersianDate value to its DateTime equivalent.
        /// </summary>
        /// <exception cref="OverflowException">
        ///  Throws when the conversion results in an un-representable DateTime
        /// </exception>
        /// <returns></returns>
        public DateTime ToDateTime()
        {
            try
            {
                return firstDateDateTime.AddDays(n);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw new OverflowException("The conversion results in an unrepresentable DateTime.", ex);
            }
        }
        /// <summary>
        /// Returns the String representation of the PersianDate value.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}/{1}/{2}", Year, Month, Day);
        }
        /// <summary>
        /// Returns the long date String representation of the PersianDate Value.
        /// </summary>
        /// <returns></returns>
        public string ToLongDateString()
        {
            return string.Format("‏" + "{3}، {2} {1} {0}", Year, MonthAsPersianMonth, Day, PersianDayOfWeek);
        }

        #region IComparable<PersianDate> Members

        public int CompareTo(PersianDate that)
        {
            return this.n.CompareTo(that.n);
        }

        #endregion


    }
}
