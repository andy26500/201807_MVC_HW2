using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.IO;
using System.Text;
using ClosedXML;
using ClosedXML.Excel;

namespace _201807_MVC_HW1.Controllers
{
    public class ExportExcelResult : ActionResult
    {
        public string FileName { get; set; }
        public string SheetName { get; set; }
        public DataTable ExportData { get; set; }

        public ExportExcelResult(DataTable data)
        {
            ExportData = data;
        }

        public ExportExcelResult(string fileName, DataTable data)
        {
            FileName = fileName;
            ExportData = data;
        }

        public ExportExcelResult(string fileName, string sheetName, DataTable data)
        {
            FileName = fileName;
            SheetName = sheetName;
            ExportData = data;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (ExportData == null)
                throw new ArgumentNullException("ExportData", "匯出不可為Null");

            if (string.IsNullOrWhiteSpace(SheetName))
                SheetName = "Sheet1";

            if (string.IsNullOrWhiteSpace(FileName))
                FileName = $"ExportFile_{DateTime.Now.Ticks}.xlsx";

            ExportExcelEventHandler(context);
        }

        private void ExportExcelEventHandler(ControllerContext context)
        {
            try
            {
                var workbook = new XLWorkbook();

                if (ExportData != null)
                {
                    context.HttpContext.Response.Clear();

                    // Encoding
                    context.HttpContext.Response.ContentEncoding = Encoding.UTF8;
                    
                    // Content-Type
                    context.HttpContext.Response.ContentType =
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                    // FileName
                    var browser = context.HttpContext.Request.Browser.Browser;
                    var exportFileName = browser.Equals("FireFox", StringComparison.OrdinalIgnoreCase)
                        ? FileName
                        : HttpUtility.UrlEncode(FileName, Encoding.UTF8);

                    // Header
                    context.HttpContext.Response.Headers.Add("Content-Disposition", $"attachment; filename={exportFileName}");

                    // Add All DataTable
                    workbook.AddWorksheet(ExportData, SheetName);

                    // Write
                    using (MemoryStream stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        stream.WriteTo(context.HttpContext.Response.OutputStream);
                        stream.Close();
                    }
                }

                workbook.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}