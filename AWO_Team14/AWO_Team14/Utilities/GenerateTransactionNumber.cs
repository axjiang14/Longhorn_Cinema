using AWO_Team14.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AWO_Team14.Utilities
{
	public class GenerateTransactionNumber
	{
			public static Int32 GetNextTransactionNum()
			{
				AppDbContext db = new AppDbContext();

				Int32 MaxTransactionNum;
				Int32 NextTransactionNum;

				if (db.Transactions.Count() == 0)
				{
					MaxTransactionNum = 10000;
				}
				else
				{
					MaxTransactionNum = db.Transactions.Max(t => t.TransactionID);
				}

				NextTransactionNum = MaxTransactionNum + 1;

				return NextTransactionNum;
			}

	}
}