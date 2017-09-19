﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Recipes.Models;
using Recipes.ViewModels;

namespace Recipes.Controllers
{
    public class ClientsController : Controller
    {
        private Context db = new Context();

        // GET: Clients
        public ActionResult Index()
        {
            if (AuthorizationMiddleware.AdminAuthorized(Session))
            {
                return View(db.Clients.ToList());
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: Clients/Details/5
        public ActionResult Details(int? id)
        {
            if (AuthorizationMiddleware.AdminAuthorized(Session))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Client client = db.Clients.Find(id);
                if (client == null)
                {
                    return HttpNotFound();
                }
                return View(client);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: Clients/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Clients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Gender,ClientName,FirstName,LastName,Password,isAdmin")] Client client)
        {
           if (ModelState.IsValid)
           {
                // Checking if the user already exist
                var isExist = db.Clients.Where(x => x.ClientName == client.ClientName).FirstOrDefault();

                if (isExist == null)
                {
                    db.Clients.Add(client);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

            }
            return View(client);
        }

        // GET: Clients/Edit/5
        public ActionResult Edit(int? id)
        {
            if (AuthorizationMiddleware.AdminAuthorized(Session))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Client client = db.Clients.Find(id);
                if (client == null)
                {
                    return HttpNotFound();
                }
                return View(client);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: Clients/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Gender,ClientName,FirstName,LastName,Password,isAdmin")] Client client)
        {
            if (AuthorizationMiddleware.AdminAuthorized(Session))
            {
                if (ModelState.IsValid)
                {
                    db.Entry(client).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(client);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: Clients/Delete/5
        public ActionResult Delete(int? id)
        {
            if (AuthorizationMiddleware.AdminAuthorized(Session))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Client client = db.Clients.Find(id);
                if (client == null)
                {
                    return HttpNotFound();
                }
                return View(client);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login([Bind(Include = "ClientName,Password")] Client client)
        {
            Client clnt = null;
            var pass = client.Password;
            var logonName = client.ClientName;

            try
            {
                clnt = db.Clients.Single(u => u.ClientName.Equals(logonName) && u.Password.Equals(pass));

                if (clnt != null)
                {
                    Session.Add("Client", clnt);
                }

                return RedirectToAction("Index", "Home");
            }
            catch (Exception)
            {
                return RedirectToAction("FailedLogin", "Clients");
            }
        }

        public ActionResult Logout()
        {
            Session.Clear();

            return RedirectToAction("Index", "Home");
        }

        public ActionResult RecipesLogin()
        {
          return View();
        }

        public ActionResult FailedLogin()
        {
            return View();
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (AuthorizationMiddleware.AdminAuthorized(Session))
            {
                Client client = db.Clients.Find(id);

                List<Recipe> recipes = new List<Recipe>();

                // Get the recipes of the user
                recipes = db.Recipes.Where(x => x.ClientID == id).ToList();

                foreach (Recipe currRecipe in recipes)
                {
                    List<Comment> comments = new List<Comment>();
                    comments = db.Comments.Where(x => x.RecipeID == currRecipe.ID).ToList();
                    
                    foreach (Comment currCmt in comments)
                    {
                        db.Comments.Remove(currCmt);
                    }

                    db.Recipes.Remove(currRecipe);
                }

                db.Clients.Remove(client);

                db.SaveChanges();

                if (((Client)Session["Client"]).ID == id)
                {
                    Session.Clear();
                }

                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        
        }

        // GET: Client/Stats
        public ActionResult Stats()
        {
            // join select for users and their recipes
            var query = (from u in db.Clients
                         join recipe in db.Recipes on u.ID equals recipe.ClientID
                         select new UserRecipesViewModel
                         {
                             UserName = u.ClientName,
                             FirstName = u.FirstName,
                             LastName = u.LastName,
                             Title = recipe.Title,
                             ID = u.ID
                         });
            var data = query.ToList();
            return View(data);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [HttpGet]
        public ActionResult Search(string username, string firstname, string lastname)
        {
            if (AuthorizationMiddleware.AdminAuthorized(Session))
            {
                var queryClients = new List<Client>();

                foreach (var client in db.Clients)
                {
                    if (username != null && username.Length > 0 && client.ClientName.Contains(username))
                    {
                        queryClients.Add(client);
                    }
                    else if (firstname != null && firstname.Length > 0 && client.FirstName.Contains(firstname))
                    {
                        queryClients.Add(client);
                    }
                    else if (lastname != null && lastname.Length > 0 && client.LastName.Contains(lastname))
                    {
                        queryClients.Add(client);
                    }
                }

                return View(queryClients.OrderByDescending(x => x.ClientName));
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public ActionResult GetGroupByGender()
        {
            var data = db.Clients.GroupBy(x => x.Gender, client => client, (gender, clients) => new
            {
                Name = gender.ToString(),
                Count = clients.Count()
            });

            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}
