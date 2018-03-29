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
		TheaterOne,
		[Description("Theater Two")]
		TheaterTwo
	}
	public class Showing
	{
		public Int32 ShowingID { get; set; }

		[Display(Name = "Date")]
		public DateTime ShowDate { get; set; }

		[Display(Name = "Show Time")]
		public Int32 ShowTime { get; set; }
		
		//Special??
		[Display(Name = "Special")]
		public Boolean Special { get; set; }

		[Display(Name = "Theater")]
		public Theater Theater { get; set; }

		public virtual Schedule Schedule { get; set; }
		public virtual List<Movie> Movies { get; set; }
		public virtual List<UserTicket> UserTickets { get; set; }

	}
}