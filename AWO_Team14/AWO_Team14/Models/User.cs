using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace AWO_Team14.Models
{
    public enum UserType { Customers, Employees, Managers}
	public enum State { AL, AK, AZ, AR, CA, CO, CT, DE, FL, GA, HI, ID, IL, IN, IA, KS, KY, LA, ME, MD, MA, MI, MN, MS, MO, MT, NE, NV, NH, NJ, NM, NY, NC, ND, OH, OK, OR, PA, RI, SC, SD, TN, TX, UT, VT, VA, WA, WV, WI, WY } 

    public class User
    {
        public Int32 UserID { get; set; }

        public UserType UserType { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "First Name is required")]
        public String FirstName { get; set; }

		[Display(Name = "Middle Initial")]
		public String MiddleInitial { get; set; }

		[Display(Name = "Last Name")]
        [Required(ErrorMessage = "Last Name is required")]
        public String LastName { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Enter a valid email address.")]
        public String Email { get; set; }

        [Display(Name = "Street")]
        public String Street { get; set; }

		[Display(Name = "City")]
		public String City { get; set; }

		[Display(Name = "State")]
		public State State { get; set; }

		[StringLength(5)]
		[Display(Name = "Zip")]
		public String Zip { get; set; }

		[Display(Name = "Birthday")]
        [Required(ErrorMessage = "Birthday is required")]
        public DateTime Birthday { get; set; }

        [Display(Name = "Phone Number")]
        [Required(ErrorMessage = "Phone Number is required")]
        [DataType(DataType.PhoneNumber)]
        [DisplayFormat(DataFormatString = "{0:###-###-####}", ApplyFormatInEditMode = true)]
        public String PhoneNumer { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Password is required")]
        public String Password { get; set; }

        [Display(Name = "Credit Card")]
        // should validate credit card number for us 
        [Required(ErrorMessage = "Credit card is required and must be valid"), CreditCard]
        public String CreditCardNumber { get; set; }

        [Display(Name = "Popcorn Points")]
        // sets default value to 0
        public int PopcornPoints { get; set; } = 0;

        [Display(Name = "Archived")]
        // sets default value to false
        public Boolean Archived { get; set; } = false;

        public virtual List<MovieReview> MovieReviews { get; set; }
        public virtual List<Vote> Votes { get; set; }
        public virtual List<Transaction> Transactions { get; set; }

    }
}