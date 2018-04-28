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

namespace AWO_Team14.Controllers
{
    public class TransactionsController : Controller
    {
        private AppDbContext db = new AppDbContext();

        public SelectList GetAllMovies()
        {
            List<Movie> allMovies = db.Movies.OrderBy(m => m.Title).ToList();

            SelectList selMovies = new SelectList(allMovies, "MovieID", "Title");

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
                if (s.Movie.MovieID == movieid && s.ShowDate >= Now)
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

        // GET: Transactions
        public ActionResult Index()
        {
            return View(db.Transactions.ToList());
        }

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
        public ActionResult SubmitTransaction([Bind(Include = "TransactionID,Payment,TransactionDate, UserTickets, User")] Transaction transaction)
        {
            Transaction t = db.Transactions.Find(transaction.TransactionID);

			if (Utilities.TransactionValidation.TicketValidation(t) == true) 
				if (ModelState.IsValid)
				{
					//TODO: put in popcorn validation - user only being able to use PP if they have enough for the whole tranaction
                    t.Payment = transaction.Payment;

                    if(transaction.Payment == Payment.PopcornPoints)
                    {
                        foreach(UserTicket ut in t.UserTickets)
                        {
                            UserTicket userTicket = db.UserTickets.Find(ut.UserTicketID);
                            userTicket.CurrentPrice = 0;
                            db.Entry(userTicket).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                    }

                    
                    db.Entry(t).State = EntityState.Modified;
                    db.SaveChanges();
					return RedirectToAction("ConfirmTransaction", new { id = t.TransactionID });

				}
            return View(transaction);
			
			
		}

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
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmTransaction([Bind(Include = "TransactionID,Payment,TransactionDate")] Transaction transaction)
        {
            Transaction t = db.Transactions.Find(transaction.TransactionID);

            if (Utilities.TransactionValidation.TicketValidation(t) == true)
                if (ModelState.IsValid)
                {
                    foreach (UserTicket ut in t.UserTickets)
                    {
                        ut.Status = Status.Active;
                    }

                    if (transaction.Payment == Payment.CreditCard)
                    {
                        Decimal decPopPoints = transaction.UserTickets.Sum(ut => ut.CurrentPrice);

                        Int32 intPopPoints = Convert.ToInt32(decPopPoints - (decPopPoints % 1));

                        Int32 CurPopPoints = transaction.User.PopcornPoints;

                        transaction.User.PopcornPoints = CurPopPoints + intPopPoints;

                    }

                    db.Entry(t).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Details", new { id = t.TransactionID });

                }
            return View(transaction);


        }


        // GET: Transactions/Details/5
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
        public ActionResult Create()
        {
            return View();
        }

        // POST: Transactions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TransactionID,Payment,TransactionDate")] Transaction transaction)
        {
            //TODO: Autoincrement transaction id
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

        //Get
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
        public ActionResult Edit([Bind(Include = "TransactionID,Payment,TransactionDate")] Transaction transaction)
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
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Transaction transaction = db.Transactions.Find(id);

            foreach (UserTicket t in transaction.UserTickets)
            {
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
