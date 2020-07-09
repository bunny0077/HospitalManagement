using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Mvc;
using HospitalManagement.Models;
using HospitalManagement.ViewModels;

namespace HospitalManagement.Controllers.Apis
{
    [HandleError]
    public class PatientController : Controller
    {
        HospitalManagementEntities _context = new HospitalManagementEntities();

        // GET: Patient
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult FindById(int id)
        {
            var patientDb = _context.Patients.SingleOrDefault(s => s.PatientId == id);
            if (patientDb == null)
                return Json(new { responseText = "Failure" }, JsonRequestBehavior.AllowGet);

            List<PatientAllergyData> pAllergyData = new List<PatientAllergyData>();
            foreach(var patientAllergy in patientDb.PatientAllergies)
            {
                PatientAllergyData allergyData = new PatientAllergyData()
                {
                    AllergyId= patientAllergy.AllergyId,
                    PatientId = patientAllergy.PatientId
                };
                pAllergyData.Add(allergyData);
            }

            PatientsData patient = new PatientsData()
            {
                PatientId = patientDb.PatientId,
                Name = patientDb.Name,
                Address = patientDb.Address,
                Email = patientDb.Email,
                DOB = patientDb.DOB,
                Phone = patientDb.Phone,
                CreatedBy = patientDb.CreatedBy,
                AllergiesData = pAllergyData
            };

            return Json(new { data = patient, responseText = "Success" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public string Create(Patient patient, string[] allergyIds)
        {
            try
            {
                if (patient != null && patient.PatientId == 0)
                {
                    patient.IsDeleted = false;
                    patient.CreatedDate = DateTime.Now;
                    _context.Patients.Add(patient);
                    _context.SaveChanges();
                }
                else
                {
                    var RemovePatientIds = _context.PatientAllergies.RemoveRange
                        (_context.PatientAllergies.Where(c => c.PatientId == patient.PatientId));

                    var updatePatient = _context.Patients.Single(c => c.PatientId == patient.PatientId);
                    updatePatient.Name = patient.Name;
                    updatePatient.Address = patient.Address;
                    updatePatient.Email = patient.Email;
                    updatePatient.Phone = patient.Phone;
                    updatePatient.DOB = patient.DOB;
                    updatePatient.ModifiedBy = patient.CreatedBy;
                    updatePatient.ModifiedDate = DateTime.Now;
                }
                //Add Allergy
                foreach (var Id in allergyIds)
                {
                    PatientAllergy patientAllergy = new PatientAllergy()
                    {
                        AllergyId = Convert.ToInt32(Id),
                        PatientId = patient.PatientId
                    };
                    _context.PatientAllergies.Add(patientAllergy);
                }
                _context.SaveChanges();
            }
            catch (Exception)
            {

            }
            return "Success";
        }

        public ActionResult DeleteById(int id, int userId)
        {
            var PatientDb = _context.Patients.Single(c => c.PatientId == id);
            if(PatientDb == null)
            {
                return Json(new { responseText = "Failure" }, JsonRequestBehavior.AllowGet);
            }
            PatientDb.IsDeleted = true;
            PatientDb.DeletedBy = userId;
            PatientDb.DeletedDate = DateTime.Now;
            _context.SaveChanges();

            return Json(new { responseText = "Success" }, JsonRequestBehavior.AllowGet);
        }
    }
}