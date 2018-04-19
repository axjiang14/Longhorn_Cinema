using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AWO_Team14.DAL;
using AWO_Team14.Models;

namespace AWO_Team14.Controllers
{
    public class UserTicketsController : Controller
    {
        private AppDbContext db = new AppDbContext();

        // GET: UserTickets
        public ActionResult Index()
        {
            return View(db.UserTickets.ToList());
        }



        public SelectList GetEmptySeats(int showingid)
        {
            List<Seat> allSeats = Enum.GetValues(typeof(Seat)).Cast<Seat>().ToList();

            List<Seat> FilledSeats = new List<Seat>();

            Showing CurrentShowing = db.Showings.Find(showingid);

            foreach (UserTicket ut in CurrentShowing.UserTickets)
            {
                FilledSeats.Add(ut.SeatNumber);
            }

            List<Seat> Empty = allSeats.Except(FilledSeats).Union(FilledSeats.Except(allSeats)).ToList();

            SelectList EmptySeats = new SelectList(Empty);

            return EmptySeats;
        }

        // GET: UserTickets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserTicket userTicket = db.UserTickets.Find(id);
            if (userTicket == null)
            {
                return HttpNotFound();
            }
            return View(userTicket);
        }

        // GET: UserTickets/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserTickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserTicketID,CurrentPrice,Status")] UserTicket userTicket)
        {
            if (ModelState.IsValid)
            {
                userTicket.Status = Status.Pending;
                db.UserTickets.Add(userTicket);
                db.SaveChanges();
                return RedirectToAction("Details", new { id = userTicket.UserTicketID });
            }

            return View(userTicket);
        }

        // GET: UserTickets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserTicket userTicket = db.UserTickets.Find(id);
            int showingid = userTicket.Showing.ShowingID;
            ViewBag.EmptySeats = GetEmptySeats(showingid);
            if (userTicket == null)
            {
                return HttpNotFound();
            }
            return View(userTicket);
        }

        // POST: UserTickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserTicketID,CurrentPrice,Status")] UserTicket userTicket, Seat SelectedSeat)
        {
            UserTicket ut= db.UserTickets.Include(UT => UT.Transaction)
                                            .Include(UT => UT.Showing)
                                            .FirstOrDefault(x => x.UserTicketID == userTicket.UserTicketID);

            if (ModelState.IsValid)
            {
                ut.SeatNumber = SelectedSeat;
                db.Entry(ut).State = EntityState.Modified;
                db.SaveChanges();

                if (ut.Status == Status.Pending)
                {
                    return RedirectToAction("PendingDetails", "Transactions", new { id = ut.Transaction.TransactionID });
                }
                if (ut.Status == Status.Active)
                {
                    return RedirectToAction("Details", "Transactions", new { id = ut.Transaction.TransactionID });
                }
            }

            int showingid = userTicket.Showing.ShowingID;
            ViewBag.EmptySeats = GetEmptySeats(showingid);

            userTicket.Transaction = ut.Transaction;

            return View(userTicket);
        }

        // GET: UserTickets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserTicket userTicket = db.UserTickets.Find(id);
            if (userTicket == null)
            {
                return HttpNotFound();
            }
            return View(userTicket);
        }

        // POST: UserTickets/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        //TODO: Removing tickets
        public ActionResult DeleteConfirmed(int id)
        { 

            
            UserTicket userTicket = db.UserTickets.Find(id);
            Debug.WriteLine(userTicket);

            if (userTicket.Status == Status.Pending)
            {

                Transaction t = userTicket.Transaction;
                t.UserTickets.Remove(userTicket);
                db.UserTickets.Remove(userTicket);
                db.SaveChanges();
                return RedirectToAction("Details", "Transactions", new { id = t.TransactionID });

            }
            else
            {
                Transaction t = userTicket.Transaction;
                userTicket.Status = Status.Returned;
                db.SaveChanges();
                return RedirectToAction("Details", "Transactions", new { id = t.TransactionID });
            }
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
