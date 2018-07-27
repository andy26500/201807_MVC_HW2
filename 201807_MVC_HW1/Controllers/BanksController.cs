using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Data;
using _201807_MVC_HW1.Models;
using _201807_MVC_HW1.ViewModels;

namespace _201807_MVC_HW1.Controllers
{
    public class BanksController : Controller
    {
        private 客戶資料Repository customerRepo;
        private 客戶銀行資訊Repository bankRepo;

        public BanksController()
        {
            customerRepo = RepositoryHelper.Get客戶資料Repository();
            bankRepo = RepositoryHelper.Get客戶銀行資訊Repository(customerRepo.UnitOfWork);
        }

        // GET: Banks
        public ActionResult Index()
        {
            var banks = bankRepo.All().Include(客 => 客.客戶資料);
            return View(banks.ToList());
        }

        [HttpPost]
        public ActionResult Search(BankSearchViewModel filter)
        {
            var result = bankRepo.Search(filter.CustomerName).Include(客 => 客.客戶資料).ToList();
            return View("Index", result);
        }

        [HttpPost]
        public ActionResult Export(BankSearchViewModel filter)
        {
            var result = bankRepo
                .Search(filter.CustomerName)
                .Include(客 => 客.客戶資料)
                .Select(x =>
                new
                {
                    customerName = x.客戶資料.客戶名稱,
                    bankName = x.銀行名稱,
                    bankCode = x.銀行代碼,
                    branchCode = x.分行代碼,
                    accountName = x.帳戶名稱,
                    accountNo = x.帳戶號碼
                })
                .ToList();

            var dt = GetDataTable();

            result.ForEach(item =>
            {
                var row = dt.NewRow();
                row[0] = item.customerName;
                row[1] = item.bankName;
                row[2] = item.bankCode;
                row[3] = item.branchCode;
                row[4] = item.accountName;
                row[5] = item.accountNo;
                dt.Rows.Add(row);
            });

            return new ExportExcelResult("客戶銀行資料.xlsx", dt);
        }

        private DataTable GetDataTable()
        {
            var dt = new DataTable();

            dt.Columns.Add("客戶名稱");
            dt.Columns.Add("銀行名稱");
            dt.Columns.Add("銀行代碼");
            dt.Columns.Add("分行代碼");
            dt.Columns.Add("帳戶名稱");
            dt.Columns.Add("帳戶號碼");

            return dt;
        }

        // GET: Banks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var bank = bankRepo.Find(id.Value);
            if (bank == null)
            {
                return HttpNotFound();
            }

            return View(bank);
        }

        // GET: Banks/Create
        public ActionResult Create()
        {
            ViewBag.客戶Id = new SelectList(customerRepo.All(), "Id", "客戶名稱");
            return View();
        }

        // POST: Banks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,客戶Id,銀行名稱,銀行代碼,分行代碼,帳戶名稱,帳戶號碼")]
            客戶銀行資訊 bank)
        {
            if (ModelState.IsValid)
            {
                bankRepo.Add(bank);
                bankRepo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }

            ViewBag.客戶Id = new SelectList(customerRepo.All(), "Id", "客戶名稱", bank.客戶Id);
            return View(bank);
        }

        // GET: Banks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            客戶銀行資訊 bank = bankRepo.Find(id.Value);
            if (bank == null)
            {
                return HttpNotFound();
            }

            ViewBag.客戶Id = new SelectList(customerRepo.All(), "Id", "客戶名稱", bank.客戶Id);
            return View(bank);
        }

        // POST: Banks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,客戶Id,銀行名稱,銀行代碼,分行代碼,帳戶名稱,帳戶號碼")]
            客戶銀行資訊 bank)
        {
            if (ModelState.IsValid)
            {
                bankRepo.UnitOfWork.Context.Entry(bank).State = EntityState.Modified;
                bankRepo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }

            ViewBag.客戶Id = new SelectList(customerRepo.All(), "Id", "客戶名稱", bank.客戶Id);
            return View(bank);
        }

        // GET: Banks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            客戶銀行資訊 bank = bankRepo.Find(id.Value);
            if (bank == null)
            {
                return HttpNotFound();
            }

            return View(bank);
        }

        // POST: Banks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            客戶銀行資訊 bank = bankRepo.Find(id);
            bankRepo.Delete(bank);
            bankRepo.UnitOfWork.Commit();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                bankRepo.UnitOfWork.Context.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}