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
using Microsoft.AspNet.Identity;

namespace AWO_Team14.Controllers
{
    [Authorize]
    public class MovieReviewsController : Controller
    {
        private AppDbContext db = new AppDbContext();

        public SelectList GetUserMovies()
        {
            String UserID = User.Identity.GetUserId();

            //generates a list of tickets that the user has seen in the past
            var ticketsQ = db.UserTickets.Where(ut => ut.Transaction.User.Id == UserID);
            ticketsQ = ticketsQ.Where(ut => ut.Status == Status.Active);
            ticketsQ = ticketsQ.Where(ut => ut.Showing.EndTime < DateTime.Now);
            List<UserTicket> UserTickets = ticketsQ.ToList();

            //creates a list of movies that the user has reviewed
            List<Movie> ReviewedMovies = new List<Movie>();
            AppUser user = db.Users.Find(UserID);
            foreach (MovieReview mr in user.MovieReviews)
            {
                ReviewedMovies.Add(mr.Movie);
            }

            List<Movie> MoviesToDisplay = new List<Movie>();

            //creates a list of all of the movies that the user can review 
            foreach (UserTicket ut in UserTickets)
            {
                //prevents duplicate movies from populating the list
                if (! (MoviesToDisplay.Contains(ut.Showing.Movie)) && !(ReviewedMovies.Contains(ut.Showing.Movie)))
                {
                    MoviesToDisplay.Add(ut.Showing.Movie);
                }
                
            }

            //add a null movie if they don't have movies to review
            if (MoviesToDisplay.Count() == 0)
            {
                // ADD NULL MOVIE
                Movie SelectNone = new Movie() { MovieID = 0, Title = "No Movies to Review", Overview = "no movie", ReleaseYear = 1900, MPAA_Rating = MPAA.All, Runtime = new TimeSpan(0, 0, 0), Actors = "none" };
                MoviesToDisplay.Add(SelectNone);
            }

            SelectList selMovies = new SelectList(MoviesToDisplay, "MovieID", "Title");

            return selMovies;
        }

        [Authorize]
        // GET: MovieReviews
        public ActionResult Index()
        {
            List<MovieReview> UserMovieReviews = db.MovieReviews.ToList();

            if (User.IsInRole("Customer"))
            {
                String UserID = User.Identity.GetUserId();
                UserMovieReviews = db.MovieReviews.Where(mr => mr.User.Id == UserID).OrderBy(mr => mr.Status).ToList();
            }
           
            return View(UserMovieReviews);
        }

        // GET: MovieReviews/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MovieReview movieReview = db.MovieReviews.Find(id);
            if (movieReview == null)
            {
                return HttpNotFound();
            }
            return View(movieReview);
        }

        // GET: MovieReviews/Create
        public ActionResult Create()
        {
            if (GetUserMovies().Count() == 0)
            {
                ViewBag.ErrorMessage = "You don't have any movies to review!!";
                return View("Index");
            }

            ViewBag.AllMovies = GetUserMovies();
            return View();
        }

        // POST: MovieReviews/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MovieReviewID,Rating,Review")] MovieReview movieReview, Int32 SelectedMovie)
        {
            if ((SelectedMovie != 0))
            {
                Movie m = db.Movies.Find(SelectedMovie);
                movieReview.Movie = m;

                String UserID = User.Identity.GetUserId();
                AppUser user = db.Users.Find(UserID);

                movieReview.User = user;

                movieReview.Status = ReviewStatus.Pending;


                if (ModelState.IsValid)
                {

                    db.MovieReviews.Add(movieReview);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                
            }

            ViewBag.AllMovies = GetUserMovies();
            return View(movieReview);

        }

        // GET: MovieReviews/Edit/5
        public ActionResult EditUser(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MovieReview movieReview = db.MovieReviews.Find(id);
            if (movieReview == null)
            {
                return HttpNotFound();
            }
            return View(movieReview);
        }

        // POST: MovieReviews/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUser([Bind(Include = "MovieReviewID,Rating,Review")] MovieReview movieReview)
        {
            MovieReview mrToChange = db.MovieReviews.Include(x => x.User).FirstOrDefault(x => x.MovieReviewID == movieReview.MovieReviewID);
            mrToChange.Status = ReviewStatus.Pending;
            mrToChange.Rating = movieReview.Rating;
            mrToChange.Review = movieReview.Review;

            if (ModelState.IsValid)
            {
                db.Entry(mrToChange).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(movieReview);
        }

        public SelectList NeedApprovalReviews()
        {
            var query = from r in db.MovieReviews
                        select r;

            query = query.Where(r => r.Status == ReviewStatus.Pending);

            List<MovieReview> Reviews = query.ToList();

            SelectList PendingReviews = new SelectList(Reviews.OrderBy(r => r.MovieReviewID), "MovieReviewID", "MovieReviewID");

            return PendingReviews;


        }

        public ActionResult GetPendingReviews()
        {
            ViewBag.PendingReviews = NeedApprovalReviews();

            return View();
        }

        public ActionResult ChangeUserProfile(int Id)
        {
            MovieReview mr = db.MovieReviews.Find(Id);

            return RedirectToAction("EditEmployee", "MovieReviews", new { id = mr.MovieReviewID });
        }

        // GET: MovieReviews/Edit/5
        [Authorize(Roles = "Employee, Manager")]
        public ActionResult EditEmployee(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MovieReview movieReview = db.MovieReviews.Find(id);
            if (movieReview == null)
            {
                return HttpNotFound();
            }
            return View(movieReview);
        }

        // POST: MovieReviews/EmployeeEdit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Employee, Manager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditEmployee([Bind(Include = "MovieReviewID, Rating, Review, Status")] MovieReview movieReview)
        {
            MovieReview mrToChange = db.MovieReviews.Include(x => x.User).FirstOrDefault(x => x.MovieReviewID == movieReview.MovieReviewID);

            mrToChange.Review = movieReview.Review;
            mrToChange.Status = movieReview.Status;

            if (ModelState.IsValid)
            {
                db.Entry(mrToChange).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(mrToChange);
        }

        // GET: MovieReviews/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    MovieReview movieReview = db.MovieReviews.Find(id);
        //    if (movieReview == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(movieReview);
        //}

        //// POST: MovieReviews/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    MovieReview movieReview = db.MovieReviews.Find(id);
        //    db.MovieReviews.Remove(movieReview);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
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
