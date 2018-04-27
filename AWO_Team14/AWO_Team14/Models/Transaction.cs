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
		public Int32 TransactionID { get; set; }

		[Display(Name = "Payment")]
		public Payment Payment { get; set; }

		[Display(Name = "Transaction Date")]
		public DateTime TransactionDate { get; set; }

        public Decimal Total { get; set; }

		public virtual AppUser User { get; set; }
		public virtual List<UserTicket> UserTickets { get; set; } 

	}
}