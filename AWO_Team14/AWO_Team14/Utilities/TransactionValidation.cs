using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AWO_Team14.Models;
using AWO_Team14.DAL;
using System.Diagnostics;

namespace AWO_Team14.Utilities
{
	public class TransactionValidation
	{
		public static Boolean TicketValidation(Transaction transaction)
		{
			AppDbContext db = new AppDbContext();

			for (var t = 0; t < transaction.UserTickets.Count; t++)
			{
				UserTicket TicketToCheck = transaction.UserTickets[t];

				//check user tickets
				for (var x = t + 1; x < transaction.UserTickets.Count; x++)
				{
					var overlapQuery = from s in db.Showings
									   select s;

					overlapQuery = overlapQuery.Where(s => (s.EndTime >= TicketToCheck.Showing.ShowDate && s.ShowDate <= TicketToCheck.Showing.ShowDate) || (s.EndTime >= TicketToCheck.Showing.EndTime && s.ShowDate <= TicketToCheck.Showing.EndTime));

					if (overlapQuery.Count() > 0)
						return false;
				}
			}
			return true;

		}

		public static Int32 AgeCalc(DateTime Birthday)
		{
			int years = DateTime.Now.Year - Birthday.Year;

			if ((Birthday.Month > DateTime.Now.Month) || (Birthday.Month == DateTime.Now.Month && Birthday.Day > DateTime.Now.Day))
						years--;

			return years;
		}

		//transaction as parameter - pull user and # tickets (if true return to database and false)

		public static Boolean PPCalc(Transaction transaction)
		{
			Int32 NumberofTickets = transaction.UserTickets.Count();

			Int32 RequiredPP = NumberofTickets * 100;

			Int32 CurrentPP = transaction.User.PopcornPoints;

			if (RequiredPP <= CurrentPP)
			{
				return true;
			}
			return false;

		}
				
	}
}