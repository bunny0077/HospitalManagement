using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using HospitalManagement.Models;
using HospitalManagement.Report;
using HospitalManagement.ViewModels;
using iTextSharp.text;
using iTextSharp.text.pdf;
using OfficeOpenXml;

namespace HospitalManagement.Controllers
{
    public class ExportDataController : Controller
    {
        HospitalManagementEntities _context = new HospitalManagementEntities();
        // GET: ExportData
        public ActionResult Index()
        {
            return View();
        }
        public void ExportToExcel(int id)
        {
            try
            {
                var PatientData = _context.Patients.Where(c => c.CreatedBy == id).Select(c => new PatientsData
                {
                    Name = c.Name,
                    Email = c.Email,
                    Phone = c.Phone,
                    DOB = c.DOB,
                    Address = c.Address,
                    IsDeleted = c.IsDeleted
                }).ToList();

                ExcelPackage pck = new ExcelPackage();
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Report");

                ws.Cells["A1"].Value = "Patient Data";

                ws.Cells["A3"].Value = "Date";
                ws.Cells["B3"].Value = string.Format("{0:dd MMMM yyyy} at {0:H: mm tt}", DateTimeOffset.Now);

                ws.Cells["A6"].Value = "Name";
                ws.Cells["B6"].Value = "Email";
                ws.Cells["C6"].Value = "Phone Number";
                ws.Cells["D6"].Value = "DOB";
                ws.Cells["E6"].Value = "Address";
                ws.Cells["F6"].Value = "Is Deleted";

                int rowStart = 7;
                foreach (var item in PatientData)
                {

                    ws.Cells[string.Format("A{0}", rowStart)].Value = item.Name;
                    ws.Cells[string.Format("B{0}", rowStart)].Value = item.Email;
                    ws.Cells[string.Format("C{0}", rowStart)].Value = item.Phone;
                    ws.Cells[string.Format("D{0}", rowStart)].Value = item.DOB;
                    ws.Cells[string.Format("E{0}", rowStart)].Value = item.Address;
                    ws.Cells[string.Format("F{0}", rowStart)].Value = item.IsDeleted;
                    rowStart++;
                }

                ws.Cells["A:AZ"].AutoFitColumns();
                Response.Clear();
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment: filename=" + "ExcelReport.xlsx");
                Response.BinaryWrite(pck.GetAsByteArray());
                Response.End();
            }
            catch (Exception)
            {

            }
        }
        public void ExportToPDF(int id)
        {
            var PatientData = _context.Patients.Where(c => c.CreatedBy == id).Select(c => new PatientsData
            {
                Name = c.Name,
                Email = c.Email,
                Phone = c.Phone,
                DOB = c.DOB,
                Address = c.Address,
                IsDeleted = c.IsDeleted
            }).ToList();
            ExtractToPdf.ExportDataPDF(PatientData);
        }
    }
    //PatientDataToPdf patient = new PatientDataToPdf();
    //byte[] abytes = patient.PreparePdf(PatientData);
    //            return File(abytes,"apllication/pdf");










}