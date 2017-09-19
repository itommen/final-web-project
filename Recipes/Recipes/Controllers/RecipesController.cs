using System;
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
    public class RecipesController : Controller
    {
        private Context db = new Context();

        // GET: Recipes
        public ActionResult Index()
        {
            var recipes = db.Recipes.Include(p => p.Client).Include(p => p.Category);
            return View(recipes.ToList());
        }

        // GET: Recipes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Recipe recipe = db.Recipes.Find(id);
            if (recipe == null)
            {
                return HttpNotFound();
            }
            return View(recipe);
        }

        // GET: Recipes/DetailsByTitle?title=Hardwierd
        public ActionResult DetailsByTitle(string title)
        {
            if (title == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Recipe recipe = db.Recipes.FirstOrDefault(x => x.Title == title);

            if (recipe == null)
            {
                return HttpNotFound();
            }
            return View("Details", recipe);
        }

        // GET: Recipes/Create
        public ActionResult Create()
        {
            if (AuthorizationMiddleware.Authorized(Session))
            {
                ViewBag.ClientID = new SelectList(db.Clients, "ID", "ClientName");
                ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Name");
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: Recipes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,clientId,CategoryID,Title,Content")] Recipe recipe)
        {
            if (recipe.Content != null && recipe.Title != null && recipe.CategoryID != 0)
            {
                if (AuthorizationMiddleware.Authorized(Session))
                {
                    if (ModelState.IsValid)
                    {
                        recipe.CreationDate = DateTime.Now;
                        db.Recipes.Add(recipe);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }

                    ViewBag.ClientID = new SelectList(db.Clients, "ID", "ClientName", recipe.ClientID);
                    ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Name", recipe.CategoryID);
                    return View(recipe);
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            
            return RedirectToAction("Index", "Home"); 
        }

        // GET: Recipes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (AuthorizationMiddleware.Authorized(Session))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Recipe recipe = db.Recipes.Find(id);
                if (recipe == null)
                {
                    return HttpNotFound();
                }
                ViewBag.ClientID = new SelectList(db.Clients, "ID", "ClientName", recipe.ClientID);
                ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Name", recipe.CategoryID);
                return View(recipe);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: Recipes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,clientId,CategoryID,Title,Content")] Recipe recipe)
        {
            if (recipe.Content != null && recipe.Title != null && recipe.Category.Name != null)
            {
                if (AuthorizationMiddleware.Authorized(Session))
                {
                    if (ModelState.IsValid)
                    {
                        recipe.CreationDate = DateTime.Now;
                        db.Entry(recipe).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    ViewBag.ClientID = new SelectList(db.Clients, "ID", "ClientName", recipe.ClientID);
                    ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Name", recipe.CategoryID);
                    return View(recipe);
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Index", "Home");
        }

        // GET: Recipes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (AuthorizationMiddleware.Authorized(Session))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Recipe recipe = db.Recipes.Find(id);
                if (recipe == null)
                {
                    return HttpNotFound();
                }
                return View(recipe);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: Recipes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (AuthorizationMiddleware.Authorized(Session))
            {

                Recipe recipe = db.Recipes.Find(id);

                // Getting all the comments of the recipe
                List<Comment> lstRemove = new List<Comment>();
                lstRemove = db.Comments.Where(x => x.Recipe.ID == id).ToList();

                // Removing all the comments of that recipe
                foreach (Comment cur in lstRemove)
                {
                    Comment comment = db.Comments.Find(cur.ID);
                    db.Comments.Remove(comment);
                }

                db.Recipes.Remove(recipe);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult PostComment(int clientId, int recipeId, string content)
        {
            if (AuthorizationMiddleware.Authorized(Session))
            {
                Comment comment = new Comment
                {
                    Content = content,
                    ClientID = clientId,
                    RecipeID = recipeId,
                    CreationDate = DateTime.Now
                };

                db.Comments.Add(comment);
                db.SaveChanges();

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
           var query = db.Recipes.Select(x => new RecipeCommentViewModel {Title = x.Title, NumberOfComment = x.Comments.Count}).ToList();
           return View(query);
        }

        public ActionResult StatsJson()
        {
            var query = db.Recipes.Select(x => new RecipeCommentViewModel { Title = x.Title, NumberOfComment = x.Comments.Count }).ToList();
            var data = Json(query, JsonRequestBehavior.AllowGet);
            return data;
        }

        [HttpGet]
        public ActionResult Search(string content, string title, DateTime? date)
        {
            var queryRecipes = new List<Recipe>();

            foreach (var recipe in db.Recipes)
            {
                if (content != null && content.Length > 0 && recipe.Content.Contains(content))
                {
                    queryRecipes.Add(recipe);
                }
                else if (title != null && title.Length > 0 && recipe.Title.Contains(title))
                {
                    queryRecipes.Add(recipe);
                }
                else if (date != null)
                {
                    var formattedDateRecipe = recipe.CreationDate.ToString("MM/dd/yyyy");
                    var formattedDate = date.Value.ToString("MM/dd/yyyy");
                    if (formattedDateRecipe.Equals(formattedDate))
                    {
                        queryRecipes.Add(recipe);
                    }
                }
            }

            return View(queryRecipes.OrderByDescending(x => x.CreationDate));
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
