using System;

namespace LawyerProject.BLL
{
    /// <summary>
    /// "Now" for the whole app, taken from the database server rather than the local PC, so a
    /// workstation with a wrong clock cannot mark steps on the wrong day or write wrong audit times.
    /// </summary>
    public static class Clock
    {
        private static TimeSpan offset = TimeSpan.Zero;

        public static DateTime Now
        {
            get { return DateTime.Now + offset; }
        }

        public static DateTime Today
        {
            get { return Now.Date; }
        }

        /// <summary>The difference between this PC's clock and the server (positive = the PC is behind).</summary>
        public static TimeSpan Offset
        {
            get { return offset; }
        }

        /// <summary>Aligns the app with the server's time. Returns the difference that was found.</summary>
        public static TimeSpan SetServerTime(DateTime serverNow)
        {
            offset = serverNow - DateTime.Now;
            if (Math.Abs(offset.TotalSeconds) < 2) offset = TimeSpan.Zero; // ignore network delay
            return offset;
        }

        public static void Reset()
        {
            offset = TimeSpan.Zero;
        }
    }
}
