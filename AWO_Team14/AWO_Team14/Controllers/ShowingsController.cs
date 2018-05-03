using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AWO_Team14.DAL;
using AWO_Team14.Models;
using AWO_Team14.Utilities;

namespace AWO_Team14.Controllers
{
    public class ShowingsController : Controller
    {
        private AppDbContext db = new AppDbContext();

        // GET: Showings
        public ActionResult Index()
        {
            var query = from s in db.Showings
                        select s;
            query = query.Where(s => s.Schedule != null);
            query = query.Where(s => s.Schedule.Published == true);
            query = query.Where(s => s.ShowDate >= DateTime.Now);

            return View(query.OrderBy(s =>s.ShowDate).ToList());
        }

        public ActionResult DayShowings(String ShowDate, Theater Theater)
        {
            DateTime firstSunday = new DateTime(1753, 1, 7);

            List<Showing> Showings = new List<Showing>();
            

            if (ShowDate == "Friday")
            {
                var query = from s in db.Showings
                            where DbFunctions.DiffDays(firstSunday, s.ShowDate) % 7 == 5
                            select s;

                query = query.Where(s => s.Theater == Theater);
                query = query.Where(s => s.Schedule != null);
                query = query.Where(s => s.Schedule.Published == true);
                query = query.Where(s => s.ShowDate >= DateTime.Now);
                Showings = query.ToList();
            }

            if (ShowDate == "Thursday")
            {
                var query = from s in db.Showings
                            where DbFunctions.DiffDays(firstSunday, s.ShowDate) % 7 == 4
                            select s;

                query = query.Where(s => s.Theater == Theater);
                query = query.Where(s => s.Schedule != null);
                query = query.Where(s => s.Schedule.Published == true);
                query = query.Where(s => s.ShowDate >= DateTime.Now);
                Showings = query.ToList();
            }

            if (ShowDate == "Wednesday")
            {
                var query = from s in db.Showings
                            where DbFunctions.DiffDays(firstSunday, s.ShowDate) % 7 == 3
                            select s;

                query = query.Where(s => s.Theater == Theater);
                query = query.Where(s => s.Schedule != null);
                query = query.Where(s => s.Schedule.Published == true);
                query = query.Where(s => s.ShowDate >= DateTime.Now);
                Showings = query.ToList();
            }

            if (ShowDate == "Tuesday")
            {
                var query = from s in db.Showings
                            where DbFunctions.DiffDays(firstSunday, s.ShowDate) % 7 == 2
                            select s;

                query = query.Where(s => s.Theater == Theater);
                query = query.Where(s => s.Schedule != null);
                query = query.Where(s => s.Schedule.Published == true);
                query = query.Where(s => s.ShowDate >= DateTime.Now);
                Showings = query.ToList();
            }

            if (ShowDate == "Monday")
            {
                var query = from s in db.Showings
                            where DbFunctions.DiffDays(firstSunday, s.ShowDate) % 7 == 1
                            select s;

                query = query.Where(s => s.Theater == Theater);
                query = query.Where(s => s.Schedule != null);
                query = query.Where(s => s.Schedule.Published == true);
                query = query.Where(s => s.ShowDate >= DateTime.Now);
                Showings = query.ToList();
            }

            if (ShowDate == "Saturday")
            {
                var query = from s in db.Showings
                            where DbFunctions.DiffDays(firstSunday, s.ShowDate) % 7 == 6
                            select s;

                query = query.Where(s => s.Theater == Theater);
                query = query.Where(s => s.Schedule != null);
                query = query.Where(s => s.Schedule.Published == true);
                query = query.Where(s => s.ShowDate >= DateTime.Now);
                Showings = query.ToList();
            }

            if (ShowDate == "Sunday")
            {
                var query = from s in db.Showings
                            where DbFunctions.DiffDays(firstSunday, s.ShowDate) % 7 == 0
                            select s;

                query = query.Where(s => s.Theater == Theater);
                query = query.Where(s => s.Schedule != null);
                query = query.Where(s => s.Schedule.Published == true);
                query = query.Where(s => s.ShowDate >= DateTime.Now);
                Showings = query.ToList();
            }

            return View("Index", Showings.OrderBy(s => s.ShowDate.TimeOfDay));



        }

