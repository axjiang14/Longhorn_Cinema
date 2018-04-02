//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using AWO_Team14.Models;
//using AWO_Team14.DAL;
//using System.Data.Entity.Migrations;

//namespace AWO_Team14.Migrations
//{

//	public class UserData
//	{
//		//public enum UserType { Customers, Employees, Managers }
//		//public enum State { AL, AK, AZ, AR, CA, CO, CT, DE, FL, GA, HI, ID, IL, IN, IA, KS, KY, LA, ME, MD, MA, MI, MN, MS, MO, MT, NE, NV, NH, NJ, NM, NY, NC, ND, OH, OK, OR, PA, RI, SC, SD, TN, TX, UT, VT, VA, WA, WV, WI, WY }

//		public void SeedUsers(AppDbContext db)
//		{
//			User u1 = new User();
//			u1.UserType = Models.UserType.Customers;
//			u1.FirstName = "Christopher";
//			u1.MiddleInitial = "L";
//			u1.LastName = "Baker";
//			u1.Password = "hello1";
//			u1.Birthday = new DateTime(1949, 11, 23);
//			u1.Street = "1245 Lake Anchorage Blvd.";
//			u1.City = "Austin";
//			u1.State = Models.State.TX;
//			u1.Zip = "78705";
//			//u1.SSN = "9075571146";
//			u1.Email = "cbaker@example.com";
//			u1.PopcornPoints = 110;
//			u1.Archived = false;
		
//			db.Users.AddOrUpdate(u => u.FirstName, u1);
//			db.SaveChanges();


//			User u2 = new User();
//			u2.UserType = Models.UserType.Customers;
//			u2.FirstName = "Wendy";
//			u2.MiddleInitial = "L";
//			u2.LastName = "Chang";
//			u2.Password = "texas1";
//			u2.Birthday = new DateTime(1997, 5, 16);
//			u2.Street = "202 Bellmont Hall";
//			u2.City = "Round Rock";
//			u2.State = Models.State.TX;
//			u2.Zip = "78681";
//			//u2.SSN = "9075943222";
//			u2.Email = "wchang@example.com";
//			u2.PopcornPoints = 0;
//			u2.Archived = false;

//			db.Users.AddOrUpdate(u => u.FirstName, u2);
//			db.SaveChanges();
//		}


//	}
//}