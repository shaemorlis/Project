using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DGTradesIn.Models;

namespace DGTradesIn.Controllers
{
    public class TranscationsController : Controller
    {
        private GameProjectEntities db = new GameProjectEntities();

        // GET: Transcations
        public ActionResult Index()
        {

            if (Session["userID"]!=null)
            {
                int userID =(Int32) Session["userID"];
                var transcations = db.Transcations.Where(x => x.TransactionRequestedBy.Equals(db.UserGamers.Where(y=>y.UserID.Equals(userID)).FirstOrDefault().GamerID)).Include(t => t.UserGamer).Include(t => t.UserGamer1);
                return View(transcations.ToList());
            }
            else
            {
                TempData["error"]="Cannot verify user";
                return Redirect("/Home");
            }
            
           
        }

        // GET: Transcations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transcation transcation = db.Transcations.Find(id);
            if (transcation == null)
            {
                return HttpNotFound();
            }
            return View(transcation);
        }

        // GET: Transcations/Create
        public ActionResult Create()
        {
            int userID= 0;
            if (Session["userID"] != null)
            {
                userID = (Int32)Session["userID"];
                //List<GameCode> test = db.GameCodes.Where(x => x.GameCodeAddedBy.Equals(db.UserGamers.Where(y => y.UserID.Equals(userID)).Select(z => z.GamerID))).ToList();
                // var test = db.GameCodes.Where(x => x.GameCodeAddedBy.Equals(db.UserGamers.Where(y => y.UserID.Equals(userID)).Select(z => z.GamerID))


                //x=>x.GameCodeAddedBy.Equals(db.UserGamers.Where(y=>y.UserID.Equals(userID)).Select(z=>z.GamerID))
                //ViewBag.GameCodeOffer = new SelectList(db.GameCodes , "GameCodeTitle", "GameCodeTitle");
                ViewBag.GameCodeOffer = new SelectList(db.GameCodes.Where(x=>x.GameCodeAddedBy.Equals(db.UserGamers.Where(y=>y.UserID.Equals(userID)).FirstOrDefault().GamerID)), "GameCodeTitle", "GameCodeTitle");
                ViewBag.GameCodeRequested = new SelectList(db.GameCodes.Where(x=>x.GameCodeAddedBy.Equals(db.UserGamers.Where(y=>y.UserID.Equals(userID)).FirstOrDefault().GamerID)), "GameCodeTitle", "GameCodeTitle");
                ViewBag.GameCodeRequested = new SelectList(db.GameCodes.Where(x=>x.GameCodeAddedBy.Equals(db.UserGamers.Where(y=>y.UserID.Equals(userID)).FirstOrDefault().GamerID)), "GameCodeTitle", "GameCodeTitle");
            }
            else
            {
                return Redirect("/Account/Login");

            }
               
            
            return View();
        }
        public ActionResult MakeRequest(int gameCodeRequested, int TransactionRequestedTo)
        {
            int userID = 0;
            if (Session["userID"] != null)
            {
                userID = (Int32)Session["userID"];
                //List<GameCode> test = db.GameCodes.Where(x => x.GameCodeAddedBy.Equals(db.UserGamers.Where(y => y.UserID.Equals(userID)).Select(z => z.GamerID))).ToList();
                // var test = db.GameCodes.Where(x => x.GameCodeAddedBy.Equals(db.UserGamers.Where(y => y.UserID.Equals(userID)).Select(z => z.GamerID))


                //x=>x.GameCodeAddedBy.Equals(db.UserGamers.Where(y=>y.UserID.Equals(userID)).Select(z=>z.GamerID))
                //ViewBag.GameCodeOffer = new SelectList(db.GameCodes , "GameCodeTitle", "GameCodeTitle");
                ViewBag.GameCodeOffer = new SelectList(db.GameCodes.Where(x => x.GameCodeAddedBy.Equals(db.UserGamers.Where(y => y.UserID.Equals(userID)).FirstOrDefault().GamerID)), "GameCodeID", "GameCodeTitle");
                
                Transcation t = new Transcation();
                t.TransactionDate = DateTime.Now;
                t.TransactionRequestedBy = db.UserGamers.Where(x => x.UserID.Equals(userID)).FirstOrDefault().GamerID; 
                t.TransactionRequestedTo= TransactionRequestedTo;
                t.GameCodeRequested = gameCodeRequested;
                
                

                return View("CustomView", t);
            }
            else
            {
                return Redirect("/Account/Login");

            }

          
            
        }
        // POST: Transcations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MakeRequest([Bind(Include = "TransactionID,TransactionDate,TransactionResponse,TransactionRequestedBy,TransactionRequestedTo,GameCodeRequested,GameCodeOffer")] Transcation transcation, string GameCodeOffer)
        {


            transcation.TransactionDate = DateTime.Now;
            transcation.TransactionResponse = "Pending";
            
            


            if (ModelState.IsValid)
            {
                db.Transcations.Add(transcation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TransactionRequestedBy = new SelectList(db.UserGamers, "GamerID", "GamerID", transcation.TransactionRequestedBy);
            ViewBag.TransactionRequestedTo = new SelectList(db.UserGamers, "GamerID", "GamerID", transcation.TransactionRequestedTo);
            return View(transcation);
        }
        // POST: Transcations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TransactionID,TransactionDate,TransactionResponse,TransactionRequestedBy,TransactionRequestedTo")] Transcation transcation)
        {
            

            transcation.TransactionDate = DateTime.Now;
            transcation.TransactionResponse = "Pending";
            int userID = (Int32)Session["userID"];
            transcation.TransactionRequestedBy = db.UserGamers.Where(x=>x.UserID.Equals(userID)).FirstOrDefault().GamerID;
            transcation.TransactionRequestedTo = db.UserGamers.Where(x => x.UserID.Equals(userID)).FirstOrDefault().GamerID;
            transcation.GameCodeOffer = 4;
            transcation.GameCodeRequested = 8;
            

            if (ModelState.IsValid)
            {
                db.Transcations.Add(transcation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TransactionRequestedBy = new SelectList(db.UserGamers, "GamerID", "GamerID", transcation.TransactionRequestedBy);
            ViewBag.TransactionRequestedTo = new SelectList(db.UserGamers, "GamerID", "GamerID", transcation.TransactionRequestedTo);
            return View(transcation);
        }

        // GET: Transcations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transcation transcation = db.Transcations.Find(id);
            if (transcation == null)
            {
                return HttpNotFound();
            }
            ViewBag.TransactionRequestedBy = new SelectList(db.UserGamers, "GamerID", "GamerID", transcation.TransactionRequestedBy);
            ViewBag.TransactionRequestedTo = new SelectList(db.UserGamers, "GamerID", "GamerID", transcation.TransactionRequestedTo);



            return View(transcation);
        }

        // POST: Transcations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TransactionID,TransactionDate,TransactionResponse,TransactionRequestedBy,TransactionRequestedTo")] Transcation transcation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(transcation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TransactionRequestedBy = new SelectList(db.UserGamers, "GamerID", "GamerID", transcation.TransactionRequestedBy);
            ViewBag.TransactionRequestedTo = new SelectList(db.UserGamers, "GamerID", "GamerID", transcation.TransactionRequestedTo);
            return View(transcation);
        }

        // GET: Transcations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transcation transcation = db.Transcations.Find(id);
            if (transcation == null)
            {
                return HttpNotFound();
            }
            return View(transcation);
        }

        // POST: Transcations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Transcation transcation = db.Transcations.Find(id);
            db.Transcations.Remove(transcation);
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

        public ActionResult GetMySwapRequests()
        {
            if (Session["userID"] != null)
            {
                int userID = (Int32)Session["userID"];
                var transcations = db.Transcations.Where(x=>x.TransactionRequestedTo.Equals(db.UserGamers.Where(y=>y.UserID.Equals(userID)).FirstOrDefault().GamerID)).Include(t => t.UserGamer).Include(t => t.UserGamer1);
                return View("SwapRequests",transcations.ToList());
            }
            else
            {
                return Redirect("/Home");
            }

        }
        public ActionResult ApproveSwap(int TransactionID)
        {
            if (Session["userID"] != null)
            {
                int userID = (Int32)Session["userID"];
                Transcation tr = db.Transcations.Where(x => x.TransactionID.Equals(TransactionID)).FirstOrDefault();
                if (tr != null)
                {
                    tr.TransactionResponse = "Accepted";
                    // swaping the game codes

                    GameCode ToSwap = tr.GameCode;
                    GameCode SwapWith = tr.GameCode1;

                    ToSwap.GameCodeAddedBy = tr.TransactionRequestedTo;
                    SwapWith.GameCodeAddedBy = tr.TransactionRequestedBy;

                    db.Entry(tr).State = EntityState.Modified;
                    db.Entry(ToSwap).State = EntityState.Modified;
                    db.Entry(SwapWith).State = EntityState.Modified;


                    db.SaveChanges();

                    TempData["success"] = "Game Codes Swapped successfully";
                }
                else
                {
                    TempData["error"] = "Error while decline";
                }



                var transcations = db.Transcations.Where(x => x.TransactionRequestedTo.Equals(userID)).Include(t => t.UserGamer).Include(t => t.UserGamer1);
                return View("SwapRequests", transcations.ToList());
            }
            else
            {
                return Redirect("/Home");
            }
        }
        public ActionResult DeclineSwap(int TransactionID)
        {
            
            if (Session["userID"] != null)
            {
                int userID = (Int32 ) Session["userID"];
                Transcation tr = db.Transcations.Where(x => x.TransactionID.Equals(TransactionID)).FirstOrDefault();
                if (tr != null)
                {
                    tr.TransactionResponse = "Decline";
                    db.Entry(tr).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    TempData["error"] = "Error while decline";
                }
               

                
                var transcations = db.Transcations.Where(x => x.TransactionRequestedTo.Equals(userID)).Include(t => t.UserGamer).Include(t => t.UserGamer1);
                return View("SwapRequests", transcations.ToList());
            }
            else
            {
                return Redirect("/Home");
            }

        }

    }
}
