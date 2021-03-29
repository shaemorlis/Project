using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using DGTradesIn.Models;

namespace DGTradesIn.Controllers
{
    public class CartsController : Controller
    {
        private GameProjectEntities db = new GameProjectEntities();

        // GET: Carts
        public ActionResult Index()
        {
            var carts = db.Carts.Include(c => c.UserGamer);
            return View(carts.ToList());
        }

        // GET: Carts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cart cart = db.Carts.Find(id);
            if (cart == null)
            {
                return HttpNotFound();
            }
            return View(cart);
        }

        // GET: Carts/Create
        public ActionResult Create()
        {
            ViewBag.CreatedBy = new SelectList(db.UserGamers, "GamerID", "GamerID");
            return View();
        }

        // POST: Carts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CartID,CreatedAt,Quantity,TotalPrice,CreatedBy,isCheckedOut")] Cart cart)
        {
            if (ModelState.IsValid)
            {
                db.Carts.Add(cart);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CreatedBy = new SelectList(db.UserGamers, "GamerID", "GamerID", cart.CreatedBy);
            return View(cart);
        }

        // GET: Carts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cart cart = db.Carts.Find(id);
            if (cart == null)
            {
                return HttpNotFound();
            }
            ViewBag.CreatedBy = new SelectList(db.UserGamers, "GamerID", "GamerID", cart.CreatedBy);
            return View(cart);
        }

        // POST: Carts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CartID,CreatedAt,Quantity,TotalPrice,CreatedBy,isCheckedOut")] Cart cart)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cart).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CreatedBy = new SelectList(db.UserGamers, "GamerID", "GamerID", cart.CreatedBy);
            return View(cart);
        }

        // GET: Carts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cart cart = db.Carts.Find(id);
            if (cart == null)
            {
                return HttpNotFound();
            }
            return View(cart);
        }

        // POST: Carts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Cart cart = db.Carts.Find(id);
            db.Carts.Remove(cart);
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

        public ActionResult GetMyCart(int ? user)
        {

            Cart cart= db.Carts.Where(x => x.UserGamer.UserID == user).Where(y=>y.isCartCheckoutFinal.Equals("no")).OrderByDescending(q=>q.CartID).FirstOrDefault();
            if (cart!=null)
            {
                return View("ViewMyCart", cart);
            }
            else
            {
                return Redirect("/home");
            }
            
        }

        public ActionResult checkout()
        {

            if (Session["userID"]==null)
            {
                return Redirect("Account/Login");
            }
            else if (Session["cartID"] == null)
            {
                return Redirect("/home");
            }
            else
            {

                // adding payment to users account 

                //checking accont of use who is buytin
                int cartID = (Int32)Session["cartID"];
                
                Cart cart= db.Carts.Where(x => x.CartID.Equals(cartID)).OrderByDescending(q=>q.CartID).FirstOrDefault();
                User user= db.Carts.Where(x => x.CartID.Equals(cartID)).OrderByDescending(q => q.CartID).FirstOrDefault().UserGamer.User;

                if (cart.TotalPrice>user.Credits)
                {

                    TempData["error"] = "Your Credits are not enough to buy this.";
                    return Redirect("/home");
                }
                else
                {
                    // user is not purchasing it. 
                    // credits will be added to other users. 
                    // depreciated from current user. 


                    // user who is purchasing code. 
                    user.Credits -= (int)cart.TotalPrice;

                    db.Entry(user).State = EntityState.Modified;
                    // now adding credits to other users. 

                    db.SaveChanges();
                    foreach (CartGameCode cartGameCode in cart.CartGameCodes)
                    {
                        User otherUser = db.Users.Where(x => x.UserID.Equals(cartGameCode.GameCode1.UserGamer.UserID)).FirstOrDefault();

                        otherUser.Credits += (int)cart.TotalPrice;

                        db.Entry(otherUser).State = EntityState.Modified;
                        // now adding credits to other users. 
                        // changing the ownership of game code. 
                        GameCode code = db.GameCodes.Where(x=>x.GameCodeAddedBy.Equals(db.UserGamers.Where(y=>y.UserID.Equals(otherUser.UserID)).FirstOrDefault().GamerID)).FirstOrDefault();
                        
                        code.GameCodeAddedBy = db.UserGamers.Where(x=>x.UserID.Equals(user.UserID)).FirstOrDefault().GamerID;
                        db.Entry(code).State=EntityState.Modified;
                        


                        db.SaveChanges();
                        // changing the cart statuss

                        cart.isCheckedOut = Encoding.ASCII.GetBytes("yes"); ;
                        db.Entry(cart).State = EntityState.Modified;
                        db.SaveChanges();
                        // removing values from session
                        Session["cartID"] = null;
                    }
                }
            }
            TempData["Success"] = "Game Codes purchased successfully";
            return Redirect("/home");
        }
    }
}
