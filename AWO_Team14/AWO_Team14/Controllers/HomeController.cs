using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AWO_Team14.Models;
using AWO_Team14.DAL;
using System.Net;
using System.Diagnostics;


namespace AWO_Team14.Controllers
{
    public class HomeController : Controller
    {
        private AppDbContext db = new AppDbContext();
        // GET: Home

        public ActionResult Index()
        {
            //Change this line later when we write validation for getting all the movies showing on today's date
            ViewBag.DisplayedMovies = db.Movies.Count();
            ViewBag.TotalMovies = db.Movies.Count();
            return View(db.Movies.ToList());
        }

        //Todo: Scaffold view for BasicMovieSearch
        public ActionResult BasicSearch(String BasicMovieSearch)
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

            return View("Index", DisplayedMovies.OrderByDescending(m => m.Title));


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

        public ActionResult DetailedSearch()
        {
            ViewBag.AllGenres = GetAllGenres();
            return View();
        }

        public ActionResult DisplayDetailedSearch(String SearchTitle, String SearchTagline, int[] SearchGenres, DateTime? SearchYear, String SearchActors)
        {
            var query = from m in db.Movies
                        select m;
            List<Movie> GenresMovies = new List<Movie>();

            if (SearchTitle != null)
            {
                query = query.Where(m => m.Title.Contains(SearchTitle));
            }

            if (SearchTagline != null)
            {
                query = query.Where(m => m.Tagline.Contains(SearchTagline));
            }

            if (SearchGenres.Count() != 0)
            {
                //List<Genre> Genres = new List<Genre>();

                foreach (int i in SearchGenres)
                {
                    var Genre = db.Genres.Find(i);
                    //Genres.Add(GenreID);
                    var movie = db.Movies.Find(Genre.GenreID);
                    GenresMovies.Add(movie);
                }

                //foreach (var item in Genres)
                //{
                //     var movie = db.Movies.Find(item.GenreID);
                //     query = query.Where(Genres.Contains(item));
                    
                //}

            }

            if (SearchYear != null)
            {
                DateTime DateSelected = SearchYear ?? new DateTime(1900, 1, 1);
                query = query.Where(m => m.ReleaseYear >= DateSelected);
            }

            if (SearchActors != null)
            {
                query = query.Where(m => m.Actors.Contains(SearchActors));
            }

            //Creates list of selected movies
            List<Movie> OtherMovies = query.ToList();

            var DisplayedMovies = OtherMovies.Intersect(GenresMovies);

            //Populates movie counts
            ViewBag.TotalMovies= db.Movies.Count();
            ViewBag.DisplayedMovies = DisplayedMovies.Count();

            DisplayedMovies.OrderByDescending(m => m.Title);

            return View("Index", DisplayedMovies);
        }

        public MultiSelectList GetAllGenres() //Gets all current genres for the genre dropdown
        {
            List<Genre> Genres = db.Genres.ToList();

            Genre SelectNone = new Models.Genre() { GenreID = 0, GenreName = "All Genres" };
            Genres.Add(SelectNone);

            MultiSelectList AllGenres = new MultiSelectList(Genres.OrderBy(l => l.GenreID), "GenreID", "GenreName");

            return AllGenres;
        }

        //public SelectList GetAllMPAA()
        //{
        //    List<>
        //}
    }

    
}