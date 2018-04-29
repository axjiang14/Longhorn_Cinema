using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace AWO_Team14.Models
{
    public enum MPAA { G, PG, PG13, R, NC17, Unrated, All}

    public class Movie
    {
        
        [Display(Name = "Movie ID")]
        public Int32 MovieID { get; set; }

        // starts at 3000
        [Display(Name = "Movie Number")]
        public Int32 MovieNumber { get; set; }

        [Required(ErrorMessage ="Title is required")]
        [Display(Name = "Title")]
        public String Title { get; set; }

        //[Required(ErrorMessage = "Tagline is required")]
        [Display(Name = "Tagline")]
        public String Tagline { get; set; }

        [Required(ErrorMessage = "Overview is required")]
        [Display(Name = "Overview")]
        public String Overview { get; set; }

        [Required(ErrorMessage = "Release Year is required")]
        [Display(Name = "Release Year")]
        public Int32 ReleaseYear { get; set; }

        [Required(ErrorMessage = "MPAA Rating is required")]
        [Display(Name = "MPAA Rating")]
        public MPAA MPAA_Rating { get; set; }

        [Required(ErrorMessage = "Runtime is required")]
        [Display(Name = "Run Time")]
        public TimeSpan Runtime { get; set; }

        [Required(ErrorMessage = "Actors are required")]
        public String Actors { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.0}")]
        [Display(Name = "Ratings Average")]
        public Decimal RatingsAvg
        {
            get
            {
                if (MovieReviews.Count() == 0)
                {
                    return 0;
                }
                else
                {
                    Double dblAvg =  MovieReviews.Average(m => (int)m.Rating);
                    Decimal decAvg = Convert.ToDecimal(dblAvg);
                    return decAvg;
                }
                
            }
        }

        public virtual List<Genre> Genres { get; set; } 
        public virtual List<MovieReview> MovieReviews { get; set; }
        public virtual List<Showing> Showings { get; set; }

        public Movie()
        {
            if (MovieReviews == null)
            {
                MovieReviews = new List<MovieReview>();
            }

            if (Showings == null)
            {
                Showings = new List<Showing>();
            }

			if (Genres == null)
			{
				Genres = new List<Genre>();
			}
		}
        
       


    }
}