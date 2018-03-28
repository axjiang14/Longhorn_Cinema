using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace AWO_Team14.Models
{
    public class Genre
    {
        public Int32 GenreID { get; set; }

        [Required(ErrorMessage ="Genre Name is required.")]
        [Display(Name = "Genre")]
        public String GenreName { get; set; }

        public virtual List<Movie> Movies { get; set; }
    }
}