        public SelectList GetAllMovies()
        { 
            List<Movie> allMovies = db.Movies.OrderBy(m => m.Title).ToList();

            SelectList selMovies = new SelectList(allMovies, "MovieID", "Title");

            return selMovies;
        }

        public SelectList GetAllMovies(Showing showing)
        {
            List<Movie> allMovies = db.Movies.OrderBy(m => m.Title).ToList();

            List<Int32> SelectedMovies = new List<Int32>();

           SelectedMovies.Add(showing.Movie.MovieID);

            SelectList selMovies = new SelectList(allMovies, "MovieID", "Title", SelectedMovies);

            return selMovies; 
        }

        public Int32 CountAvailableSeats(int? showingid)
        {
            List<Seat> allSeats = Enum.GetValues(typeof(Seat)).Cast<Seat>().ToList();

            List<Seat> FilledSeats = new List<Seat>();

            Showing CurrentShowing = db.Showings.Find(showingid);

            foreach (UserTicket ut in CurrentShowing.UserTickets)
            {
                FilledSeats.Add(ut.SeatNumber);
            }

            List<Seat> Empty = allSeats.Except(FilledSeats).Union(FilledSeats.Except(allSeats)).ToList();

            if(Empty.Contains(Seat.Seat))
            {
                return Empty.Count() - 1;
            }
            return Empty.Count();

        }

        // GET: Showings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Showing showing = db.Showings.Find(id);
            if (showing == null)
            {
                return HttpNotFound();
            }
            ViewBag.EmptySeats = CountAvailableSeats(id);
     
            return View(showing);
        }

        // GET: Showings/Create
        [Authorize(Roles = "Manager")]
        public ActionResult Create(int ScheduleID)
        {
            Showing showing = new Showing();

            // find schedule object in db
            // Schedule schedule = new Schedule();

            Schedule schedule = db.Schedules.Find(ScheduleID);

            // attach showing to schedule
            showing.Schedule = schedule;

            ViewBag.AllMovies = GetAllMovies();
            return View(showing);
        }

        // POST: Showings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Manager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Bind(Include = "ShowingID,ShowDate,StartHour, StartMinute, Special,Theater")]
        public ActionResult Create(Showing showing, int SelectedMovie)
        {
            Movie m = db.Movies.Find(SelectedMovie);

            showing.ShowDate = showing.ShowDate.AddHours(showing.StartHour).AddMinutes(showing.StartMinute).AddSeconds(0);
            
            showing.Movie = m;

            showing.EndTime = showing.ShowDate.Add(m.Runtime);

            // find the schedule object associated with the showing's schedule's ScheduleID
            Schedule schedule = db.Schedules.Find(showing.Schedule.ScheduleID);

            // set the showing's schedule to the 
            // newly found schedule object 
            showing.Schedule = schedule;
            
            if (ScheduleValidation.ShowingInRange(showing) == false)
            {
                ViewBag.OutOfRange = "Show date is out of schedule date range";
            }


            String ValidationMessage = ScheduleValidation.ShowingValidation(showing);
            if (ValidationMessage == "ok" && ScheduleValidation.ShowingInRange(showing))
            {
                if (ModelState.IsValid)
                {
                    db.Showings.Add(showing);
                    db.SaveChanges();
                    // redirects to schedule's details page
                    return RedirectToAction("Details", "Schedules", new { id = schedule.ScheduleID });
                }
                
            }
            else
            {
                ViewBag.ErrorMessage = ValidationMessage;
            }

            ViewBag.AllMovies = GetAllMovies();
            return View(showing);
        }

