using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AWO_Team14.Models;
using AWO_Team14.DAL;
using System.Diagnostics;
using System.Data.Entity;

namespace AWO_Team14.Utilities
{
    public static class ScheduleValidation
    {
        public static String ShowingValidation(Showing showing)
        {
            AppDbContext db = new AppDbContext();

            //only make showings that will happen in the future
            if (showing.ShowDate < DateTime.Now)
            {
                return "You cannot schedule a showing that will happen in the past";
            }

            //check that the movie ends before midnight
            if (showing.EndTime.Date > showing.ShowDate.Date)
            {
                return "The showing needs to end before midnight";
            }

            //check for overlap
            var overlapQuery = from s in db.Showings
                        select s;
            //filter only active showings
            overlapQuery = overlapQuery.Where(s => s.Schedule != null);
            
            overlapQuery = overlapQuery.Where(s => s.Theater == showing.Theater);
            
            overlapQuery = overlapQuery.Where(s => (s.EndTime >= showing.ShowDate && s.ShowDate <= showing.ShowDate) || (s.EndTime >= showing.EndTime && s.ShowDate <= showing.EndTime));

            if (overlapQuery.Count() >= 1 && overlapQuery.FirstOrDefault().ShowingID != showing.ShowingID)
            {
                return "This showing overlaps with an existing showing";
            }

            //check for duplicate showing in other theatre
            var query = from s in db.Showings
                        select s;

            //filter only active showings
            query = query.Where(s => s.Schedule !=null && s.ShowingID != showing.ShowingID);
            //find showings in the other theater
			//query = query.Where(s => s.Theater != showing.Theater);
			//find showings that are showing at the same time
            query = query.Where(s => s.ShowDate == showing.ShowDate);
           


            //check if they are showing the same movie
            if (query.ToList().Count() > 0)
			{
                foreach (Showing show in query.ToList())
                {
                    if (showing.Movie.Title == show.Movie.Title)
                    {
                        return "This showing is a duplicate of another showing in either this theater or the other theater.";
                    }
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
            dayQuery = dayQuery.Where(s => s.Schedule != null);
            dayQuery = dayQuery.Where(s => s.Theater == theater);
            dayQuery = dayQuery.Where(s => DbFunctions.TruncateTime(s.ShowDate) == DbFunctions.TruncateTime(Date) && s.Theater == theater).OrderBy(s=>s.ShowDate);
            List<Showing> dayShowings = dayQuery.ToList();

            if (dayShowings.Count() == 0)
            {
                return "You haven't scheduled any movies!";
            }

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
                    String ErrorMessage = "The gap between " + dayShowings[i].Movie.Title + " and " + dayShowings[i + 1].Movie.Title + " must be between 25 and 45 minutes";
                    //Debug.WriteLine("The gap between ", dayShowings[i].Movie.Title, " and ", dayShowings[i + 1].Movie.Title, "must be between 25 and 45 minutes");
                    return ErrorMessage;
                }

            }

            return "ok";
		}
        
        public static Boolean ShowingInRange(Showing showing)
        {
            Boolean bolInRange = (showing.ShowDate.Date >= showing.Schedule.StartDate.Date) && (showing.ShowDate.Date <= showing.Schedule.EndDate.Date);

            return bolInRange;
        }

        //public static String RescheduleValidation(Showing showing)
        //{
        //    AppDbContext db = new AppDbContext();

        //    DateTime dayToCheck = showing.ShowDate.Date;

        //    var query = db.Showings.Where(s => DbFunctions.TruncateTime(s.ShowDate) == DbFunctions.TruncateTime(dayToCheck));
        //    List<Showing> dayShowings = query.ToList();

        //    //the first movie must start between 9 AM and 10 AM
        //    DateTime date9 = new DateTime(2018, 1, 1, 9, 00, 00);
        //    DateTime date10 = new DateTime(2018, 1, 1, 10, 00, 00);

