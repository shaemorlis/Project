using DGTradesIn.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DGTradesIn.Controllers
{
    public class HomeController : Controller
    {
        private GameProjectEntities db = new GameProjectEntities();
        public ActionResult Index()
        {
            try
            {
                if (Session["userID"] != null)
                {
                    int userID = (Int32)Session["userID"];
                    return View(db.GameCodes.Where(x => !x.GameCodeAddedBy.Equals(db.UserGamers.Where(y => y.UserID.Equals(userID)).FirstOrDefault().GamerID)).ToList());
                }
                else
                {
                    return View(db.GameCodes.ToList());
                }
            }
            catch (Exception ex)
            {
                return View("Nothing is Selected. Exception Occured : "+ ex.Message);
               
            }

            
            //return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}