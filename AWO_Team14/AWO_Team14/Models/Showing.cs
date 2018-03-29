using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace AWO_Team14.Models
{
	public enum Theater
	{
		[Description("1")]
		TheaterOne = 1,
		[Description("2")]
		TheaterTwo = 2
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
		public Enum Theater { get; set; }

		public virtual Schedule Schedule { get; set; }
		public virtual List<Movie> Movies { get; set; }
		public virtual List<UserTicket> UserTickets { get; set; }

	}
}