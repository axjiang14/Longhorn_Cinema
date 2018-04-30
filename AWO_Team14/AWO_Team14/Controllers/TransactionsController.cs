using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AWO_Team14.DAL;
using AWO_Team14.Models;
using System.Diagnostics;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using AWO_Team14.Utilities;
using System.Web.Security;

namespace AWO_Team14.Controllers
{
    public class TransactionsController : Controller
    {
        private AppDbContext db = new AppDbContext();

        public SelectList GetAllMovies()
        {
            List<Movie> allMovies = db.Movies.OrderBy(m => m.Title).ToList();

            List<Movie> relMovies = new List<Movie>();
            
            foreach(Movie m in allMovies)
            {
                if(m.Showings.Count() != 0)
                {
                    relMovies.Add(m);
                }
            }

            SelectList selMovies = new SelectList(relMovies, "MovieID", "Title");

            return selMovies;
        }

        public SelectList GetRelShowings(int movieid)
        {
            //Gets showings for movie
            List<Showing> Showings1 = new List<Showing>();
            List<Showing> Showings2 = new List<Showing>();

            var query = from s in db.Showings
                        select s;

            //query = query.Where(s => s.ShowDate > DateTime.Now);

            Showings1 = query.ToList(); //List of showings from query

            DateTime Now = DateTime.Now;

            foreach (Showing s in Showings1)
            {
                if (s.Movie.MovieID == movieid && s.ShowDate >= Now && s.Schedule.Published == true)
                {
                    Showings2.Add(s);
                }
            }

            SelectList ChosenShowings = new SelectList(Showings2.OrderBy(s => s.ShowingID), "ShowingID", "ShowDate");

            return ChosenShowings;
        }

        public SelectList GetAllShowings()
        {
            List<Showing> Showings = db.Showings.ToList();

            SelectList AllShowings = new SelectList(Showings.OrderBy(s => s.ShowingID), "ShowingID", "ShowDate");

            return AllShowings;

        }

        public SelectList GetAllCustomers()
        {

            var roles = db.Roles.Where(r => r.Name == "Customer");
            if (roles.Any())
            {
                var roleId = roles.First().Id;
                var dbCustomers = from user in db.Users
                                  where user.Roles.Any(r => r.RoleId == roleId)
                                  select user;
                List<AppUser> Customers = dbCustomers.ToList();

                SelectList CustomersList = new SelectList(Customers.OrderBy(u => u.Id), "Id", "UserName");

                return CustomersList;

            }

            return null;
        }

        // GET: Transactions
        [Authorize]
        public ActionResult Index()
        {
            var query = from t in db.Transactions
                        select t;

            if (User.IsInRole("Customer"))
            {
                string userid = User.Identity.GetUserId();
                query = query.Where(t => t.User.Id == userid);
            }

            List<Transaction> transactions = query.ToList();

            return View(transactions);

            return View(db.Transactions.ToList());
        }

        [Authorize(Roles = "Customer")]
        public ActionResult ReceivedGifts()
        {
            var query = from t in db.Transactions
                        select t;

            if (User.IsInRole("Customer"))
            {
                string userid = User.Identity.GetUserId();
                query = query.Where(t => t.Giftee.Id == userid);
            }

            List<Transaction> transactions = query.ToList();

            return View(transactions);
        }

