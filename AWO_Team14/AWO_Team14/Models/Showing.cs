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

        [DataType(DataType.Time)]
        [Display(Name = "Show Time")]
		public DateTime ShowTime { get; set; }
		
		//Special??
		[Display(Name = "Special")]
		public Boolean Special { get; set; }

		[Display(Name = "Theater")]
		public Theater Theater { get; set; }

		//public virtual Schedule Schedule { get; set; }
		public virtual Movie Movie { get; set; }
		//public virtual List<UserTicket> UserTickets { get; set; }

        //public Showing()
        //{
        //    if (UserTickets == null)
        //    {
        //        UserTickets = new List<UserTicket>();
        //    }
        //}

	}
}