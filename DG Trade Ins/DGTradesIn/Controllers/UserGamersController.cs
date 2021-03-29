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
    public class UserGamersController : Controller
    {
        private GameProjectEntities db = new GameProjectEntities();

        // GET: UserGamers
        public ActionResult Index()
        {
            var userGamers = db.UserGamers.Include(u => u.User);
            return View(userGamers.ToList());
        }

        // GET: UserGamers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserGamer userGamer = db.UserGamers.Find(id);
            if (userGamer == null)
            {
                return HttpNotFound();
            }
            return View(userGamer);
        }

        // GET: UserGamers/Create
        public ActionResult Create()
        {
            ViewBag.UserID = new SelectList(db.Users, "UserID", "UserName");
            return View();
        }

        // POST: UserGamers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "GamerID,UserID")] UserGamer userGamer)
        {
            if (ModelState.IsValid)
            {
                db.UserGamers.Add(userGamer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserID = new SelectList(db.Users, "UserID", "UserName", userGamer.UserID);
            return View(userGamer);
        }

        // GET: UserGamers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserGamer userGamer = db.UserGamers.Find(id);
            if (userGamer == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserID = new SelectList(db.Users, "UserID", "UserName", userGamer.UserID);
            return View(userGamer);
        }

        // POST: UserGamers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "GamerID,UserID")] UserGamer userGamer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userGamer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserID = new SelectList(db.Users, "UserID", "UserName", userGamer.UserID);
            return View(userGamer);
        }

        // GET: UserGamers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserGamer userGamer = db.UserGamers.Find(id);
            if (userGamer == null)
            {
                return HttpNotFound();
            }
            return View(userGamer);
        }

        // POST: UserGamers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserGamer userGamer = db.UserGamers.Find(id);
            db.UserGamers.Remove(userGamer);
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
