using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.ModelBinding;
using HospitalManagement.Models;
using HospitalManagement.ViewModels;

namespace HospitalManagement.Controllers.Apis
{

    public class UserController : ApiController
    {
        HospitalManagementEntities _context = new HospitalManagementEntities();

        [HttpGet]
        [Route("api/user/patients")]
        public SysDataTable<View_PatientAllergies> GetPateints(int id)
        {
            List<View_PatientAllergies> data = new List<View_PatientAllergies>();
            //var draw = Request.Form.GetValues("draw").FirstOrDefault();
            NameValueCollection nvc = HttpUtility.ParseQueryString(Request.RequestUri.Query);
            var start = (Convert.ToInt32(nvc["start"]));
            var totalLength = (Convert.ToInt32(nvc["length"])) == 0 ? 10 : (Convert.ToInt32(nvc["length"]));
            var searchvalue = nvc["search[value]"] ?? "";
            var sortcoloumnIndex = Convert.ToInt32(nvc["order[0][column]"]);
            var SortColumn = "";
            var SortOrder = "";
            var sortDirection = nvc["order[0][dir]"] ?? "asc";
            var recordsTotal = 0;
            try
            {
                switch (sortcoloumnIndex)
                {
                    case 0:
                        SortColumn = "PatientId";
                        break;
                    case 1:
                        SortColumn = "Name";
                        break;
                    case 2:
                        SortColumn = "Address";
                        break;
                    case 3:
                        SortColumn = "Email";
                        break;
                    case 4:
                        SortColumn = "DOB";
                        break;
                    case 5:
                        SortColumn = "Phone";
                        break;
                    default:
                        SortColumn = "PatientId";
                        break;
                }
                if (sortDirection == "asc")
                    SortOrder = "asc";
                else
                    SortOrder = "desc";
                data = _context.sp_GetPatientsInfo(start, searchvalue, totalLength, SortColumn, SortOrder,id.ToString()).ToList<View_PatientAllergies>();
                recordsTotal = _context.Patients.Where(c => c.CreatedBy == id && c.IsDeleted == false).Count();//done
            }
            catch (Exception)
            {

            }
            var UserPaged = new SysDataTable<View_PatientAllergies>(data, recordsTotal, recordsTotal);
            //return Json(new { recordsTotal = recordsTotal, recordsFiltered = recordsTotal, data = data });
            return UserPaged;
        }
        [HttpPost]
        [Route("api/user/create")]
        public IHttpActionResult Create(User user)
        {
            if(user != null)
            {
                var checkUser = _context.Users.Where(c => c.Email == user.Email).FirstOrDefault();
                if (checkUser != null)
                    return Json(new { responseText = "AlreadyExists" });
                //Encrypt Password
                string pass = EncryptAndDecrypt.EncryptData(user.Password, EncryptAndDecrypt.EncryptedKey);
                user.Password = pass;

                _context.Users.Add(user);
            }
            _context.SaveChanges();
            return Ok();
        }
    }
}
