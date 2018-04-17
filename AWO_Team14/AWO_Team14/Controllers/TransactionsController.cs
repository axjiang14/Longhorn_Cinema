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

        // GET: Transactions
        public ActionResult Index()
        {
            return View(db.Transactions.ToList());
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
                return RedirectToAction("Index");
            }

            return View(transaction);
        }

        //public ActionResult ChooseMovie(int id)
        //{
        //    //New instance of user ticket
        //    UserTicket ut = new UserTicket();

        //    //Finds transaction for user ticket
        //    Transaction t = db.Transactions.Find(id);

        //    //Sets user ticket's transaction to the transaction
        //    ut.Transaction = t;

        //    ViewBag.AllShowings = GetAllMovies();

        //    return View(ut);
        //}

        

        public ActionResult AddToTransaction(int id)
        {
            //New instance of user ticket
            UserTicket ut = new UserTicket();

            //Finds transaction for user ticket
            Transaction t = db.Transactions.Find(id);

            //Sets user ticket's transaction to the transaction
            ut.Transaction = t;

            ViewBag.AllShowings = GetAllShowings();
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

            //TODO: Change price
            ut.CurrentPrice = 12;

            ut.Current = true;

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
