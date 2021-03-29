using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DGTradesIn.Models;

namespace DGTradesIn.Controllers
{
    public class GameCodesController : Controller
    {
        private GameProjectEntities db = new GameProjectEntities();
        int userid = 0;
        int GamerID = 0;
        // GET: GameCodes
        public ActionResult Index()
        {

            
            if(Session["userID"]!=null)
                 userid = (Int32)Session["userID"];
            
            var gameCodes = db.GameCodes.Include(g => g.UserGamer);

            gameCodes.ToList();
        
            gameCodes = gameCodes.Where(x => x.GameCodeAddedBy == db.UserGamers.Where(y => y.UserID == userid).FirstOrDefault().GamerID);
            return View(gameCodes);
        }

        // GET: GameCodes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GameCode gameCode = db.GameCodes.Find(id);
            if (gameCode == null)
            {
                return HttpNotFound();
            }
            return View(gameCode);
        }

        // GET: GameCodes/Create
        public ActionResult Create()
        {
            ViewBag.GameCodeAddedBy = new SelectList(db.UserGamers, "GamerID", "GamerID");
            return View();
        }

        //// POST: GameCodes/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "GameCodeID,GameCodeTitle,GameCodeDescription,GameCodePrice,GameCodeDiscount,GameCodeAddedDate,wantSwap")] GameCode gameCode, HttpPostedFileBase GameCodeImage)
        //{

        //    if (Session["userID"] != null)
        //    {
        //        userid = (Int32)Session["userID"];
        //        gameCode.GameCodeAddedBy = db.UserGamers.Where(y => y.UserID == userid).FirstOrDefault().GamerID;
        //        gameCode.GameCodeAddedDate = System.DateTime.Now;

        //        if (ModelState.IsValid)
        //        {
        //            // uploading files

        //            try
        //            {

        //                //Method 2 Get file details from HttpPostedFileBase class    

        //                // getting last image code
        //                int imageCode = db.GameCodes.OrderByDescending(x => x.GameCodeID).FirstOrDefault().GameCodeID;
        //                    if (GameCodeImage != null)
        //                {
        //                    string path = Path.Combine(Server.MapPath("~/GameCodeImages"), Path.GetFileName(""+(imageCode + 1))+".jpeg");
        //                    GameCodeImage.SaveAs(path);
        //                    gameCode.GameCodeImage = "/GameCodeImages/"+  (imageCode + 1)+ ".jpeg";
        //                }
        //                ViewBag.FileStatus = "File uploaded successfully.";
        //            }
        //            catch (Exception)
        //            {
        //                ViewBag.FileStatus = "Error while file uploading."; ;
        //            }



        //            db.GameCodes.Add(gameCode);
        //            db.SaveChanges();
        //            return RedirectToAction("Index");
        //        }

        //        ViewBag.GameCodeAddedBy = new SelectList(db.UserGamers, "GamerID", "GamerID", gameCode.GameCodeAddedBy);
        //        return View(gameCode);
        //    }

        //    else
        //    {
        //        TempData["error"] = "Error while creating Game Code ";


        //        return Redirect("Account/Login");
        //    }

        //}

        // POST: GameCodes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "GameCodeID,GameCodeTitle,GameCodeDescription,GameCodePrice,GameCodeDiscount,GameCodeAddedDate,wantSwap")] GameCode gameCode, HttpPostedFileBase GameCodeImage)
        {

            if (Session["userID"] != null)
            {
                userid = (Int32)Session["userID"];
                gameCode.GameCodeAddedBy = db.UserGamers.Where(y => y.UserID == userid).FirstOrDefault().GamerID;
                gameCode.GameCodeAddedDate = System.DateTime.Now;

                if (ModelState.IsValid)
                {
                    // uploading files

                    try
                    {

                        //Method 2 Get file details from HttpPostedFileBase class    

                        // getting last image code
                        GameCode gameCode1 = db.GameCodes.OrderByDescending(x => x.GameCodeID).FirstOrDefault();


                        if (GameCodeImage != null)
                        {
                            int imageCode;
                            if (gameCode1 != null)
                            {
                                imageCode = gameCode1.GameCodeID;
                            }
                            else
                            {
                                imageCode = 0;
                            }
                            string path = Path.Combine(Server.MapPath("~/GameCodeImages"), Path.GetFileName("" + (imageCode + 1)) + ".jpeg");
                            GameCodeImage.SaveAs(path);
                            gameCode.GameCodeImage = "/GameCodeImages/" + (imageCode + 1) + ".jpeg";
                        }
                        ViewBag.FileStatus = "File uploaded successfully.";
                    }
                    catch (Exception)
                    {
                        ViewBag.FileStatus = "Error while file uploading."; ;
                    }



                    db.GameCodes.Add(gameCode);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.GameCodeAddedBy = new SelectList(db.UserGamers, "GamerID", "GamerID", gameCode.GameCodeAddedBy);
                return View(gameCode);
            }

            else
            {
                TempData["error"] = "Error while creating Game Code ";


                return Redirect("Account/Login");
            }

        }

        public ActionResult UploadFiles(HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
              
            }
            return View("Index");
        }


        // GET: GameCodes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GameCode gameCode = db.GameCodes.Find(id);
            if (gameCode == null)
            {
                return HttpNotFound();
            }
            ViewBag.GameCodeAddedBy = new SelectList(db.UserGamers, "GamerID", "GamerID", gameCode.GameCodeAddedBy);
            return View(gameCode);
        }

        // POST: GameCodes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "GameCodeID,GameCodeImage,GameCodeTitle,GameCodeDescription,GameCodePrice,GameCodeDiscount,GameCodeAddedDate,GameCodeAddedBy")] GameCode gameCode)
        {
            if (ModelState.IsValid)
            {
                db.Entry(gameCode).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.GameCodeAddedBy = new SelectList(db.UserGamers, "GamerID", "GamerID", gameCode.GameCodeAddedBy);
            return View(gameCode);
        }

        // GET: GameCodes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GameCode gameCode = db.GameCodes.Find(id);
            if (gameCode == null)
            {
                return HttpNotFound();
            }
            return View(gameCode);
        }

        // POST: GameCodes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GameCode gameCode = db.GameCodes.Find(id);
            db.GameCodes.Remove(gameCode);
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
