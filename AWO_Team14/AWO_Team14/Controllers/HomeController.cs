using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AWO_Team14.Models;
using AWO_Team14.DAL;
using System.Net;
using System.Diagnostics;
using Microsoft.AspNet.Identity;

namespace AWO_Team14.Controllers
{
    public enum RangeType { Before, After, Equal}
    public enum StarComp { GreaterThan, LessThan };

    public class HomeController : Controller
    {
        private AppDbContext db = new AppDbContext();
        // GET: Home

        public ActionResult Index()
        {
            var query = from s in db.Showings
                       select s;

            
            query = query.Where(s => s.ShowDate >= DateTime.Now);

            List<Showing> qShowings = query.ToList();

            List<Movie> SelectedMovies = new List<Movie>();

            foreach (Showing s in qShowings)
            {
                Movie m = db.Movies.Find(s.Movie.MovieID);
                if (SelectedMovies.Contains(m) == false)
                {
                    SelectedMovies.Add(m);
                }
            }

            ViewBag.TotalMovies = db.Movies.Count();
            ViewBag.DisplayedMovies = SelectedMovies.Count();

            SelectedMovies.OrderByDescending(m => m.Title);


            if (User.Identity.IsAuthenticated)
            {
                string currentUserId = User.Identity.GetUserId();
                AppUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);
                ViewBag.Name = currentUser.FirstName;
            }