        // GET: Showings/Reschedule/5
        [Authorize(Roles = "Manager")]
        public ActionResult Reschedule(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Showing showing = db.Showings.Find(id);
            if (showing == null)
            {
                return HttpNotFound();
            }

            return View(showing);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public ActionResult Reschedule([Bind(Include = "ShowingID,ShowDate,StartHour,StartMinute")] Showing showing)
        {
            Showing showingToChange = db.Showings.Include(x => x.Schedule)
                                             .FirstOrDefault(x => x.ShowingID == showing.ShowingID);

            DateTime oldShowDate = showingToChange.ShowDate;

            // take back to Schedules/Details if Schedule is unpublished
            if (showingToChange.Schedule.Published == false)
            {
                ViewBag.RescheduleError = "Schedule is unpublished. Click 'Edit' to change showings.";
                return RedirectToAction("Details", "Schedules", new { id = showingToChange.Schedule.ScheduleID });
            }

            if (ModelState.IsValid)
            {

                showingToChange.ShowDate = showing.ShowDate;
                showingToChange.ShowDate = showingToChange.ShowDate.AddHours(showing.StartHour).AddMinutes(showing.StartMinute).AddSeconds(0);
                showingToChange.StartHour = showing.StartHour;
                showingToChange.StartMinute = showing.StartMinute;
                showingToChange.EndTime = showingToChange.ShowDate.Add(showingToChange.Movie.Runtime);

                // check if showing is range of current schedule
                if (ScheduleValidation.ShowingInRange(showingToChange))
                {
                    String ValidationMessage = ScheduleValidation.ShowingValidation(showingToChange);
                    // checks is showing fits into current schedule
                    if (ValidationMessage == "ok")
                    {
                      
                        db.Entry(showingToChange).State = EntityState.Modified;
                        db.SaveChanges();
                        // email each user who bought a ticket that showing has been rescheduled
                        foreach (UserTicket t in showingToChange.UserTickets)
                        {
                            String Message = "Hello " + t.Transaction.User.FirstName + ",\n\n" + "The " + oldShowDate  + " " + showingToChange.Movie.Title 
                                + " showing has been rescheduled to " + showingToChange.ShowDate + ".\n\n" + "Love,\n" + "Dan";
                            Emailing.SendEmail(t.Transaction.User.Email, "Showing Rescheduled", Message);
                        }
                        
                        return RedirectToAction("Details", "Schedules", new { id = showingToChange.Schedule.ScheduleID });
                    }
                    else
                    {
                        ViewBag.ErrorMessage = ValidationMessage;
                    }
                }
                else
                {
                    ViewBag.OutOfRange = "Show date is out of schedule date range";
                }
            }

            return View(showing);
        }

        [Authorize(Roles = "Manager")]
        // GET: Showings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Showing showing = db.Showings.Find(id);
            if (showing == null)
            {
                return HttpNotFound();
            }

            ViewBag.AllMovies = GetAllMovies();
            return View(showing);
        }

       

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public ActionResult Edit([Bind(Include = "ShowingID,ShowDate,Special,StartHour,StartMinute,Theater")] Showing showing, int SelectedMovie, int StartHour, int StartMinute)
        {
            Showing showingToChange = db.Showings.Include(x => x.Schedule)
                                             .Include(x => x.Movie)
                                             .FirstOrDefault(x => x.ShowingID == showing.ShowingID);
            if (ModelState.IsValid)
            {
                //Find showing to change
                //Showing showingToChange = db.Showings.Find(showing.ShowingID);

                //Find schedule to reattach
                //Schedule schedule = db.Schedules.Find(showing.Schedule.ScheduleID);
                //showingToChange.Schedule = schedule;

                //Remove existing movie
                showingToChange.Movie = null;
                Movie movie = db.Movies.Find(SelectedMovie);
                showingToChange.Movie = movie;

                showingToChange.ShowDate = showing.ShowDate;
                showingToChange.StartHour = showing.StartHour;
                showingToChange.StartMinute = showing.StartMinute;
                
                showingToChange.ShowDate = showingToChange.ShowDate.AddHours(StartHour).AddMinutes(StartMinute).AddSeconds(0);
                //showingToChange.StartTime = showingToChange.ShowDate;
                showingToChange.EndTime = showingToChange.ShowDate.Add(movie.Runtime);
                showingToChange.Special = showing.Special;
                showingToChange.Theater = showing.Theater;

                String ValidationMessage = ScheduleValidation.ShowingValidation(showingToChange);
                if (ValidationMessage == "ok")
                {
                    db.Entry(showingToChange).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Details", "Schedules", new { id = showingToChange.Schedule.ScheduleID });
                }
                else
                {
                    ViewBag.ErrorMessage = ValidationMessage;
                }

                

            }
            ViewBag.AllMovies = GetAllMovies();
            return View(showing);
        }


