using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AWO_Team14.Models;
using AWO_Team14.DAL;

namespace AWO_Team14.Utilities
{
    public static class ScheduleValidation
    {
        public static Boolean ShowingValidation(Showing showing)
        {
            AppDbContext db = new AppDbContext();

            //check that endtime < 12:00:00 AM
            if(showing.EndTime > showing.ShowDate.AddDays(1).AddSeconds(-1))
            {
                return false;
            }

            //check for overlap
            var overlapQuery = from s in db.Showings
                        select s;
            overlapQuery = overlapQuery.Where(s => s.Theater == showing.Theater);
            overlapQuery = overlapQuery.Where(s => (s.EndTime >= showing.StartTime && s.StartTime <= showing.StartTime) || (s.EndTime >= showing.EndTime && s.StartTime <= showing.EndTime));
            if(overlapQuery.Count() >0)
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
                
            query = query.Where(s => s.StartTime == showing.StartTime);
            if(query.Count() > 0)
            {
                return false;
            }

            //showing is ok!
            return true;
        }
        

    }
}