        //    if (dayShowings.FirstOrDefault().ShowDate.TimeOfDay < date9.TimeOfDay || dayShowings.FirstOrDefault().ShowDate.TimeOfDay > date10.TimeOfDay)
        //    {
        //        return "The first movie must start between 9:00 and 10:00";
        //    }

        //    //the last movie must end after 21:30
        //    DateTime date2130 = new DateTime(2018, 1, 1, 21, 30, 00);
        //    if (dayShowings.Count() > 0)
        //    {
        //        if (dayShowings[dayShowings.Count() - 1].EndTime.TimeOfDay < date2130.TimeOfDay)
        //        {
        //            return "The last movie must end after 21:30";
        //        }
        //    }


        //    return "Your showing has been rescheduled";
        //}

        public static String RescheduleValidation(DateTime newShowDate, DateTime newEndTime, Theater newTheater)
        { 
            AppDbContext db = new AppDbContext();

            DateTime dayToCheck = newShowDate.Date;

            //var query = db.Showings.Where(s => s.ShowDate.DayOfYear == dayToCheck.DayOfYear);
            var query = db.Showings.Where(s => DbFunctions.TruncateTime(s.ShowDate) == DbFunctions.TruncateTime(dayToCheck));
            query = query.Where(s => s.Theater == newTheater);
            List<Showing> dayShowings = query.OrderBy(s => s.ShowDate).ToList();


            //the first movie must start between 9 AM and 10 AM
            DateTime date9 = new DateTime(2018, 1, 1, 9, 00, 00);
            DateTime date10 = new DateTime(2018, 1, 1, 10, 00, 00);

            if (dayShowings.FirstOrDefault().ShowDate.TimeOfDay < date9.TimeOfDay || dayShowings.FirstOrDefault().ShowDate.TimeOfDay > date10.TimeOfDay)
            {
                return "The first movie must start between 9:00 and 10:00";
            }
            {
                if (newShowDate.TimeOfDay > date10.TimeOfDay)
                {
                    return "The first movie must start between 9 AM and 10 AM";
                }
            }

            //the last movie must end after 21:30
            DateTime date2130 = new DateTime(2018, 1, 1, 21, 30, 00);
            if (dayShowings.Count() > 0)
            {
                if (dayShowings[dayShowings.Count() - 1].EndTime.TimeOfDay < date2130.TimeOfDay)
                {
                    if (newEndTime.TimeOfDay < date2130.TimeOfDay)
                    {
                        return "The last movie must end after 21:30";
                    }
                    
                }
            }

            //check gaps
            var showingsBefore = dayShowings.Where(s => s.EndTime < newShowDate);
            var showingsAfter = dayShowings.Where(s => s.ShowDate < newEndTime);

            List<Showing> lstShowingsBefore = showingsBefore.OrderBy(s=> s.ShowDate).ToList();
            List<Showing> lstShowingsAfter = showingsAfter.OrderBy(s => s.ShowDate).ToList();

            //check the first showing that's before you
            if (showingsBefore.Count() > 0)
            {
                TimeSpan beforeGap =  - (lstShowingsBefore[lstShowingsBefore.Count() - 1].EndTime - newShowDate);
                Int32 intBeforeGap = Convert.ToInt32(beforeGap);

                if (intBeforeGap > 45 || intBeforeGap < 25)
                {
                    return "The gap between your movies must be between 25 and 45 minutes";
                }
            }

            //check the first showing that's before you
            if (showingsAfter.Count() > 0)
            {
                TimeSpan afterGap = lstShowingsAfter[lstShowingsAfter.Count() - 1].ShowDate - newEndTime;
                Int32 intAfterGap = Convert.ToInt32(afterGap);

                if (intAfterGap > 45 || intAfterGap < 25)
                {
                    return "The gap between your movies must be between 25 and 45 minutes";
                }
            }


            return "ok";
        }
    }
}