        [Authorize]
        public ActionResult PendingDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        [Authorize]
        public ActionResult SubmitTransaction(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult SubmitTransaction([Bind(Include = "TransactionID,Payment,TransactionDate, UserTickets, User, PopcornPointsSpent")] Transaction transaction, string SearchGiftee)
        {
            Transaction t = db.Transactions.Find(transaction.TransactionID);

            Debug.WriteLine(Utilities.TransactionValidation.TicketValidation(t));
            if (Utilities.TransactionValidation.TicketValidation(t) == true)
            {
                if (ModelState.IsValid)
                {
					if (SearchGiftee != null && SearchGiftee != "")
					{
						var Giftee = db.Users.Where(u => u.Email == SearchGiftee);
						AppUser RGiftee = Giftee.FirstOrDefault();

						if (RGiftee == null)
						{
							ViewBag.ErrorMessage = "The user can't be found";
							return View(t);
						}


						//TODO: Check that the giftee is a customer
						//if (! RGiftee.IsUserInRole("Customer"))
						//{
						//	ViewBag.ErrorMessage = "The user isn't a customer";
						//	return View(t);
						//}

							foreach (UserTicket item in t.UserTickets)
						{
							if (item.Showing.Movie.MPAA_Rating == MPAA.NC17 || item.Showing.Movie.MPAA_Rating == MPAA.R)
							{
								if (Utilities.TransactionValidation.AgeCalc(RGiftee.Birthday) < 18)
								{
									ViewBag.ErrorMessage = "You can't gift a NC-17 or R rated movie to a minor";
									return View(t);
								}
							}
						}
						t.Giftee = RGiftee;

					}

                    //TODO: put in popcorn validation - user only being able to use PP if they have enough for the whole tranaction
                    t.Payment = transaction.Payment;
					if (transaction.Payment == Payment.PopcornPoints)
					{
						if (Utilities.TransactionValidation.PPCalc(t) == false)
						{
							ViewBag.ErrorMessage = "You don't have enough Popcorn Points to purchase these tickets";
							return View(t);
						}

						else
						{
							Int32 CurPopPoints = t.User.PopcornPoints;
							Int32 intTickets = t.UserTickets.Count();
							Int32 PPTickets = intTickets * 100;
							t.User.PopcornPoints = CurPopPoints - PPTickets;
                            t.PopcornPointsSpent = PPTickets;

						
								foreach (UserTicket ut in t.UserTickets)
								{
									UserTicket userTicket = db.UserTickets.Find(ut.UserTicketID);
									userTicket.CurrentPrice = 0;
									db.Entry(userTicket).State = EntityState.Modified;
									db.SaveChanges();
								}
						}
					}


                    db.Entry(t).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("ConfirmTransaction", new { id = t.TransactionID });

                }
            }
            return View(t);
			
			
		}

        [Authorize]
        public ActionResult ConfirmTransaction(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmTransaction([Bind(Include = "TransactionID,Payment,TransactionDate")] Transaction transaction)
        {
            Transaction t = db.Transactions.Find(transaction.TransactionID);

            if (Utilities.TransactionValidation.TicketValidation(t) == true)
            {

                if (ModelState.IsValid)
                {
                    // get prices for each ticket
                    
                    // add popcorn points from transaction
                    if (t.Payment == Payment.CreditCard)
                    {
                        foreach (UserTicket ut in t.UserTickets)
                        {
                            int ticketCount = 1;
                            ut.Status = Status.Active;
                            ut.CurrentPrice = DiscountPrice.GetTicketPrice(ut, ticketCount);
                            ticketCount += 1;
                        }

                        Decimal decPopPoints = t.UserTickets.Sum(ut => ut.CurrentPrice);

                        Int32 intPopPoints = Convert.ToInt32(decPopPoints - (decPopPoints % 1));

                        Int32 CurPopPoints = t.User.PopcornPoints;

                        t.User.PopcornPoints = CurPopPoints + intPopPoints;

                        

                    }
                    // save changes to database
                    db.Entry(t).State = EntityState.Modified;
                    db.SaveChanges();

                    String Message = "Hello " + t.User.FirstName + ",\n\n" + "Your order number" + t.TransactionNumber + "has been placed.\n\n" + "Love,\n" + "Dan";
                    Emailing.SendEmail(t.User.Email, "Order Placed", Message);
                    return RedirectToAction("Details", new { id = t.TransactionID });

                }
            }

            return View(transaction);

        }


        // GET: Transactions/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        // GET: Transactions/Create
        [Authorize(Roles = "Customer")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Transactions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [Authorize(Roles = "Customer")]
        public ActionResult Create([Bind(Include = "TransactionID,Payment,TransactionDate, TransactionNumber")] Transaction transaction)
        {

			//TODONE: Autoincrement transaction id
			transaction.TransactionNumber = Utilities.GenerateTransactionNumber.GetNextTransactionNum();

			transaction.TransactionDate = DateTime.Now;
            transaction.Payment = Payment.CreditCard;
            transaction.User = db.Users.Find(User.Identity.GetUserId());

            if (ModelState.IsValid)
            {
                db.Transactions.Add(transaction);
                db.SaveChanges();
                return RedirectToAction("ChooseMovie",  new { id = transaction.TransactionID });
            }

            return View(transaction);
        }

        // GET: Transactions/Create
        [Authorize(Roles = "Employee, Manager")]
        public ActionResult EmployeeCreate()
        {
            ViewBag.AllCustomers = GetAllCustomers();
            return View();
        }

        // POST: Transactions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Employee, Manager")]
        public ActionResult EmployeeCreate([Bind(Include = "TransactionID,Payment,TransactionDate, User, TransactionNumber")] Transaction transaction, string Customer)
        {
            //TODO: Autoincrement transaction id
            transaction.TransactionDate = DateTime.Now;
            transaction.Payment = Payment.CreditCard;
            transaction.User = db.Users.Find(Customer);

            if (ModelState.IsValid)
            {
                db.Transactions.Add(transaction);
                db.SaveChanges();
                return RedirectToAction("ChooseMovie", new { id = transaction.TransactionID });
            }

            ViewBag.AllCustomers = GetAllCustomers();
            return View(transaction);
        }

        //Get
        [Authorize]
        public ActionResult ChooseMovie(int id)
        {
            //New instance of user ticket
            UserTicket ut = new UserTicket();

            //Finds transaction for user ticket
            Transaction t = db.Transactions.Find(id);

            //Sets user ticket's transaction to the transaction
            ut.Transaction = t;

            ViewBag.AllMovies = GetAllMovies();

            return View(ut);
        }

        [HttpPost]
        [Authorize]
        public ActionResult ChooseMovie(UserTicket ut, int SelectedMovie)
        {
            //Finds movie
            Movie movie = db.Movies.Find(SelectedMovie);

            //Sets ticket's showing
            ut.MovieID = movie.MovieID;

            //Finds transaction associated w/ ticket
            Transaction transaction = db.Transactions.Find(ut.Transaction.TransactionID);

            //Sets user ticket's transaction
            ut.Transaction = transaction;

			if (movie.MPAA_Rating == MPAA.NC17 || movie.MPAA_Rating == MPAA.R)
			{
				if (Utilities.TransactionValidation.AgeCalc(transaction.User.Birthday) < 18)
				{
					return View(ut);
				}
			}

			//TODO: Change price
			ut.CurrentPrice = 12;

            ut.Status = Status.Pending;

            ut.SeatNumber = Seat.Seat;

            //Sets user ticket's showing
            ut.Showing = null;

				if (ModelState.IsValid)
				{
					db.UserTickets.Add(ut);
					db.SaveChanges();
					//return RedirectToAction("Details", "Transactions", new { id = transaction.TransactionID });
					//return RedirectToAction("Edit", "UserTickets", new { id = ut.UserTicketID });
					return RedirectToAction("AddToTransaction", new { ticketid = ut.UserTicketID, transid = ut.Transaction.TransactionID });
				}

            ViewBag.AllMovies = GetAllMovies();
            return View(ut);
        }

        [Authorize]
        public ActionResult AddToTransaction(int ticketid, int transid)
        {
            //Find user ticket
            UserTicket ut = db.UserTickets.Find(ticketid);

            ut.Transaction = db.Transactions.Find(transid);

            //Finds movie for user ticket
            Movie m = db.Movies.Find(ut.MovieID);

            ViewBag.AllShowings = GetRelShowings(m.MovieID);

            return View(ut);
        }

        //Post AddToOrder
        [HttpPost]
        [Authorize]
        public ActionResult AddToTransaction([Bind(Include = "UserTicketID, CurrentPrice, SeatNumber, Status, MovieID, Showing, Transaction")]
        UserTicket ut, int SelectedShowing)
        {
            //Finds showing
            Showing showing = db.Showings.Find(SelectedShowing);
            Debug.WriteLine(showing.ShowingID);

            //Sets ticket's showing
            UserTicket userticket = db.UserTickets.Find(ut.UserTicketID);
            userticket.Showing = showing;

            Transaction t = db.Transactions.Find(ut.Transaction.TransactionID);

            userticket.Transaction = t;

            //Finds transaction associated w/ ticket
            //Transaction transaction = db.Transactions.Find(ut.Transaction.TransactionID);

            //Sets user ticket's transaction
           // ut.Transaction = transaction;

            //Sets MovieID
            //ut.MovieID = showing.Movie.MovieID;

            //TODO: Change price
            //ut.CurrentPrice = 12;

            //ut.Status = Status.Pending;

            //TODO: Change seat number
            //ut.SeatNumber = Seat.Seat;


            if (ModelState.IsValid)
            {
                db.Entry(userticket).State = EntityState.Modified;
                db.SaveChanges();
                //return RedirectToAction("Details", "Transactions", new { id = transaction.TransactionID });
                return RedirectToAction("Edit", "UserTickets", new { id = ut.UserTicketID });
            }

            ViewBag.AllShowings = GetAllShowings();
            return View(ut);
        }

        [Authorize]
        public ActionResult RemoveFromTransaction(int id)
        {
            Transaction t = db.Transactions.Find(id);

            if (t == null)//transaction is not found
            {
                return RedirectToAction("Details", new { id = id });
            }

            if (t.UserTickets.Count == 0) // there are no tickets
            {
                return RedirectToAction("Details", new { id = id });
            }

            //pass the list of order details to the view
            return View(t.UserTickets);
        }

        // GET: Transactions/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        // POST: Transactions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "TransactionID,Payment,TransactionDate, TransactionNumber")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                db.Entry(transaction).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(transaction);
        }

        // GET: Transactions/Delete/5

        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Transaction transaction = db.Transactions.Find(id);

			if (transaction.Payment == Payment.PopcornPoints)
			{
				Int32 CurPopPoints = transaction.User.PopcornPoints;
				Int32 intTickets = transaction.UserTickets.Count();
				Int32 PPTickets = intTickets * 100;
                transaction.PopcornPointsSpent = 0;
				transaction.User.PopcornPoints = CurPopPoints + PPTickets;
                db.SaveChanges();
			}

            if (transaction.Payment == Payment.CreditCard)
			{
				Decimal decPopPoints = transaction.UserTickets.Sum(ut => ut.CurrentPrice);

				Int32 intPopPoints = Convert.ToInt32(decPopPoints - (decPopPoints % 1));

				//Int32 CurPopPoints = transaction .User.PopcornPoints; 

				transaction.User.PopcornPoints -= intPopPoints;
                db.SaveChanges();

                //TODO: DAN - email customers that used credit card that their $ has been refunded
                String Message = "Hello " + transaction.User.FirstName + ",\n" + "The transaction number" + transaction.TransactionNumber + "has been cancelled.\n\n" + "Love,\n" + "Dan";
                Emailing.SendEmail(transaction.User.Email, "Transaction Cancelled", Message);

            }

			foreach (UserTicket t in transaction.UserTickets)
            {
                t.CurrentPrice = 0;
                t.Status = Status.Returned;
                t.SeatNumber = Seat.Seat;
			}
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        
    }
}
