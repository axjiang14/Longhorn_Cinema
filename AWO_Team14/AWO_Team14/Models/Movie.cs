using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace AWO_Team14.Models
{
    public enum MPAA { G, PG, PG13, R, Unrated}

    public class Movie
    {
        
        [Display(Name = "Movie ID")]
        public Int32 MovieID { get; set; }

        // starts at 3000
        [Display(Name = "Movie Number")]
        [Required(ErrorMessage = "Movie Number is required")]
        public Int32 MovieNumber { get; set; }

        [Required(ErrorMessage ="Title is required")]
        [Display(Name = "Title")]
        public String Title { get; set; }

        [Required(ErrorMessage = "Tagline is required")]
        [Display(Name = "Tagline")]
        public String Tagline { get; set; }

        [Required(ErrorMessage = "Overview is required")]
        [Display(Name = "Overview")]
        public String Overview { get; set; }

        [Required(ErrorMessage = "Release Year is required")]
        [Display(Name = "Release Year")]
        public DateTime ReleaseYear { get; set; }

        [Required(ErrorMessage = "MPAA Rating is required")]
        [Display(Name = "MPAA Rating")]
        public MPAA MPAA_Rating { get; set; }

        [Required(ErrorMessage = "Runtime is required")]
        [Display(Name = "Run Time")]
        public Int32 Runtime { get; set; }

        [Required(ErrorMessage = "Actors are required")]
        public String Actors { get; set; }

        public virtual List<Genre> Genres { get; set; } 
        //public virtual List<MovieReview> MovieReviews { get; set; }
        //public virtual List<Showing> Showings { get; set; }


    }
}