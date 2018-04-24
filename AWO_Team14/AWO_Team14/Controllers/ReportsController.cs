using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AWO_Team14.Controllers
{
    public enum Report {Seats, Revenue, Both}

    public class ReportsController : Controller
    {
        // GET: Reports
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GenerateReport(Report ReportCriteria, int[] SearchMovies, DateTime? StartDate, DateTime? EndDate, DateTime? Time)
        {

        }
    }
}