        [Authorize(Roles = "Manager")]
        public ActionResult CheckDayShowings()
        {
            return View();

        }

        [Authorize(Roles = "Manager")]
        public ActionResult DisplayCheckDayShowings(DateTime ShowDate, Theater SelectedTheater)
        {
            AppDbContext db = new AppDbContext();

            String ValidationMessage = ScheduleValidation.DayShowingValidation(ShowDate, SelectedTheater);

            if (ValidationMessage == "ok")
            {
                ViewBag.ErrorMessage = "Your schedule is great!";
            }
            else
            {
                ViewBag.ErrorMessage = ValidationMessage;               
            }

            
            //display list of that day's showings
            var dayQuery = from s in db.Showings
                           select s;
            dayQuery = dayQuery.Where(s => s.Schedule != null);
            dayQuery = dayQuery.Where(s => s.ShowDate.Day == ShowDate.Day && s.Theater == SelectedTheater).OrderBy(s => s.ShowDate);
            List<Showing> dayShowings = dayQuery.ToList();

            return View(dayShowings);


        }


        // GET: Showings/Delete/5
        //[Authorize(Roles = "Manager")]
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Showing showing = db.Showings.Find(id);
        //    if (showing == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(showing);
        //}

        //// POST: Showings/Delete/5
        //[Authorize(Roles = "Manager")]
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Showing showing = db.Showings.Find(id);

        //    // saves showing's schedule to return to page 
        //    int ScheduleID = showing.Schedule.ScheduleID;

        //    // if schedule is published
        //    // showing is canceled not delete
        //    if (showing.Schedule.Published && DateTime.Now < showing.ShowDate )
        //    {
        //        // go to the cancelling a showing controller 
        //        showing.Schedule = null;
        //        db.SaveChanges();

        //        // returns and loops through each user that bought tickets to the showing
        //        foreach (UserTicket t in showing.UserTickets)
        //        {
        //            // change status of tickets for canceled showing
        //            t.Status = Status.Cancelled;
        //            t.SeatNumber = Seat.Seat;                   
        //            db.SaveChanges();

        //            // return popcorn points if showing is canceled
        //            if (t.Transaction.Payment == Payment.PopcornPoints)
        //            {
        //                t.Transaction.User.PopcornPoints += t.Transaction.PopcornPointsSpent;
        //                t.CurrentPrice = 0;
        //                db.SaveChanges();
        //            }
        //            else
        //            {
        //                // take back popcorn points received from credit card payment if showing is canceled
        //                t.Transaction.User.PopcornPoints -= Convert.ToInt32(t.CurrentPrice);
        //                t.CurrentPrice = 0;
        //                db.SaveChanges();
        //            }
        //            // emails the buyer that the showing has been canceled
        //            String Message = "Hello " + t.Transaction.User.FirstName + ",\n" + "The " + showing.ShowDate + showing.Movie.Title
        //                + "showing has been canceled.\n\n" + "Love,\n" + "Dan";
        //            Emailing.SendEmail(t.Transaction.User.Email, "Showing Canceled", Message);
        //        }
        //        // returns to Schedule's Detail page
        //        return RedirectToAction("Details", "Schedules", new { id = ScheduleID });
        //    }

        //    // schedule is unpublished
        //    if (showing.Schedule.Published == false)
        //    {
        //        // delete showing from table
        //        db.Showings.Remove(showing);
        //        db.SaveChanges();
        //        // returns to Schedule's Detail page
        //        return RedirectToAction("Details", "Schedules", new { id = ScheduleID });
        //    }

        //    return View(showing);
            
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
