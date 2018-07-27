using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using _201807_MVC_HW1.Models;
using _201807_MVC_HW1.ViewModels;

namespace _201807_MVC_HW1.Controllers
{
    public class CustomersController : Controller
    {
        private 客戶資料Repository customerRepo;
        private 客戶分類Repository categoryRepo;

        public CustomersController()
        {
            customerRepo = RepositoryHelper.Get客戶資料Repository();
            categoryRepo = RepositoryHelper.Get客戶分類Repository(customerRepo.UnitOfWork);
        }

        // GET: Customers
        public ActionResult Index()
        {
            var data = customerRepo.All().Include(x => x.客戶分類1).ToList();
            ViewBag.Category = new SelectList(categoryRepo.All(), "Id", "分類名稱");
            return View(data);
        }

        [HttpPost]
        public ActionResult Search(CustomerSearchViewModel filter)
        {
            var data = customerRepo.Search(filter.Name, filter.CategoryID).Include(x => x.客戶分類1).ToList();
            ViewBag.Category = new SelectList(categoryRepo.All(), "Id", "分類名稱");
            return View("Index", data);
        }

        [HttpPost]
        public ActionResult Export(CustomerSearchViewModel filter)
        {
            var result = customerRepo
                .Search(filter.Name, filter.CategoryID)
                .Include(客 => 客.客戶分類1)
                .Select(x =>
                    new
                    {
                        customerName = x.客戶名稱,
                        taxID = x.統一編號,
                        phone = x.電話,
                        fax = x.傳真,
                        address = x.地址,
                        email = x.Email,
                        category = x.客戶分類1.分類名稱
                    })
                .ToList();

            var dt = GetDataTable();

            result.ForEach(item =>
            {
                var row = dt.NewRow();
                row[0] = item.customerName;
                row[1] = item.taxID;
                row[2] = item.phone;
                row[3] = item.fax;
                row[4] = item.address;
                row[5] = item.email;
                row[6] = item.category;
                dt.Rows.Add(row);
            });

            return new ExportExcelResult("客戶資料.xlsx", dt);
        }

        private DataTable GetDataTable()
        {
            var dt = new DataTable();

            dt.Columns.Add("客戶名稱");
            dt.Columns.Add("統一編號	");
            dt.Columns.Add("電話");
            dt.Columns.Add("傳真");
            dt.Columns.Add("地址");
            dt.Columns.Add("Email");
            dt.Columns.Add("客戶分類");
            

            return dt;
        }

        // GET: Customers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var customer = customerRepo.Find(id.Value);

            if (customer == null)
            {
                return HttpNotFound();
            }

            return View(customer);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            ViewBag.客戶分類 = new SelectList(categoryRepo.All(), "Id", "分類名稱");
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,客戶名稱,統一編號,電話,傳真,地址,Email,客戶分類")]
            客戶資料 model)
        {
            if (ModelState.IsValid)
            {
                customerRepo.Add(model);
                customerRepo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }

            ViewBag.客戶分類 = new SelectList(categoryRepo.All(), "Id", "分類名稱");
            return View(model);
        }

        // GET: Customers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var customer = customerRepo.Find(id.Value);
            if (customer == null)
            {
                return HttpNotFound();
            }

            ViewBag.客戶分類 = new SelectList(categoryRepo.All(), "Id", "分類名稱", customer.客戶分類);
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,客戶名稱,統一編號,電話,傳真,地址,Email,客戶分類")]
            客戶資料 customer)
        {
            if (ModelState.IsValid)
            {
                customerRepo.UnitOfWork.Context.Entry(customer).State = EntityState.Modified;
                customerRepo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }

            ViewBag.客戶分類 = new SelectList(categoryRepo.All(), "Id", "分類名稱", customer.客戶分類);
            return View(customer);
        }

        // GET: Customers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var customer = customerRepo.Find(id.Value);
            if (customer == null)
            {
                return HttpNotFound();
            }

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var customer = customerRepo.Find(id);
            customerRepo.Delete(customer);
            customerRepo.UnitOfWork.Commit();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                customerRepo.UnitOfWork.Context.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}