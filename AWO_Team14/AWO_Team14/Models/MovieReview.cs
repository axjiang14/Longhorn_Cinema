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
    public class MovieReview
    {
        public Int32 MovieReviewID { get; set; }

        public Rating Rating { get; set; }
        
        public String Review { get; set; }

        public virtual Movie Movie { get; set; }
        public virtual User User { get; set; }
        public virtual List<Vote> Votes { get; set; }

    }
}