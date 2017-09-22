using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Recipes.Models;
using Recipes.Models.Db;
using Recipes.ViewModels;

namespace Recipes.Controllers
{
    public class RecipesController : Controller
    {
        private readonly ModelsMapping _db = new ModelsMapping();

        public ActionResult Index()
        {
            var recipes = _db.Recipes.Include(p => p.Client).Include(p => p.Category);

            return View(recipes.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var recipe = _db.Recipes.Find(id);

            if (recipe == null)
            {
                return HttpNotFound();
            }

            return View(recipe);
        }

        public ActionResult DetailsByTitle(string title)
        {
            if (title == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var recipe = _db.Recipes.FirstOrDefault(x => x.Title == title);

            if (recipe == null)
            {
                return HttpNotFound();
            }

            return View("Details", recipe);
        }

        public ActionResult Create()
        {
            if (!AuthorizationMiddleware.Authorized(Session)) return RedirectToAction("Index", "Home");

            ViewBag.ClientID = new SelectList(_db.Clients, "ID", "ClientName");
            ViewBag.CategoryID = new SelectList(_db.Categories, "ID", "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,clientId,CategoryID,Title,Content")] Recipe recipe)
        {
            if (recipe.Content == null || recipe.Title == null || recipe.CategoryId == 0)
            {
                return RedirectToAction("Index", "Home");
            }

            if (!AuthorizationMiddleware.Authorized(Session)) return RedirectToAction("Index", "Home");

            if (ModelState.IsValid)
            {
                recipe.CreationDate = DateTime.Now;
                _db.Recipes.Add(recipe);
                _db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.ClientID = new SelectList(_db.Clients, "ID", "ClientName", recipe.ClientId);
            ViewBag.CategoryID = new SelectList(_db.Categories, "ID", "Name", recipe.CategoryId);

            return View(recipe);
        }

        public ActionResult Edit(int? id)
        {
            if (!AuthorizationMiddleware.Authorized(Session)) return RedirectToAction("Index", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var recipe = _db.Recipes.Find(id);

            if (recipe == null)
            {
                return HttpNotFound();
            }

            ViewBag.ClientID = new SelectList(_db.Clients, "ID", "ClientName", recipe.ClientId);
            ViewBag.CategoryID = new SelectList(_db.Categories, "ID", "Name", recipe.CategoryId);

            return View(recipe);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,clientId,CategoryID,Title,Content")] Recipe recipe)
        {
            if (recipe.Content == null || recipe.Title == null || recipe.Category.Name == null)
            {
                return RedirectToAction("Index", "Home");
            }

            if (!AuthorizationMiddleware.Authorized(Session)) return RedirectToAction("Index", "Home");

            if (ModelState.IsValid)
            {
                recipe.CreationDate = DateTime.Now;
                _db.Entry(recipe).State = EntityState.Modified;
                _db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.ClientID = new SelectList(_db.Clients, "ID", "ClientName", recipe.ClientId);
            ViewBag.CategoryID = new SelectList(_db.Categories, "ID", "Name", recipe.CategoryId);

            return View(recipe);
        }

        public ActionResult Delete(int? id)
        {
            if (!AuthorizationMiddleware.Authorized(Session)) return RedirectToAction("Index", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var recipe = _db.Recipes.Find(id);

            if (recipe == null)
            {
                return HttpNotFound();
            }

            return View(recipe);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!AuthorizationMiddleware.Authorized(Session)) return RedirectToAction("Index", "Home");

            var recipe = _db.Recipes.Find(id);
            var commentsToRemove = _db.Comments.Where(x => x.Recipe.Id == id).ToList();

            foreach (var commentToRemove in commentsToRemove)
            {
                var comment = _db.Comments.Find(commentToRemove.Id);
                _db.Comments.Remove(comment);
            }

            _db.Recipes.Remove(recipe);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult PostComment(int clientId, int recipeId, string content)
        {
            if (!AuthorizationMiddleware.Authorized(Session)) return RedirectToAction("Index", "Home");

            var comment = new Comment
            {
                Content = content,
                ClientId = clientId,
                RecipeId = recipeId,
                CreationDate = DateTime.Now
            };

            _db.Comments.Add(comment);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Stats()
        {
           var query = _db.Recipes.Select(x =>
                new RecipeCommentViewModel {Title = x.Title, NumberOfComment = x.Comments.Count}).ToList();

           return View(query);
        }

        public ActionResult StatsJson()
        {
            var query = _db.Recipes.Select(x =>
                new RecipeCommentViewModel { Title = x.Title, NumberOfComment = x.Comments.Count }).ToList();
            var data = Json(query, JsonRequestBehavior.AllowGet);

            return data;
        }

        [HttpGet]
        public ActionResult Search(string content, string title, DateTime? date)
        {
            var queryRecipes = new List<Recipe>();

            foreach (var recipe in _db.Recipes)
            {
                if (!string.IsNullOrEmpty(content) && recipe.Content.Contains(content))
                {
                    queryRecipes.Add(recipe);
                }
                else if (!string.IsNullOrEmpty(title) && recipe.Title.Contains(title))
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
                _db.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
