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
    public class UserTicketsController : Controller
    {
        private AppDbContext db = new AppDbContext();

        // GET: UserTickets
        public ActionResult Index()
        {
            return View(db.UserTickets.ToList());
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
        public ActionResult Create([Bind(Include = "UserTicketID,CurrentPrice,Current")] UserTicket userTicket)
        {
            if (ModelState.IsValid)
            {
                db.UserTickets.Add(userTicket);
                db.SaveChanges();
                return RedirectToAction("Index");
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
        public ActionResult Edit([Bind(Include = "UserTicketID,CurrentPrice,Current")] UserTicket userTicket)
        {
            UserTicket ut= db.UserTickets.Include(UT => UT.Transaction)
                                            .Include(UT => UT.Showing)
                                            .FirstOrDefault(x => x.UserTicketID == userTicket.UserTicketID);

            if (ModelState.IsValid)
            {
                db.Entry(userTicket).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "Transactions", new { id = ut.Transaction.TransactionID });
            }

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
            Transaction t = userTicket.Transaction;
            userTicket.Current = false;
            db.SaveChanges();
            return RedirectToAction("Details", "Transactions", new { id = t.TransactionID });
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
