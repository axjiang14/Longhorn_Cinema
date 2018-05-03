using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

//Change this namespace to match your project
namespace AWO_Team14.Models
{
   
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {

        //Add any fields that you need for creating a new user
        //Additional fields go here (First name is an example)
        //Remember, there is already a field for email, phone number, and password
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

        [Display(Name = "Birthday")]
        [Required(ErrorMessage = "Birthday is required")]
        public DateTime Birthday { get; set; }

        //[Display(Name = "Credit Card 1")]
        //// should validate credit card number for us 
        //[DataType(DataType.CreditCard)]
        //public String CreditCardNumber1 { get; set; }

        //[Display(Name = "Credit Card 2")]
        //// should validate credit card number for us 
        //[DataType(DataType.CreditCard)]
        //public String CreditCardNumber2 { get; set; }

        //[Display(Name = "Popcorn Points")]
        //// sets default value to 0
        //public int PopcornPoints { get; set; } = 0;

        [Display(Name = "Archived")]
        // sets default value to false
        public Boolean Archived { get; set; } = false;


        //NOTE: Here is the property for email
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        //NOTE: Here is the property for phone number
        [Required(ErrorMessage = "Phone number is required")]
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        //NOTE: Here is the logic for putting in a password
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class IndexViewModel
    {
        public bool HasPassword { get; set; }
        public String UserName { get; set; }
        public String Email { get; set; }
        public String UserID { get; set; }
    }
}
