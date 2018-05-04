using AWO_Team14.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AWO_Team14.Utilities
{
    public class GenerateScheduleDay
    {
        public static DateTime GetNextSchedDate()
        {
            AppDbContext db = new AppDbContext();

            DateTime MaxDate;
            DateTime NextDate;

            if (db.Schedules.Count() == 0)
            {
                MaxDate = new DateTime(2018,5,03);
            }
            else
            {
                MaxDate = db.Schedules.Max(s => s.EndDate);
            }

            NextDate = MaxDate.AddDays(1);

            return NextDate;
        }
    }
}