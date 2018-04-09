using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AWO_Team14.DAL;

namespace AWO_Team14.Utilities
{
    public class GenerateMovieNumber
    {
        public static Int32 GetNextMovieNum()
        {
            AppDbContext db = new AppDbContext();

            Int32 MaxMovieNum;
            Int32 NextMovieNum;

            if (db.Movies.Count() == 0)
            {
                MaxMovieNum = 3000;
            }
            else
            {
                MaxMovieNum = db.Movies.Max(m => m.MovieNumber);   
            }

            NextMovieNum = MaxMovieNum + 1;

            return NextMovieNum;
        }
    }
}