            return View(SelectedMovies);
        }

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
            ViewBag.AllMPAA = GetAllMPAA();
            return View();
        }

        public ActionResult DisplayDetailedSearch(String SearchTitle, String SearchTagline, int[] SearchGenres, String SearchYear, String SearchActors, RangeType SearchYearType, MPAA MPAARating, String SearchStarRatings, StarComp SelectedStar)
        {
            var query = from m in db.Movies
                        select m;

            //List of movies w/ selected genres
            List<Movie> GenresMovies = new List<Movie>();
            List<Movie> FilterMovies = new List<Movie>();

            if (SearchTitle != null)
            {
                query = query.Where(m => m.Title.Contains(SearchTitle));
            }

            if (SearchTagline != null)
            {
                query = query.Where(m => m.Tagline.Contains(SearchTagline));
            }

            if (SearchGenres == null || SearchGenres.Count() == 0)
            {


            }
            else
            {
                //List<Genre> Genres = new List<Genre>();
                GenresMovies = query.ToList(); //List of movies from query
                FilterMovies = query.ToList(); //We need two of these because you can't change the contents of the list during the loop. So we loop through GenresMovies and then remove the movies that don't fit the criteria from FilterMovies

                //foreach (int i in SearchGenres)
                foreach (Movie m in GenresMovies)
                {
                    foreach (int i in SearchGenres)
                    {
                        //If movie doesn't contain the genre, remove the movie from the list of movies
                        if (m.Genres.Contains(db.Genres.Find(i)) == false)
                        {
                            FilterMovies.Remove(m);
                            break;
                        }
                    }
                }
            }

            if (SearchYear != null && SearchYear != "")
            {
                try
                {
                    Int32 DateSelected = Convert.ToInt32(SearchYear);
                    if (SearchYearType == RangeType.After)
                    { query = query.Where(m => m.ReleaseYear >= DateSelected); }
                    else if (SearchYearType == RangeType.Before)
                    { query = query.Where(m => m.ReleaseYear <= DateSelected); }
                    else if (SearchYearType == RangeType.Equal)
                    {
                        query = query.Where(m => m.ReleaseYear == DateSelected);
                    }
                }
                    
                catch
                {
                    ViewBag.Message = SearchYear + "is not a valid number.";
                    ViewBag.AllGenres = GetAllGenres();
                    ViewBag.AllMPAA = GetAllMPAA();
                    return View("DetailedSearch");
                }
            }

            if (SearchActors != null)
            {
                query = query.Where(m => m.Actors.Contains(SearchActors));
            }

            if (MPAARating != MPAA.All)
            {
                query = query.Where(m => m.MPAA_Rating == MPAARating);
            }

            

            //Creates list of selected movies
            List<Movie> OtherMovies = query.ToList();
            List<Movie> FinalMovies = new List<Movie>();
            foreach (Movie item in OtherMovies)
            {
                FinalMovies.Add(item);
            }

            Decimal decSearchStar;
            if (SearchStarRatings != null && SearchStarRatings != "")
            {
                try
                {
                    decSearchStar = Convert.ToDecimal(SearchStarRatings);
                    if (decSearchStar >= 1.0m && decSearchStar <= 5.0m)
                    {
                        if (SelectedStar == StarComp.GreaterThan)
                        {
                            foreach(Movie item in OtherMovies)
                            {
                                if (item.RatingsAvg < decSearchStar)
                                {
                                    FinalMovies.Remove(item);
                                } 
                                
                            }
                                                   
                        }
                        if (SelectedStar == StarComp.LessThan)
                        {
                            foreach (Movie item in OtherMovies)
                            {
                                if (item.RatingsAvg > decSearchStar)
                                {
                                    FinalMovies.Remove(item);
                                }

                            }
                        }
                    }
                    else
                    {
                        ViewBag.Message = SearchStarRatings + "is not a valid number.";
                        ViewBag.AllGenres = GetAllGenres();
                        ViewBag.AllMPAA = GetAllMPAA();
                        return View("DetailedSearch");
                    }
                }

                catch
                {
                    ViewBag.Message = SearchStarRatings + "is not a valid number.";
                    ViewBag.AllGenres = GetAllGenres();
                    ViewBag.AllMPAA = GetAllMPAA();
                    return View("DetailedSearch");
                }
            }
            

            var DisplayedMovies = FinalMovies;

            //If genres were selected, find the movies that have those genres and the other entered search requirements
            if (GenresMovies.Count() != 0)
            {
                Debug.WriteLine("Genres selected");
                DisplayedMovies = FinalMovies.Intersect(FilterMovies).ToList();
            }
            else //If not genres were selected, return the movies with the other filters
            {
                Debug.WriteLine("No genres selected");
                DisplayedMovies = FinalMovies;
            }

            List<Movie> SelectedMovies = DisplayedMovies.ToList();

            //Populates movie counts
            ViewBag.TotalMovies = db.Movies.Count();
            ViewBag.DisplayedMovies = SelectedMovies.Count();

            SelectedMovies.OrderByDescending(m => m.Title);

            return View("../Movies/Index", SelectedMovies);
        }

        public ActionResult ShowdateSearch()
        {
            return View();
        }

        public ActionResult DisplayShowdateSearch(DateTime ShowDate)
        {
            var query = from s in db.Showings
                        select s;

            query = query.Where(s => s.ShowDate >= ShowDate);

            List<Showing> qShowings = query.ToList();

            List<Movie> SelectedMovies = new List<Movie>();

            foreach (Showing s in qShowings)
            {
                Movie m = db.Movies.Find(s.Movie.MovieID);
                if (SelectedMovies.Contains(m) == false)
                { SelectedMovies.Add(m); }
            }

            ViewBag.TotalMovies = db.Movies.Count();
            ViewBag.DisplayedMovies = SelectedMovies.Count();

            SelectedMovies.OrderByDescending(m => m.Title);

            return View("Index", SelectedMovies);


        }


        public MultiSelectList GetAllGenres() //Gets all current genres for the genre dropdown
        {
            List<Genre> Genres = db.Genres.ToList();

            Genre SelectNone = new Models.Genre() { GenreID = 0, GenreName = "All Genres" };
            Genres.Add(SelectNone);

            MultiSelectList AllGenres = new MultiSelectList(Genres.OrderBy(l => l.GenreID), "GenreID", "GenreName");

            return AllGenres;
        }

        public MultiSelectList GetAllMPAA()
        {
            List<MPAA> AllMPAA = Enum.GetValues(typeof(MPAA))
                                    .Cast<MPAA>()
                                    .ToList(); ;

            MultiSelectList GetMPAA = new MultiSelectList(AllMPAA);

            return GetMPAA;
        }

    }
}