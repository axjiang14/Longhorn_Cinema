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
            List<UserTicket> UserTickets = db.UserTickets.Where(ut => ut.Transaction.User.Id == UserID).ToList();
            List<Movie> MoviesToDisplay = new List<Movie>();
            foreach (UserTicket ut in UserTickets)
            {
                //only let users write reviews for movies they have seen
                if (! (MoviesToDisplay.Contains(ut.Showing.Movie)))
                {
                    MoviesToDisplay.Add(ut.Showing.Movie);
                }
                
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
                UserMovieReviews = db.MovieReviews.Where(mr => mr.User.Id == UserID).ToList();
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
                return View();
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
            if (ModelState.IsValid)
            {
                db.Entry(mrToChange).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(movieReview);
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
        public ActionResult Delete(int? id)
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

        // POST: MovieReviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MovieReview movieReview = db.MovieReviews.Find(id);
            db.MovieReviews.Remove(movieReview);
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
