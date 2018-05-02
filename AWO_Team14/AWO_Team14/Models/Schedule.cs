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

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; } 


        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

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