using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASNTechnosoft.Controllers
{
    public class TimeZoneController : Controller
    {
        // GET: TimeZone
        public ActionResult Index()
        {
            DateTime dateTime_Indian = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));

            DateTime dateTime = DateTime.Now;
            ViewBag.IndianDateTime = dateTime_Indian;
            ViewBag.CurrentDate = dateTime;
            return View();
        
        }
    }
}