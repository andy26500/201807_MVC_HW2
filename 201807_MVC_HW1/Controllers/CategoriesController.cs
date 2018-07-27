using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using _201807_MVC_HW1.Models;

namespace _201807_MVC_HW1.Controllers
{
    public class CategoriesController : Controller
    {
        private 客戶分類Repository categoryRepo;

        public CategoriesController()
        {
            categoryRepo = RepositoryHelper.Get客戶分類Repository();
        }

        // GET: Categories
        public ActionResult Index()
        {
            return View(categoryRepo.All().ToList());
        }

        // GET: Categories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            客戶分類 category = categoryRepo.Find(id.Value);
            if (category == null)
            {
                return HttpNotFound();
            }

            return View(category);
        }

        // GET: Categories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,分類名稱")] 客戶分類 category)
        {
            if (ModelState.IsValid)
            {
                categoryRepo.Add(category);
                categoryRepo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }

            return View(category);
        }

        // GET: Categories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            客戶分類 category = categoryRepo.Find(id.Value);
            if (category == null)
            {
                return HttpNotFound();
            }

            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,分類名稱")] 客戶分類 category)
        {
            if (ModelState.IsValid)
            {
                categoryRepo.UnitOfWork.Context.Entry(category).State = EntityState.Modified;
                categoryRepo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }

            return View(category);
        }

        // GET: Categories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            客戶分類 category = categoryRepo.Find(id.Value);
            if (category == null)
            {
                return HttpNotFound();
            }

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            客戶分類 category = categoryRepo.Find(id);
            categoryRepo.Delete(category);
            categoryRepo.UnitOfWork.Commit();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                categoryRepo.UnitOfWork.Context.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}