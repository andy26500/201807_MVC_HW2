using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using _201807_MVC_HW1.Models;
using _201807_MVC_HW1.ViewModels;

namespace _201807_MVC_HW1.Controllers
{
    public class ContactsController : Controller
    {
        private 客戶聯絡人Repository contactRepo;
        private 客戶資料Repository customerRepo;

        public ContactsController()
        {
            contactRepo = RepositoryHelper.Get客戶聯絡人Repository();
            customerRepo = RepositoryHelper.Get客戶資料Repository(contactRepo.UnitOfWork);
        }

        // GET: Contacts
        public ActionResult Index()
        {
            var contacts = contactRepo.All().Include(客 => 客.客戶資料);
            var titles = contactRepo.All().Select(x => x.職稱).Distinct().ToList();
            ViewBag.Titles = new SelectList(titles);
            return View(contacts.ToList());
        }

        [HttpPost]
        public ActionResult Search(ContactSearchViewModel filter)
        {
            var contacts = contactRepo.Search(filter.Name, filter.Title).Include(客 => 客.客戶資料).ToList();
            var titles = contactRepo.All().Select(x => x.職稱).Distinct().ToList();
            ViewBag.Titles = new SelectList(titles);
            return View("Index", contacts);
        }

        [HttpPost]
        public ActionResult Export(ContactSearchViewModel filter)
        {
            var result = contactRepo
                .Search(filter.Name, filter.Title)
                .Include(客 => 客.客戶資料)
                .Select(x =>
                    new
                    {
                        customerName = x.客戶資料.客戶名稱,
                        title = x.職稱,
                        contactName = x.姓名,
                        email = x.Email,
                        cellPhone = x.手機,
                        phone = x.電話
                    })
                .ToList();

            var dt = GetDataTable();

            result.ForEach(item =>
            {
                var row = dt.NewRow();
                row[0] = item.customerName;
                row[1] = item.title;
                row[2] = item.contactName;
                row[3] = item.email;
                row[4] = item.cellPhone;
                row[5] = item.phone;
                dt.Rows.Add(row);
            });

            return new ExportExcelResult("客戶聯絡人資料.xlsx", dt);
        }

        private DataTable GetDataTable()
        {
            var dt = new DataTable();

            dt.Columns.Add("客戶名稱");
            dt.Columns.Add("職稱");
            dt.Columns.Add("姓名");
            dt.Columns.Add("Email");
            dt.Columns.Add("手機");
            dt.Columns.Add("電話");

            return dt;
        }

        // GET: Contacts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            客戶聯絡人 contact = contactRepo.Find(id.Value);
            if (contact == null)
            {
                return HttpNotFound();
            }

            return View(contact);
        }

        // GET: Contacts/Create
        public ActionResult Create()
        {
            ViewBag.客戶Id = new SelectList(customerRepo.All(), "Id", "客戶名稱");
            return View();
        }

        // POST: Contacts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,客戶Id,職稱,姓名,Email,手機,電話")]
            客戶聯絡人 contact)
        {
            if (ModelState.IsValid)
            {
                contactRepo.Add(contact);
                contactRepo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }

            ViewBag.客戶Id = new SelectList(customerRepo.All(), "Id", "客戶名稱", contact.客戶Id);
            return View(contact);
        }

        // GET: Contacts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            客戶聯絡人 contact = contactRepo.Find(id.Value);
            if (contact == null)
            {
                return HttpNotFound();
            }

            ViewBag.客戶Id = new SelectList(customerRepo.All(), "Id", "客戶名稱", contact.客戶Id);
            return View(contact);
        }

        // POST: Contacts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,客戶Id,職稱,姓名,Email,手機,電話")]
            客戶聯絡人 contact)
        {
            if (ModelState.IsValid)
            {
                contactRepo.UnitOfWork.Context.Entry(contact).State = EntityState.Modified;
                contactRepo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }

            ViewBag.客戶Id = new SelectList(customerRepo.All(), "Id", "客戶名稱", contact.客戶Id);
            return View(contact);
        }

        // GET: Contacts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            客戶聯絡人 contact = contactRepo.Find(id.Value);
            if (contact == null)
            {
                return HttpNotFound();
            }

            return View(contact);
        }

        // POST: Contacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            客戶聯絡人 contact = contactRepo.Find(id);
            contactRepo.Delete(contact);
            contactRepo.UnitOfWork.Commit();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                contactRepo.UnitOfWork.Context.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}