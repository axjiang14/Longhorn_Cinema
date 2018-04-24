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
        public static Boolean ShowingValidation(Showing showing)
        {
            AppDbContext db = new AppDbContext();

            //check that the movie ends before midnight
            if ((showing.EndTime.Day > showing.ShowDate.Day))
            {
                Debug.WriteLine("1");
                return false;
            }

            //check for overlap
            var overlapQuery = from s in db.Showings
                        select s;
            overlapQuery = overlapQuery.Where(s => s.Theater == showing.Theater);
            overlapQuery = overlapQuery.Where(s => (s.EndTime >= showing.ShowDate && s.ShowDate <= showing.ShowDate) || (s.EndTime >= showing.EndTime && s.ShowDate <= showing.EndTime));

            if (overlapQuery.Count() >0)
            {
                Debug.WriteLine("2");
                return false;
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
					Debug.WriteLine("duplicate showing in other theatre");
					return false;
				}
			}
				
            //showing is ok!
            return true;
        }

		public static Boolean DayShowingValidation(DateTime Date)
		{
			AppDbContext db = new AppDbContext();
			var dayQuery = from s in db.Showings
							   select s;
			dayQuery = dayQuery.Where(s => s.ShowDate.Day == Date.Day).OrderBy(s=>s.ShowDate);

			//the first movie must start between 9 AM and 10 AM
			if (dayQuery.FirstOrDefault().StartHour < 9 || dayQuery.FirstOrDefault().StartHour > 10)
			{
				Debug.WriteLine("The first movie must start between 9 and 10");
				return false;
			}

			//the last movie must end after 21:30
			if ((dayQuery.LastOrDefault().EndTime.Hour < 21 && dayQuery.LastOrDefault().EndTime.Minute < 30))
			{
				Debug.WriteLine("The last movie must end after 21:30");
				return false;
			}

            //check the gaps
            List<Showing> dayShowings = dayQuery.ToList();
            for (var i = 0; i < dayShowings.Count; i++)
            {
                TimeSpan dateTimeGap = dayShowings[i].ShowDate - dayShowings[i + 1].ShowDate;
                Int32 intGap = Convert.ToInt32(dateTimeGap);

                if (intGap < 25 || intGap > 45)
                {
                    Debug.WriteLine("The gap between", dayShowings[i].Movie.Title, "and", dayShowings[i + 1].Movie.Title, "must be between 25 and 45 minutes");
                    return false;
                }

            }

            return true;
		}
        

    }
}