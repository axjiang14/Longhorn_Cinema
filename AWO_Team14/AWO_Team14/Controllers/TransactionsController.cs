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

            Showings1 = query.ToList(); //List of showings from query

            foreach (Showing s in Showings1)
            {
                if (s.Movie.MovieID == movieid)
                {
                    Showings2.Add(s);
                    break;
                }
            }

            SelectList ChosenShowings = new SelectList(Showings2.OrderBy(s => s.ShowingID), "ShowingID", "ShowDate");

            return ChosenShowings;
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
        public ActionResult SubmitTransaction([Bind(Include = "TransactionID,Payment,TransactionDate")] Transaction transaction)
        {
            Transaction t = db.Transactions.Find(transaction.TransactionID);

			if (Utilities.TransactionValidation.TicketValidation(transaction) == true) 
				if (ModelState.IsValid)
				{
					foreach (UserTicket ut in t.UserTickets)
					{
						ut.Status = Status.Active;
					}

					db.Entry(t).State = EntityState.Modified;
					db.SaveChanges();
					return RedirectToAction("Details", new { id = t.TransactionID });

				}
            return View(transaction);
			
			
		}

        public SelectList GetAllShowings(int MovieID)
        {
            var query = from s in db.Showings
                        select s;
            query = query.Where(s => s.Movie.MovieID == MovieID);

            List<Showing> Showings = query.ToList();

            SelectList AllShowings = new SelectList(Showings.OrderBy(s => s.ShowingID), "ShowingID", String.Format("ShowDate" + "Movie"));

            return AllShowings;
   
        }

        public SelectList GetAllShowings()
        {
            List<Showing> Showings = db.Showings.ToList();

            SelectList AllShowings = new SelectList(Showings.OrderBy(s => s.ShowingID), "ShowingID", "ShowDate");

            return AllShowings;

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
                return RedirectToAction("AddToTransaction", new { ticketid = ut.UserTicketID });
            }

            ViewBag.AllMovies = GetAllMovies();
            return View(ut);
        }



        public ActionResult AddToTransaction(int ticketid)
        {
            //New instance of user ticket
            UserTicket ut = db.UserTickets.Find(ticketid);

            //Finds movie for user ticket
            Movie m = db.Movies.Find(ut.MovieID);


            ViewBag.AllShowings = GetRelShowings(m.MovieID);

            return View(ut);
        }

        //Post AddToOrder
        [HttpPost]
        public ActionResult AddToTransaction(UserTicket ut, int SelectedShowing)
        {
            //Finds showing
            Showing showing = db.Showings.Find(SelectedShowing);

            //Sets ticket's showing
            ut.Showing = showing;

            //Finds transaction associated w/ ticket
            Transaction transaction = db.Transactions.Find(ut.Transaction.TransactionID);

            //Sets user ticket's transaction
            ut.Transaction = transaction;

            //Sets MovieID
            ut.MovieID = showing.Movie.MovieID;

            //TODO: Change price
            ut.CurrentPrice = 12;

            ut.Status = Status.Pending;

            //TODO: Change seat number
            ut.SeatNumber = Seat.Seat;


            if (ModelState.IsValid)
            {
                db.UserTickets.Add(ut);
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

        //// POST: Transactions/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Transaction transaction = db.Transactions.Find(id);
        //    db.Transactions.Remove(transaction);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

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
