using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMovie.App_Start;
using WebMovie.Models;

namespace WebMovie.Controllers
{
    public class HomeController : Controller
    {
        MovieDataDataContext data = new MovieDataDataContext();
        public ActionResult Index()
        {
            return View();
        }
       
        public ActionResult About()
        {
            return View(data.THEs.ToList().OrderBy(n => n.Mathe));
        }
        [UserAuthorize]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}