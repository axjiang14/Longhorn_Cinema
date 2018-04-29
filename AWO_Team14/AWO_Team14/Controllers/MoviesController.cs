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
    public class MoviesController : Controller
    {
        private AppDbContext db = new AppDbContext();

        // GET: Movies
        public ActionResult Index()
        {
            return View(db.Movies.ToList());
        }

        public MultiSelectList GetAllGenres()
        {
            List<Genre> allGenres = db.Genres.OrderBy(g => g.GenreName).ToList();

            MultiSelectList selGenres = new MultiSelectList(allGenres, "GenreID", "GenreName");

            return selGenres;
        }

        public MultiSelectList GetAllGenres(Movie movie)
        {
            List<Genre> allGenres = db.Genres.OrderBy(g => g.GenreName).ToList();

            List<Int32> SelectedGenres = new List<Int32>();

            foreach (Genre g in movie.Genres)
            {
                SelectedGenres.Add(g.GenreID);
            }

            MultiSelectList selGenres = new MultiSelectList(allGenres, "GenreID", "GenreName", SelectedGenres);

            return selGenres;
        }

        //// GET: Movies/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Movie movie = db.Movies.Find(id);
        //    if (movie == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(movie);
        //}

        // GET: Movies/Create
        public ActionResult Create()
        {
            ViewBag.SelectedGenres = GetAllGenres();
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MovieID,MovieNumber,Title,Tagline,Overview,ReleaseYear,MPAA_Rating,RunTime,Actors")] Movie movie, int[] SelectedGenres)
        {
            movie.MovieNumber = Utilities.GenerateMovieNumber.GetNextMovieNum();

            foreach (int i in SelectedGenres)
            {
                Genre genre = db.Genres.Find(i);
                movie.Genres.Add(genre);
            }
            if (ModelState.IsValid)
            {
                db.Movies.Add(movie);
                db.SaveChanges();
                return RedirectToAction("Details", "Home", new { id = movie.MovieID });

            }

            ViewBag.SelectedGenres = GetAllGenres(movie);
            return View(movie);
        }

        // GET: Movies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = db.Movies.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }

            ViewBag.SelectedGenres = GetAllGenres(movie);
            return View(movie);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MovieID,MovieNumber,Title,Tagline,Overview,ReleaseYear,MPAA_Rating,RunTime,Actors")] Movie movie, int[] SelectedGenres)
        {

            if (ModelState.IsValid)
            {
                //Find movie to change
                Movie movieToChange = db.Movies.Find(movie.MovieID);

                //Remove existing genres
                movieToChange.Genres.Clear();

                //Add new genres
                foreach (int i in SelectedGenres)
                {
                    Genre g = db.Genres.Find(i);
                    movieToChange.Genres.Add(g);
                }

                //Change other properties
                movieToChange.Title = movie.Title;
				movieToChange.MovieNumber = movie.MovieNumber;
                movieToChange.Tagline = movie.Tagline;
                movieToChange.Overview = movie.Overview;
                movieToChange.ReleaseYear = movie.ReleaseYear;
                movieToChange.MPAA_Rating = movie.MPAA_Rating;
                movieToChange.Runtime = movie.Runtime;
                movieToChange.Actors = movie.Actors;

                db.Entry(movieToChange).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "Home", new { id = movie.MovieID });
            }
            ViewBag.SelectedGenres = GetAllGenres(movie);
            return View(movie);
        }

        // GET: Movies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = db.Movies.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Movie movie = db.Movies.Find(id);
            db.Movies.Remove(movie);
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
