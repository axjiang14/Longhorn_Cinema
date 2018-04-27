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
    public enum Report { Seats, Revenue, Both }

    public class ReportsController : Controller
    {
        private AppDbContext db = new AppDbContext();

        // GET: Reports
        public ActionResult Index()
        {
            return View();
        }

        public SelectList GetAllMovies()
        {
            List<Movie> Movies = db.Movies.OrderBy(m => m.Title).ToList();

            Movie SelectNone = new Models.Movie() { MovieID = 0, Title = "All Movies" };

            Movies.Insert(0, SelectNone);

            SelectList AllMovies = new SelectList(Movies, "MovieID", "Title");

            return AllMovies;
        }

        public MultiSelectList GetAllMPAA()
        {
            List<MPAA> AllMPAA = Enum.GetValues(typeof(MPAA))
                                    .Cast<MPAA>()
                                    .ToList(); ;

            MultiSelectList GetMPAA = new MultiSelectList(AllMPAA);

            return GetMPAA;
        }

        public ActionResult GenerateReport()
        {
            ViewBag.AllMovies = GetAllMovies();
            ViewBag.AllMPAA = GetAllMPAA();
            return View();
        }

        public ActionResult DisplayReport(Report ReportCriteria, int SearchMovie, DateTime? StartDate, DateTime? EndDate, DateTime? StartTime, DateTime? EndTime, MPAA MPAARating)
        {
                var query = from ut in db.UserTickets
                            select ut;

                query = query.Where(ut => ut.Status == Status.Active);

                if (SearchMovie != 0)
                {
                    query = query.Where(ut => ut.MovieID == SearchMovie);
                }

                if (StartDate != null)
                {
                    query = query.Where(ut => ut.Showing.ShowDate >= StartDate);
                }

                if (EndDate != null)
                {
                    query = query.Where(ut => ut.Showing.ShowDate <= EndDate);
                }

                if (StartTime != null)
                {
                    DateTime sTime = StartTime ?? new DateTime(1900, 1, 1);
                    query = query.Where(ut => ut.Showing.ShowDate.TimeOfDay >= sTime.TimeOfDay);

                }

                if (EndTime != null)
                {
                    DateTime eTime = StartTime ?? new DateTime(1900, 1, 1);
                    query = query.Where(ut => ut.Showing.ShowDate.TimeOfDay <= eTime.TimeOfDay);
                }

                if (MPAARating != MPAA.All)
                {
                    query = query.Where(ut => ut.Showing.Movie.MPAA_Rating == MPAARating);
                }

            if (ReportCriteria == Report.Seats)
                {

                    List<UserTicket> ReportQuery = new List<UserTicket>();
                    ReportQuery = query.ToList();
                    ViewBag.SoldTicketsCount = ReportQuery.Count();    

                }

                if (ReportCriteria == Report.Revenue)
                {
                    Decimal decRevenue = 0;

                    foreach(UserTicket ut in query)
                    {
						decRevenue = decRevenue + ut.CurrentPrice;
                    }

                    ViewBag.Revenue = decRevenue.ToString("C");

                }

                if (ReportCriteria == Report.Both)
                {
                    List<UserTicket> ReportQuery = new List<UserTicket>();
                    ReportQuery = query.ToList();
                    ViewBag.SoldTicketsCount = ReportQuery.Count();

                    Decimal decRevenue = 0;

                    foreach (UserTicket ut in query)
                    {
                        decRevenue = decRevenue + ut.CurrentPrice;
                    }

                    ViewBag.Revenue = decRevenue.ToString("C");
            }

                List<UserTicket> ReportTickets = new List<UserTicket>();
                ReportTickets = query.ToList();
                return View("../Reports/DisplayReport", ReportTickets);
        }
    }
}
