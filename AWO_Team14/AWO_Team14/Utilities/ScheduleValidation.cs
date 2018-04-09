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
            DateTime LatestEnd = new DateTime(1900, 1, 1, 23, 59, 59);

            //Movie movie = showing.Movie;
            
            //check that endtime < 12:00:00 AM
           
            if ((showing.EndTime.TimeOfDay > LatestEnd.TimeOfDay)||(showing.EndTime.TimeOfDay < showing.StartTime.TimeOfDay))
            {               
                return false;
            }

            //check for overlap
            var overlapQuery = from s in db.Showings
                        select s;
            overlapQuery = overlapQuery.Where(s => s.Theater == showing.Theater);
            overlapQuery = overlapQuery.Where(s => (s.EndTime.TimeOfDay >= showing.StartTime.TimeOfDay && s.StartTime.TimeOfDay <= showing.StartTime.TimeOfDay) || (s.EndTime.TimeOfDay >= showing.EndTime.TimeOfDay && s.StartTime.TimeOfDay <= showing.EndTime.TimeOfDay));

          

            if (overlapQuery.Count() >0)
            {
                return false;
            }

            //check for duplicate showing in other theatre
            var query = from s in db.Showings
                        select s;
            if (showing.Theater == Theater.One)
            {
                query = query.Where(s => s.Theater == Theater.Two);
            }
            else
            {
                query = query.Where(s => s.Theater == Theater.One);
            }
                
            query = query.Where(s => s.StartTime.TimeOfDay == showing.StartTime.TimeOfDay);
            query = query.Where(s => s.Movie == showing.Movie);


            if(query.Count() > 0)
            {
                return false;
            }

            //showing is ok!
            return true;
        }
        

    }
}