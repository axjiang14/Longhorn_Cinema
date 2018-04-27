using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace AWO_Team14.Models
{
	public enum Payment { PopcornPoints, CreditCard }
    

    public class Transaction
	{
        private const Decimal SalesTaxRate = 0.0825m;

        public Int32 TransactionID { get; set; }

		[Display(Name = "Payment")]
		public Payment Payment { get; set; }

		[Display(Name = "Transaction Date")]
		public DateTime TransactionDate { get; set; }

        public Decimal Subtotal
        {
            get { return UserTickets.Sum(ut => ut.CurrentPrice); }
        }

        [Display(Name = "Sales Tax")]
        public Decimal SalesTax
        {
            get { return Subtotal * SalesTaxRate; }
        }

        [Display(Name = "Order Total")]
        [DisplayFormat(DataFormatString = "{0:c}")]
        public Decimal Total
        {
            get { return Subtotal + SalesTax; }
        }

        public virtual AppUser User { get; set; }
		public virtual List<UserTicket> UserTickets { get; set; } 

	}
}