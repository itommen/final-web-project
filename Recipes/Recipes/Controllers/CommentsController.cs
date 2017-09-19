using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Recipes.Models;

namespace Recipes.Controllers
{
    public class CommentsController : Controller
    {
        private Context db = new Context();

        // GET: Comments
        public ActionResult Index()
        {
            var comments = db.Comments.Include(c => c.Client).Include(c => c.Recipe);
            return View(comments.ToList());
        }

        // GET: Comments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // GET: Comments/Create
        public ActionResult Create()
        {
            if (AuthorizationMiddleware.Authorized(Session))
            {
                ViewBag.ClientID = new SelectList(db.Clients, "ID", "ClientName");
                ViewBag.RecipeID = new SelectList(db.Recipes, "ID", "Content");
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: Comments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ClientID,RecipeID,Content,CreationDate")] Comment comment)
        {
            if (AuthorizationMiddleware.Authorized(Session))
            {
                if (ModelState.IsValid)
                {
                    db.Comments.Add(comment);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.ClientID = new SelectList(db.Clients, "ID", "ClientName", comment.ClientID);
                ViewBag.RecipeID = new SelectList(db.Recipes, "ID", "Content", comment.RecipeID);
                return View(comment);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
              
        }

        // GET: Comments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (AuthorizationMiddleware.Authorized(Session))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Comment comment = db.Comments.Find(id);
                if (comment == null)
                {
                    return HttpNotFound();
                }
                ViewBag.ClientID = new SelectList(db.Clients, "ID", "ClientName", comment.ClientID);
                ViewBag.RecipeID = new SelectList(db.Recipes, "ID", "Content", comment.RecipeID);
                return View(comment);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ClientID,RecipeID,Content,CreationDate")] Comment comment)
        {
            if (AuthorizationMiddleware.Authorized(Session))
            {
                if (ModelState.IsValid)
                {
                    db.Entry(comment).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.ClientID = new SelectList(db.Clients, "ID", "ClientName", comment.ClientID);
                ViewBag.RecipeID = new SelectList(db.Recipes, "ID", "Content", comment.RecipeID);
                return View(comment);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: Comments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (AuthorizationMiddleware.Authorized(Session))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Comment comment = db.Comments.Find(id);
                if (comment == null)
                {
                    return HttpNotFound();
                }
                return View(comment);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
                
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (AuthorizationMiddleware.Authorized(Session))
            {
                Comment comment = db.Comments.Find(id);
                db.Comments.Remove(comment);
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
