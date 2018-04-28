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
    public class SchedulesController : Controller
    {
        private AppDbContext db = new AppDbContext();

        // GET: Schedules
        public ActionResult Index()
        {
            return View(db.Schedules.ToList());
        }

        // Get: Schedules/Copy/5
        public ActionResult Copy (int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Schedule schedule = db.Schedules.Find(id);
            if (schedule == null)
            {
                return HttpNotFound();
            }
            return View(schedule);
        }

        //Post: Schedules/Copy/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Copy ([Bind(Include = "ScheduleID")] Schedule schedule, 
            DateTime datPickDate, Theater theaterPick, DateTime datCopyToDate, Theater theaterCopy)
        {
            // find object in database
            Schedule s = db.Schedules.Find(schedule.ScheduleID);

            var query = from sh in db.Showings select sh;

            // find showings in the correct schedule
            query = query.Where(c => c.Schedule.ScheduleID == schedule.ScheduleID);
            // find date to copy showing from
            query = query.Where(c => c.ShowDate == datPickDate);
            // find theater to copy from
            query = query.Where(c => c.Theater == theaterPick);

            List<Showing> SelectedShowings = query.ToList();

            if (ModelState.IsValid)
            {
                foreach (Showing showing in SelectedShowings)
                {
                    Showing copyShowing = new Showing();
                    copyShowing.ShowDate = datCopyToDate;
                    copyShowing.Theater = theaterCopy;
                    copyShowing.Schedule = s;

                    copyShowing.Movie = showing.Movie;
                    copyShowing.StartHour = showing.StartHour;
                    copyShowing.StartMinute = showing.StartMinute;
                    copyShowing.EndTime = showing.EndTime;
                    copyShowing.Special = showing.Special;

                    db.Showings.Add(copyShowing);
                    db.SaveChanges();
                }

                return RedirectToAction("Details", "Schedules", new { id = schedule.ScheduleID });
            }

            return View(schedule);

        }

        // GET: Schedules/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Schedule schedule = db.Schedules.Find(id);
            if (schedule == null)
            {
                return HttpNotFound();
            }
            return View(schedule);
        }

        // GET: Schedules/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Schedules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ScheduleID,Published,StartDate")] Schedule schedule)
        {
            schedule.StartDate = DateTime.Today;
            schedule.EndDate = DateTime.Today.AddDays(6);

            if (ModelState.IsValid)
            {
                db.Schedules.Add(schedule);
                db.SaveChanges();
                // pass ScheduleID to attach a schedule when create showings
                return RedirectToAction("Create", "Showings", new { ScheduleID = schedule.ScheduleID });
            }

            return View(schedule);
        }

        // GET: Schedules/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Schedule schedule = db.Schedules.Find(id);
            if (schedule == null)
            {
                return HttpNotFound();
            }
            return View(schedule);
        }

        // POST: Schedules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ScheduleID,Published")] Schedule schedule)
        {
            // TODO: add schedule final validations
            // ex. minimize gaps and last showing endtime
            Schedule s = db.Schedules.Find(schedule.ScheduleID);
            s.Published = schedule.Published;

            if (ModelState.IsValid)
            {
                db.Entry(s).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(schedule);
        }

        // GET: Schedules/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Schedule schedule = db.Schedules.Find(id);
            if (schedule == null)
            {
                return HttpNotFound();
            }
            return View(schedule);
        }

        // POST: Schedules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Schedule schedule = db.Schedules.Find(id);
            db.Schedules.Remove(schedule);
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
