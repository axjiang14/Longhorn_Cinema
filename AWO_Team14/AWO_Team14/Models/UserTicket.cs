using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace AWO_Team14.Models
{
    public class UserTicket
    {
        public Int32 UserTicketID { get; set; }
        
        [Display(Name = "Price")]
        public Decimal CurrentPrice { get; set;}

        [Display(Name = "Seat Number")]
        [Required(ErrorMessage = "Seat Number is required")]
        public String SeatNumber { get; set; }




    }
}