using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace AWO_Team14.Models
{
	public enum Payment
	{
		PopcornPoints, CreditCardNumber1, CreditCardNumber2, OtherCreditCard
	}


	public class Transaction
	{
		private const Decimal SalesTaxRate = 0.0825m;

		public Int32 TransactionID { get; set; }

        public Int32 TransactionNumber { get; set; }

        public Int32 PopcornPointsSpent { get; set; }

		[Display(Name = "Payment")]
		public Payment Payment { get; set; }

        [Display(Name = "Payment Used")]
        public String PaymentUsed { get; set; }

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
		public virtual AppUser Giftee { get; set; }
		public virtual List<UserTicket> UserTickets { get; set; }

		public Transaction()
		{
			if (UserTickets == null)
			{
				UserTickets = new List<UserTicket>();
			}
		}

	}
}