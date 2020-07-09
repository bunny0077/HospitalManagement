using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HospitalManagement.Models;
using HospitalManagement.ViewModels;

namespace HospitalManagement.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        HospitalManagementEntities _context = new HospitalManagementEntities();

        public ActionResult Index()
        {
            if (Session["User"] != null)
                return RedirectToAction("UserData", "Home");

            return View();
        }
        public ActionResult UserData()
        {
            if (Session["User"] == null)
                return RedirectToAction("Index", "Home");

            return View();
        }
        [HttpPost]
        public ActionResult Login(User objUser)
        {
            if (ModelState.IsValid)
            {
                string pass = EncryptAndDecrypt.EncryptData(objUser.Password, EncryptAndDecrypt.EncryptedKey);
                var obj = _context.Users.Where(a => a.Email.Equals(objUser.Email) && a.Password.Equals(pass)).FirstOrDefault();
                if (obj != null)
                {
                    Session["UserId"] = obj.UserId;
                    Session["User"] = obj;
                    //return RedirectToAction("UserData", "Home");
                }
                else
                {
                    return Json(new { responseText = "Failure" }, JsonRequestBehavior.AllowGet);
                }
            }
            return RedirectToAction("Index");
        }

        public ActionResult LogOut()
        {
            if (Session["User"] != null)
            {
                Session["UserId"] = null;
                Session["User"] = null;
                Session.Abandon();
                Session.Clear();
                Session.RemoveAll();
                System.Web.Security.FormsAuthentication.SignOut();
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult GetOTP(string email)
        {
            var genOTP = "";
            try
            {
                var getUserDb = _context.Users.Where(c => c.Email == email).FirstOrDefault();
                if (getUserDb == null)
                    return Json(new { responseText = "UserNotFound" }, JsonRequestBehavior.AllowGet);

                genOTP = GenrateOTP.GenerateRandomOTP();

                SendingEmail.SendingEmails(email, email, genOTP, "Sending OTP", "Here is Your OTP :");
            }
            catch (Exception)
            {
                return Json(new { responseText = "Failure" }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { genratedOtp = genOTP, responseText = "Success" }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult ChangeUserPassword(string Email, string Password)
        {
            var EncryptPass = EncryptAndDecrypt.EncryptData(Password, EncryptAndDecrypt.EncryptedKey);
            var getUserDb = _context.Users.Where(c => c.Email == Email).FirstOrDefault();
            if (getUserDb == null)
                return Json(new { responseText = "UserNotFound" }, JsonRequestBehavior.AllowGet);

            getUserDb.Password = EncryptPass;
            _context.SaveChanges();

            return Json(new { responseText = "Success" }, JsonRequestBehavior.AllowGet);
        }

    }
}