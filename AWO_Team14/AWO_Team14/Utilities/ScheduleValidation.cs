using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AWO_Team14.Models;
using AWO_Team14.DAL;
using System.Diagnostics;

namespace AWO_Team14.Utilities
{
    public static class ScheduleValidation
    {
        public static String ShowingValidation(Showing showing)
        {
            AppDbContext db = new AppDbContext();

            //check that the movie ends before midnight
            if ((showing.EndTime.Day > showing.ShowDate.Day))
            {
                return "The showing needs to end before midnight";
            }

            //check for overlap
            var overlapQuery = from s in db.Showings
                        select s;
            overlapQuery = overlapQuery.Where(s => s.Theater == showing.Theater);
            overlapQuery = overlapQuery.Where(s => (s.EndTime >= showing.ShowDate && s.ShowDate <= showing.ShowDate) || (s.EndTime >= showing.EndTime && s.ShowDate <= showing.EndTime));

            if (overlapQuery.Count() >0)
            {
                return "This showing overlaps with an existing showing";
            }

            //check for duplicate showing in other theatre
            var query = from s in db.Showings
                        select s;

			//find showings in the other theater
			query = query.Where(s => s.Theater != showing.Theater);
			//find showings that are showing at the same time
            query = query.Where(s => s.ShowDate == showing.ShowDate);

			//check if they are showing the same movie
			if (query.Count() == 1)
			{
				Showing OtherShowing = query.FirstOrDefault();
				
				if (showing.Movie.Title == OtherShowing.Movie.Title)
				{
					return "This showing is a duplicate of another showing in the other theater.";
				}
			}
				
            //showing is ok!
            return "ok";
        }

		public static String DayShowingValidation(DateTime Date, Theater theater)
		{
			AppDbContext db = new AppDbContext();
			var dayQuery = from s in db.Showings
							   select s;
            dayQuery = dayQuery.Where(s => s.Theater == theater);
            dayQuery = dayQuery.Where(s => s.ShowDate.Day == Date.Day && s.Theater == theater).OrderBy(s=>s.ShowDate);
            List<Showing> dayShowings = dayQuery.ToList();

            //the first movie must start between 9 AM and 10 AM
            DateTime date9 = new DateTime(2018, 1, 1, 9, 00, 00);
            DateTime date10 = new DateTime(2018, 1, 1, 10, 00, 00);

            if (dayShowings.FirstOrDefault().ShowDate.TimeOfDay < date9.TimeOfDay || dayShowings.FirstOrDefault().ShowDate.TimeOfDay > date10.TimeOfDay)
            { 
				return "The first movie must start between 9:00 and 10:00";
			}

            //the last movie must end after 21:30
            DateTime date2130 = new DateTime(2018, 1, 1, 21, 30, 00);
            if (dayShowings.Count() >0)
            {
                if (dayShowings[dayShowings.Count() - 1].EndTime.TimeOfDay < date2130.TimeOfDay)
                {
                    return "The last movie must end after 21:30";
                }
            }

            //check the gaps          
            for (var i = 0; i+1 < dayShowings.Count; i++)
            {
                TimeSpan dateTimeGap = dayShowings[i+1].ShowDate - dayShowings[i].EndTime;
                Int32 intGap = Convert.ToInt32(dateTimeGap.Minutes);

                Debug.WriteLine(dateTimeGap);
                Debug.WriteLine(intGap);

                if (intGap < 25 || intGap > 45)
                {
                    String ErrorMessage = "The gap between " + dayShowings[i].Movie.Title + " and " + dayShowings[i + 1].Movie.Title + "must be between 25 and 45 minutes";
                    //Debug.WriteLine("The gap between ", dayShowings[i].Movie.Title, " and ", dayShowings[i + 1].Movie.Title, "must be between 25 and 45 minutes");
                    return ErrorMessage;
                }

            }

            return "ok";
		}
        
        public static Boolean ShowingInRange(Showing showing)
        {
            Boolean bolInRange = (showing.ShowDate >= showing.Schedule.StartDate) && (showing.ShowDate <= showing.Schedule.EndDate);

            return bolInRange;
        }
    }
}