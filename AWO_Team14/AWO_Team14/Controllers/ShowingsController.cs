using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
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
			return View(db.Showings.OrderBy(s =>s.ShowDate).ToList());
        }

        //public ActionResult DayShowings(String ShowDate)
        //{
        //    DateTime firstSunday = new DateTime(1753, 1, 7);

        //    List<Showing> Showings = new List<Showing>();
        //    //var query = from s in db.Showings
        //    //            select s;
        //    //query = query.Where(s => s.ShowDate.DayOfWeek == ShowDate);

        //    if (ShowDate == "Friday")
        //    {
        //        var query = from s in db.Showings
        //                    where DbFunctions.DiffDays(firstSunday, s.ShowDate) % 7 == 5
        //                    select s;
        //        Showings = query.ToList();
        //    }

        //    if (ShowDate == "Thursday")
        //    {
        //        var query = from s in db.Showings
        //                    where DbFunctions.DiffDays(firstSunday, s.ShowDate) % 7 == 4
        //                    select s;
        //        Showings = query.ToList();
        //    }

        //    if (ShowDate == "Wednesday")
        //    {
        //        var query = from s in db.Showings
        //                    where DbFunctions.DiffDays(firstSunday, s.ShowDate) % 7 == 3
        //                    select s;
        //        Showings = query.ToList();
        //    }

        //    if (ShowDate == "Tuesday")
        //    {
        //        var query = from s in db.Showings
        //                    where DbFunctions.DiffDays(firstSunday, s.ShowDate) % 7 == 2
        //                    select s;
        //        Showings = query.ToList();
        //    }

        //    if (ShowDate == "Monday")
        //    {
        //        var query = from s in db.Showings
        //                    where DbFunctions.DiffDays(firstSunday, s.ShowDate) % 7 == 1
        //                    select s;
        //        Showings = query.ToList();
        //    }

        //    if (ShowDate == "Saturday")
        //    {
        //        var query = from s in db.Showings
        //                    where DbFunctions.DiffDays(firstSunday, s.ShowDate) % 7 == 6
        //                    select s;
        //        Showings = query.ToList();
        //    }

        //    if (ShowDate == "Sunday")
        //    {
        //        var query = from s in db.Showings
        //                    where DbFunctions.DiffDays(firstSunday, s.ShowDate) % 7 == 0
        //                    select s;
        //        Showings = query.ToList();
        //    }

        //    return View("Index", Showings.OrderBy(s => s.ShowTime));



        //}

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
                Showings = query.ToList();
            }

            if (ShowDate == "Thursday")
            {
                var query = from s in db.Showings
                            where DbFunctions.DiffDays(firstSunday, s.ShowDate) % 7 == 4
                            select s;

                query = query.Where(s => s.Theater == Theater);
                Showings = query.ToList();
            }

            if (ShowDate == "Wednesday")
            {
                var query = from s in db.Showings
                            where DbFunctions.DiffDays(firstSunday, s.ShowDate) % 7 == 3
                            select s;

                query = query.Where(s => s.Theater == Theater);
                Showings = query.ToList();
            }

            if (ShowDate == "Tuesday")
            {
                var query = from s in db.Showings
                            where DbFunctions.DiffDays(firstSunday, s.ShowDate) % 7 == 2
                            select s;

                query = query.Where(s => s.Theater == Theater);
                Showings = query.ToList();
            }

            if (ShowDate == "Monday")
            {
                var query = from s in db.Showings
                            where DbFunctions.DiffDays(firstSunday, s.ShowDate) % 7 == 1
                            select s;

                query = query.Where(s => s.Theater == Theater);
                Showings = query.ToList();
            }

            if (ShowDate == "Saturday")
            {
                var query = from s in db.Showings
                            where DbFunctions.DiffDays(firstSunday, s.ShowDate) % 7 == 6
                            select s;

                query = query.Where(s => s.Theater == Theater);
                Showings = query.ToList();
            }

            if (ShowDate == "Sunday")
            {
                var query = from s in db.Showings
                            where DbFunctions.DiffDays(firstSunday, s.ShowDate) % 7 == 0
                            select s;

                query = query.Where(s => s.Theater == Theater);
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
            return View(showing);
        }

        // GET: Showings/Create
        public ActionResult Create()
        {
            ViewBag.AllMovies = GetAllMovies();
            return View();
        }

        // POST: Showings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ShowingID,ShowDate,StartHour, StartMinute, Special,Theater")] Showing showing, int SelectedMovie)
        {
            Movie m = db.Movies.Find(SelectedMovie);

            showing.ShowDate = showing.ShowDate.AddHours(showing.StartHour).AddMinutes(showing.StartMinute).AddSeconds(0);
            

            showing.Movie = m;

            showing.EndTime = showing.ShowDate.Add(m.Runtime);
            if (ScheduleValidation.ShowingValidation(showing) == true)
            {
                if (ModelState.IsValid)
                {
                    db.Showings.Add(showing);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            

            ViewBag.AllMovies = GetAllMovies();
            return View(showing);
        }

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

        // POST: Showings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "ShowingID,ShowDate,StartTime,Special,Theater")] Showing showing, int SelectedMovie)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        //Find showing to change
        //        Showing showingToChange = db.Showings.Find(showing.ShowingID);

        //        //Remove existing movie
        //        showingToChange.Movie = null;

        //        Movie movie = db.Movies.Find(SelectedMovie);

        //        showingToChange.Movie = movie;



        //        //showingToChange.Movie = movie;
        //        showingToChange.ShowDate = showing.ShowDate;
        //        showingToChange.StartTime = showing.StartTime;
        //        showing.EndTime = showing.StartTime.Add(movie.Runtime);
        //        showingToChange.Special = showing.Special;
        //        showingToChange.Theater = showing.Theater;

        //        if (Utilities.ScheduleValidation.ShowingValidation(showing) == true)
        //        {
        //            db.Entry(showingToChange).State = EntityState.Modified;
        //            db.SaveChanges();
        //            return RedirectToAction("Index");
        //        }

        //    }
        //    ViewBag.AllMovies = GetAllMovies();
        //    return View(showing);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ShowingID,ShowDate,Special,Theater")] Showing showing, int SelectedMovie, int StartHour, int StartMinute)
        {
            if (ModelState.IsValid)
            {
                //Find showing to change
                Showing showingToChange = db.Showings.Find(showing.ShowingID);
                //Remove existing movie
                showingToChange.Movie = null;
                Movie movie = db.Movies.Find(SelectedMovie);
                showingToChange.Movie = movie;

                showingToChange.ShowDate = showing.ShowDate;

                showingToChange.ShowDate = showingToChange.ShowDate.AddHours(StartHour).AddMinutes(StartMinute).AddSeconds(0);
                //showingToChange.StartTime = showingToChange.ShowDate;
                showing.EndTime = showing.ShowDate.Add(movie.Runtime);
                showingToChange.Special = showing.Special;
                showingToChange.Theater = showing.Theater;

                if (ScheduleValidation.ShowingValidation(showing) == true)
                {
                    db.Entry(showingToChange).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

            }
            ViewBag.AllMovies = GetAllMovies();
            return View(showing);
        }


        public ActionResult CheckDayShowings()
        {
            return View();

        }

        [HttpPost]
        public ActionResult CheckDayShowings(DateTime date)
        {
            if (ScheduleValidation.DayShowingValidation(date))
            {
                ViewBag.Message = "Your schedule is great!";
            }
            else
            {
                ViewBag.Message = "Your schedule is wrong";
            }
            return RedirectToAction("Index");

        }


        // GET: Showings/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: Showings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Showing showing = db.Showings.Find(id);
            db.Showings.Remove(showing);
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
