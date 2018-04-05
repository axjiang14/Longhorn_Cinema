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
    public class ShowingsController : Controller
    {
        private AppDbContext db = new AppDbContext();

        // GET: Showings
        public ActionResult Index()
        {
            return View(db.Showings.ToList());
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
        public ActionResult Create([Bind(Include = "ShowingID,ShowDate,ShowTime,Special,Theater")] Showing showing, int SelectedMovie)
        {
            Movie m = db.Movies.Find(SelectedMovie);

            showing.Movie = m;

            if (ModelState.IsValid)
            {
                db.Showings.Add(showing);
                db.SaveChanges();
                return RedirectToAction("Index");
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ShowingID,ShowDate,ShowTime,Special,Theater")] Showing showing, int SelectedMovie)
        {

            if (ModelState.IsValid)
            {
                //Find showing to change
                Showing showingToChange = db.Showings.Find(showing.ShowingID);

                //Remove existing movie
                showingToChange.Movie = null;

                Movie movie = db.Movies.Find(SelectedMovie);

                showingToChange.Movie = movie;



                //showingToChange.Movie = movie;
                showingToChange.ShowDate = showing.ShowDate;
                showingToChange.ShowTime = showing.ShowTime;
                showingToChange.Special = showing.Special;
                showingToChange.Theater = showing.Theater;

                db.Entry(showingToChange).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AllMovies = GetAllMovies();
            return View(showing);
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
