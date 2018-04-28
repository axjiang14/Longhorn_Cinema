using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace AWO_Team14.Models
{
    //Rating is a decimal between 1.0 and 5.0
    //Data annotation for restricting values
    public enum Rating
    {
        [Description("1")]
        One = 1,
        [Description("2")]
        Two = 2,
        [Description("3")]
        Three = 3,
        [Description("4")]
        Four = 4,
        [Description("5")]
        Five = 5
    }

	public enum ReviewStatus
	{
		Pending, Approved, Rejected
	}

	public class MovieReview
    {
        public Int32 MovieReviewID { get; set; }

        [Required(ErrorMessage = "You must give the movie a rating")]
        public Rating Rating { get; set; }
        
        [StringLength(100)]
        public String Review { get; set; }

		public ReviewStatus Status { get; set; }

		public virtual Movie Movie { get; set; }
        public virtual AppUser User { get; set; }
        public virtual List<Vote> Votes { get; set; }

        public MovieReview()
        {
            if (Votes == null)
            {
                Votes = new List<Vote>();
            }

        }

    
    }
}