using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _201807_MVC_HW1.Models;
using _201807_MVC_HW1.ViewModels;

namespace _201807_MVC_HW1.Controllers
{
    public class StatisticsController : Controller
    {
        private 客戶統計資訊Repository statisticRepo;

        public StatisticsController()
        {
            statisticRepo = RepositoryHelper.Get客戶統計資訊Repository();
        }
        
        // GET: Statistics
        public ActionResult Index()
        {
            var data = statisticRepo.All().ToList();
            return View(data);
        }

        [HttpPost]
        public ActionResult Export(BankSearchViewModel filter)
        {
            var result = statisticRepo
                .All()
                .Select(x =>
                    new
                    {
                        customerName = x.客戶名稱,
                        contacts = x.聯絡人數量,
                        banks = x.銀行帳戶數量
                    })
                .ToList();

            var dt = GetDataTable();

            result.ForEach(item =>
            {
                var row = dt.NewRow();
                row[0] = item.customerName;
                row[1] = item.contacts;
                row[2] = item.banks;
                dt.Rows.Add(row);
            });

            return new ExportExcelResult("客戶統計資料.xlsx", dt);
        }

        private DataTable GetDataTable()
        {
            var dt = new DataTable();

            dt.Columns.Add("客戶名稱");
            dt.Columns.Add("聯絡人數量");
            dt.Columns.Add("銀行帳戶數量");

            return dt;
        }
    }
}