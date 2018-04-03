using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace AWO_Team14.Models
{
	public class Schedule
	{
		public Int32 ScheduleID { get; set; } 

		[Display(Name = "Published")]
		public Boolean Published { get; set; }

		public virtual List<Showing> Showings { get; set; }

        public Schedule()
        {
            if (Showings == null)
            {
                Showings = new List<Showing>();
            }
        }
	}
}