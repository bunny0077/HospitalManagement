using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using HospitalManagement.ViewModels;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace HospitalManagement.Report
{
    public class ExtractToPdf
    {
        public static void ExportDataPDF(List<PatientsData> patientsData)
        {
            //var PatientData = _context.Patients.Where(c => c.CreatedBy == id).Select(c => new PatientsData
            //{
            //    Name = c.Name,
            //    Email = c.Email,
            //    Phone = c.Phone,
            //    DOB = c.DOB,
            //    Address = c.Address,
            //    IsDeleted = c.IsDeleted
            //}).ToList();
            string fileName = Guid.NewGuid() + ".pdf";
            string filePath = Path.Combine(@"C:/Users/ss/source/repos/HospitalManagement/HospitalManagement/Report",fileName);
            Document doc = new Document(PageSize.A4, 2, 2, 2, 2);
            // Create paragraph for show in PDF file header
            Paragraph p = new Paragraph("Patient Data");
            //p.SetAlignment("center");

            try
            {
                PdfWriter.GetInstance(doc, new FileStream(filePath, FileMode.Create));
                //Create table here for write database data
                PdfPTable pdfTab = new PdfPTable(6); // here 4 is no of column
                pdfTab.HorizontalAlignment = 1; // 0- Left, 1- Center, 2- right
                pdfTab.SpacingBefore = 8f;
                pdfTab.SpacingAfter = 8f;


                pdfTab.AddCell("Name");
                pdfTab.AddCell("Email");
                pdfTab.AddCell("Phone");
                pdfTab.AddCell("DOB");
                pdfTab.AddCell("Address");
                pdfTab.AddCell("Is Deleted");
                pdfTab.CompleteRow();

                foreach (var patient in patientsData)
                {
                    pdfTab.AddCell(patient.Name);
                    pdfTab.AddCell(patient.Email);
                    pdfTab.AddCell(patient.Phone);
                    pdfTab.AddCell(patient.DOB);
                    pdfTab.AddCell(patient.Address);
                    pdfTab.AddCell(patient.IsDeleted.ToString());
                    pdfTab.CompleteRow();
                }
                doc.Open();
                doc.Add(p);
                doc.Add(pdfTab);
                doc.Close();
                byte[] content = File.ReadAllBytes(filePath);
                HttpContext context = HttpContext.Current;

                context.Response.BinaryWrite(content);
                context.Response.ContentType = "application/pdf";
                context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
                context.Response.End();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                doc.Close();
            }
        }
    }
}