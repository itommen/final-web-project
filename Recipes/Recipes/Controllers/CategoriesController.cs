using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Recipes.Models;

namespace Recipes.Controllers
{
    public class CategoriesController : Controller
    {
        private Context db = new Context();

        // GET: Catgories
        public ActionResult Index()
        {
            if (AuthorizationMiddleware.AdminAuthorized(Session))
            {
                return View(db.Categories.ToList());
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Category category = db.Categories.Find(id);

            if (category == null)
            {
                return HttpNotFound();
            }

            if (AuthorizationMiddleware.AdminAuthorized(Session))
            {
                return View(category);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: Categories/Create
        public ActionResult Create()
        {
            if (((Client)Session["Client"]) != null && ((Client)Session["Client"]).IsAdmin)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name")] Category category)
        {
            if (AuthorizationMiddleware.AdminAuthorized(Session))
            {
                if (ModelState.IsValid)
                {
                    // Checking if the category already exist
                    var isExist = db.Categories.Where(x => x.Name == category.Name).FirstOrDefault();

                    if (isExist == null)
                    {
                        db.Categories.Add(category);
                        db.SaveChanges();

                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return View(category);
                    }
                }

                return View(category);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }   
        }

        // GET: Categories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (AuthorizationMiddleware.AdminAuthorized(Session))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                Category category = db.Categories.Find(id);

                if (category == null)
                {
                    return HttpNotFound();
                }

                return View(category);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name")] Category category)
        {
            if (AuthorizationMiddleware.AdminAuthorized(Session))
            {
                if (ModelState.IsValid)
                {
                    db.Entry(category).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(category);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: Categories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (AuthorizationMiddleware.AdminAuthorized(Session))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Category category = db.Categories.Find(id);

                if (category == null)
                {
                    return HttpNotFound();
                }

                return View(category);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (AuthorizationMiddleware.AdminAuthorized(Session))
            {
                Category category = db.Categories.Find(id);

                // Getting all the recipes of the category
                List<Recipe> recipes = new List<Recipe>();
                recipes= db.Recipes.Where(x => x.Category.ID == id).ToList();

                // Removing all the recipes of that category
                foreach (Recipe currRecipe in recipes)
                {
                    Recipe recipe = db.Recipes.Find(currRecipe.ID);

                    List<Comment> lstComments = new List<Comment>();
                    lstComments = db.Comments.Where(x => x.RecipeID == currRecipe.ID).ToList();
                    
                    foreach (Comment curComm in lstComments)
                    {
                        db.Comments.Remove(curComm);
                    }

                    db.Recipes.Remove(recipe);
                }

                db.Categories.Remove(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
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
