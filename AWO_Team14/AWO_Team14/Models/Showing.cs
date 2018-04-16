using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace AWO_Team14.Models
{
	public enum Theater
	{
		[Description("Theater One")]
		One,
		[Description("Theater Two")]
		Two
	}
	public class Showing
	{
        public Int32 ShowingID { get; set; }

        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{MM/dd/yyyy}")]
        [DataType(DataType.Date)]
        [Display(Name = "Date")]
		public DateTime ShowDate { get; set; }

        //[DataType(DataType.Time)]
        ////[Range(typeof(DateTime), "9:00:00", "23:59:59",
        ////ErrorMessage = "Value for {0} must be between {1} and {2}")]
        //[Display(Name = "Start Time")]
        //public DateTime StartTime { get; set; }

        [Range(9, 23)]
        public Int32 StartHour { get; set; }

        [Range(0, 60)]
        public Int32 StartMinute { get; set; }

        [DataType(DataType.Time)]
        [Display(Name = "End Time")]
        public DateTime EndTime { get; set; }

        //Special??
        [Display(Name = "Special")]
		public Boolean Special { get; set; }

		[Display(Name = "Theater")]
		public Theater Theater { get; set; }

        public List<String> Seats { get; set; }

		//public virtual Schedule Schedule { get; set; }
		public virtual Movie Movie { get; set; }
		public virtual List<UserTicket> UserTickets { get; set; }

        public Showing()
        {
            if (UserTickets == null)
            {
                UserTickets = new List<UserTicket>();
            }
        }

	}
}