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
            if (showing.Theater == Theater.One)
            {
                query = query.Where(s => s.Theater == Theater.Two);
            }
            else
            {
                query = query.Where(s => s.Theater == Theater.One);
            }

            query = query.Where(s => s.ShowDate == showing.ShowDate);
            Debug.WriteLine(query.ToList());
            query = query.Where(s => s.Movie == showing.Movie);
            Debug.WriteLine(query.ToList());


            if (query.ToList().Count == 0)
            {
                Debug.WriteLine("duplicate showing in other theatre");
                return false;
            }


            Debug.WriteLine("Gucci");
            //showing is ok!
            return true;
        }
        

    }
}