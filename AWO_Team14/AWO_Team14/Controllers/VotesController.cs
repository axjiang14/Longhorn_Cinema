using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AWO_Team14.DAL;
using AWO_Team14.Models;
using Microsoft.AspNet.Identity;

namespace AWO_Team14.Controllers
{
    public class VotesController : Controller
    {
        private AppDbContext db = new AppDbContext();

        //// GET: Votes
        //public ActionResult Index()
        //{
        //    return View(db.Votes.ToList());
        //}

        // GET: Votes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vote vote = db.Votes.Find(id);
            if (vote == null)
            {
                return HttpNotFound();
            }
            return View(vote);
        }

        [Authorize]
        // GET: Votes/Create
        public ActionResult Create(Int32 id)
        {
            Vote vote = new Vote();

            MovieReview mr = db.MovieReviews.Find(id);

            vote.MovieReview = mr;

            return View(vote);
        }

        // POST: Votes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Vote vote)
        {

            MovieReview mr = db.MovieReviews.Find(vote.MovieReview.MovieReviewID);
            vote.MovieReview = mr;

            List<Vote> MovieVotes = mr.Votes;

            AppUser CurrentUser = db.Users.Find(User.Identity.GetUserId());

            foreach(Vote v in MovieVotes)
            {
                if(v.User == CurrentUser)
                {
                    return RedirectToAction("Edit", "Votes", new { id = v.VoteID });
                }
            }

            String UserID = User.Identity.GetUserId();
            AppUser user = db.Users.Find(UserID);

            vote.User = user;

            if (ModelState.IsValid)
            {
                db.Votes.Add(vote);
                db.SaveChanges();
                return RedirectToAction("Details", "Home", new {id = vote.MovieReview.Movie.MovieID });
            }

            return View(vote);
        }

        [Authorize]
        // GET: Votes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vote vote = db.Votes.Find(id);
            if (vote == null)
            {
                return HttpNotFound();
            }
            return View(vote);
        }

        // POST: Votes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "VoteID,ThumbVote")] Vote vote)
        {
            if (ModelState.IsValid)
            {
                Vote VoteToChange = db.Votes.Find(vote.VoteID);

                VoteToChange.ThumbVote = vote.ThumbVote;

                db.Entry(VoteToChange).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "Home", new { id = VoteToChange.MovieReview.Movie.MovieID });
            }
            return View(vote);
        }

        //// GET: Votes/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Vote vote = db.Votes.Find(id);
        //    if (vote == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(vote);
        //}

        //// POST: Votes/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Vote vote = db.Votes.Find(id);
        //    db.Votes.Remove(vote);
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
