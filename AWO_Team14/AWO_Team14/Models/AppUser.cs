using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

//Change this namespace to match your project
namespace AWO_Team14.Models
{
    public enum State { AL, AK, AZ, AR, CA, CO, CT, DE, FL, GA, HI, ID, IL, IN, IA, KS, KY, LA, ME, MD, MA, MI, MN, MS, MO, MT, NE, NV, NH, NJ, NM, NY, NC, ND, OH, OK, OR, PA, RI, SC, SD, TN, TX, UT, VT, VA, WA, WV, WI, WY }
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.

    //NOTE: This is the class for users
    public class AppUser : IdentityUser
    {
        //Put any additional fields that you need for your user here
        //First name is here as an example
        [Required(ErrorMessage = "First name is required.")]
        [Display(Name = "First Name")]
        public String FirstName { get; set; }

        [Display(Name = "Middle Initial")]
        public String MiddleInitial { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
		[Display(Name = "Last Name")]
		public String LastName { get; set; }

        [Required(ErrorMessage = "Street is required.")]
        [Display(Name = "Street")]
        public String Street { get; set; }

        [Required(ErrorMessage = "City is required.")]
        [Display(Name = "City")]
        public String City { get; set; }

        [Required(ErrorMessage = "State is required.")]
        [Display(Name = "State")]
        public State State { get; set; }

        [Required(ErrorMessage = "Zip is required.")]
        [StringLength(5)]
        [Display(Name = "Zip")]
        public String Zip { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Birthday")]
        [Required(ErrorMessage = "Birthday is required")]
        public DateTime Birthday { get; set; }

        [Display(Name = "Credit Card 1")]
        // should validate credit card number for us 
        [DataType(DataType.CreditCard)]
        public String CreditCardNumber1 { get; set; }

        [Display(Name = "Credit Card 2")]
        // should validate credit card number for us 
        [DataType(DataType.CreditCard)]
        public String CreditCardNumber2 { get; set; }

		[Display(Name = "Popcorn Points")]
        // sets default value to 0
        public int PopcornPoints { get; set; } = 0;

        [Display(Name = "Archived")]
        // sets default value to false
        public Boolean Archived { get; set; } = false;

        //Add any navigational properties needed for your user
        //Orders is here as an example
        //TODO: ??? this looks weird ???
        public DbSet<Transaction> Transactions { get; set; }


        public virtual List<MovieReview> MovieReviews { get; set; }
        public virtual List<Vote> Votes { get; set; }

        public AppUser()
        {
            if (MovieReviews == null)
            {
                MovieReviews = new List<MovieReview>();
            }
            if (Votes == null)
            {
                Votes = new List<Vote>();
            }

        }


        //This method allows you to create a new user
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<AppUser> manager)
        {
            // NOTE: The authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

	}
}