using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace AWO_Team14.Models
{
	public class Discount
	{
		public Int32 DiscountID { get; set; } 

		[Display(Name = "Discount Name")]
		public String DiscountName { get; set; }

		[Display(Name = "Discount Value")]
		public Decimal DiscountValue { get; set; }
	}
}