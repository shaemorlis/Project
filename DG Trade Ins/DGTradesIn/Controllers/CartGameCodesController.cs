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
    public class CartGameCodesController : Controller
    {
        private GameProjectEntities db = new GameProjectEntities();

        // GET: CartGameCodes
        public ActionResult Index()
        {
            var cartGameCodes = db.CartGameCodes.Include(c => c.Cart).Include(c => c.GameCode1);
            return View(cartGameCodes.ToList());
        }

        // GET: CartGameCodes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CartGameCode cartGameCode = db.CartGameCodes.Find(id);
            if (cartGameCode == null)
            {
                return HttpNotFound();
            }
            return View(cartGameCode);
        }

        // GET: CartGameCodes/Create
        public ActionResult Create()
        {
            ViewBag.CartID = new SelectList(db.Carts, "CartID", "CartID");
            ViewBag.GameCode = new SelectList(db.GameCodes, "GameCodeID", "GameCodeImage");
            return View();
        }

        // POST: CartGameCodes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CartGameCodeID,GameCode,CartID")] CartGameCode cartGameCode)
        {
            if (ModelState.IsValid)
            {
                db.CartGameCodes.Add(cartGameCode);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CartID = new SelectList(db.Carts, "CartID", "CartID", cartGameCode.CartID);
            ViewBag.GameCode = new SelectList(db.GameCodes, "GameCodeID", "GameCodeImage", cartGameCode.GameCode);
            return View(cartGameCode);
        }

        // GET: CartGameCodes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CartGameCode cartGameCode = db.CartGameCodes.Find(id);
            if (cartGameCode == null)
            {
                return HttpNotFound();
            }
            ViewBag.CartID = new SelectList(db.Carts, "CartID", "CartID", cartGameCode.CartID);
            ViewBag.GameCode = new SelectList(db.GameCodes, "GameCodeID", "GameCodeImage", cartGameCode.GameCode);
            return View(cartGameCode);
        }

        // POST: CartGameCodes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CartGameCodeID,GameCode,CartID")] CartGameCode cartGameCode)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cartGameCode).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CartID = new SelectList(db.Carts, "CartID", "CartID", cartGameCode.CartID);
            ViewBag.GameCode = new SelectList(db.GameCodes, "GameCodeID", "GameCodeImage", cartGameCode.GameCode);
            return View(cartGameCode);
        }

        // GET: CartGameCodes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CartGameCode cartGameCode = db.CartGameCodes.Find(id);
            if (cartGameCode == null)
            {
                return HttpNotFound();
            }
            return View(cartGameCode);
        }

        // POST: CartGameCodes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CartGameCode cartGameCode = db.CartGameCodes.Find(id);
            db.CartGameCodes.Remove(cartGameCode);
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

        public ActionResult addItemToCart(int? gameCode)
        {
            
            if (Session["userCart"]==null)
            {
                int userid;
                if (Session["userID"] != null)
                {
                    userid = (Int32)Session["userID"];

                }
                else
                {
                    return Redirect("/Account/Login");
                }
               
                Cart cart = new Cart();
                cart.CreatedAt = DateTime.Now;
                cart.CreatedBy =(Int32) Session["userID"];
                cart.UserGamer = db.UserGamers.Where(x=>x.UserID.Equals(cart.CreatedBy)).First();
                GameCode code = db.GameCodes.Find(gameCode);
                cart.TotalPrice = 0;
                cart.Quantity = 0;
                cart.TotalPrice += code.GameCodePrice-code.GameCodeDiscount;
                cart.Quantity++;
                cart.isCartCheckoutFinal = "no";
                db.Carts.Add(cart);

                db.SaveChanges();
               
                // adding to session
                Session["userCart"] ="yes";
                Session["cartID"] = db.Carts.Where(x=>x.UserGamer.GamerID.Equals(cart.UserGamer.GamerID)).Where(y=>y.isCartCheckoutFinal.Equals("no")).OrderByDescending(q=>q.CartID).FirstOrDefault().CartID;
                // adding products to carts. 

                CartGameCode cartGameCode = new CartGameCode();
                cartGameCode.CartID = (Int32)Session["cartID"];
                cartGameCode.GameCode = (int)gameCode;

               
             
                db.Carts.Where(y=>y.UserGamer.UserID.Equals(userid)).OrderByDescending(q=>q.CartID).FirstOrDefault().CartGameCodes.Add(cartGameCode);
                db.SaveChanges();

            }
            else if(Session["userCart"].ToString().Equals("yes"))
            {
                int userid = (Int32)Session["userID"];
                int cartID= (Int32)Session["cartID"]; ;
                CartGameCode cartGameCode = new CartGameCode();
                cartGameCode.CartID = (Int32)Session["cartID"];
                cartGameCode.GameCode = (int)gameCode;
                GameCode code = db.GameCodes.Find(gameCode);
                db.Carts.Find(cartID).TotalPrice += code.GameCodePrice-code.GameCodeDiscount;
                db.Carts.Find(cartID).Quantity++;
                
                db.Carts.Where(y => y.UserGamer.UserID.Equals(userid)).OrderByDescending(q => q.CartID).FirstOrDefault().CartGameCodes.Add(cartGameCode);
                db.SaveChanges();
            }
            TempData["success"] = "Game added to cart";
            
            return Redirect("/Home");
        }
    }
}
