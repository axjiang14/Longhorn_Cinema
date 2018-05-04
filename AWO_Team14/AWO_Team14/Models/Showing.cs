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

    public enum Seat
    {
        Seat, A1, A2, A3, A4, A5, A6, A7, A8,
        B1, B2, B3, B4, B5, B6, B7, B8,
        C1, C2, C3, C4, C5, C6, C7, C8,
        D1, D2, D3, D4, D5, D6, D7, D8
    }
	public class Showing
	{
        public Int32 ShowingID { get; set; }

        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{MM/dd/yyyy}")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date")]
		public DateTime ShowDate { get; set; }

        //[DataType(DataType.Time)]
        ////[Range(typeof(DateTime), "9:00:00", "23:59:59",
        //[ErrorMessage = "Value for {0} must be between {1} and {2}")]
        //[Display(Name = "Start Time")]
        //public DateTime StartTime { get; set; }

        [Range(9, 23)]
        [Required(ErrorMessage = "Your movie must begin from the hours of 9:00 to 23:00")]
        public Int32 StartHour { get; set; }

        [Range(0, 60)]
        [Required(ErrorMessage = "Your minutes for a movie must be between 0 and 60")]
        public Int32 StartMinute { get; set; }

        [DataType(DataType.Time)]
        [Display(Name = "End Time")]
        public DateTime EndTime { get; set; }

        //Special??
        [Display(Name = "Special")]
		public Boolean Special { get; set; }

        public Decimal ShowingPrice { get; set; }

		[Display(Name = "Theater")]
		public Theater Theater { get; set; }

        public List<Seat> Seats { get; set; }

		public virtual Schedule Schedule { get; set; }
		public virtual Movie Movie { get; set; }
		public virtual List<UserTicket> UserTickets { get; set; }

        public Showing()
        {
            if (UserTickets == null)
            {
                UserTickets = new List<UserTicket>();
            }
            //if (Schedule == null)
            //{
            //    Schedule = new Schedule();
            //}
        }

	}
}