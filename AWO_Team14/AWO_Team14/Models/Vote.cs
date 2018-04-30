using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace AWO_Team14.Models
{
    public enum ThumbVote { Up, Down }

    public class Vote
    {
        public Int32 VoteID { get; set; }

        public ThumbVote ThumbVote { get; set; }

        public virtual MovieReview MovieReview { get; set; }

        public virtual AppUser User { get; set; }



    }
}