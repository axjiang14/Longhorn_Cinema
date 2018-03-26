using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AWO_Team14.Models;
using AWO_Team14.DAL;
using System.Net;


namespace AWO_Team14.Controllers
{
    public class HomeController : Controller
    {
        private AppDbContext db = new AppDbContext();
        // GET: Home

        //public ActionResult Index()
        //{
        //    return View(db.Movies.ToList());
       // }
        public ActionResult Index(String BasicMovieSearch)
        {
            List<Movie> DisplayedMovies = new List<Movie>();

            var query = from m in db.Movies
                        select m;
            if (BasicMovieSearch != null)
            {
                query = query.Where(m => m.Title.Contains(BasicMovieSearch));
            }

            DisplayedMovies = query.ToList();

            ViewBag.TotalMovies = db.Movies.Count();
            ViewBag.DisplayedMovies = DisplayedMovies.Count();

            return View(DisplayedMovies.OrderByDescending(m => m.Title));


        }

        public ActionResult DetailedSearch()
        {
            ViewBag.AllGenres = GetAllGenres();
            return View();
        }

        public ActionResult Details(int? id)
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

        public SelectList GetAllGenres() //Gets all current genres for the genre dropdown
        {
            List<Genre> Genres = db.Genres.ToList();

            Genre SelectNone = new Models.Genre() { GenreID = 0, GenreName = "All Languages" };
            Genres.Add(SelectNone);

            SelectList AllGenres = new SelectList(Genres.OrderBy(l => l.GenreID), "GenreID", "GenreName");

            return AllGenres;
        }
    }

    
}