using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Mvc;
using HospitalManagement.Models;
using HospitalManagement.ViewModels;

namespace HospitalManagement.Controllers.Apis
{
    public class AllergyController : ApiController
    {
        HospitalManagementEntities _context = new HospitalManagementEntities();

        //[Route("api/allergies")]
        public IEnumerable<Allergies> Get()
        {
            _context.Database.Log = Console.Write;
            List<Allergies> allergies = new List<Allergies>();
            var allergy = _context.Allergies.ToList();
            foreach(var alg in allergy)
            {
                Allergies al = new Allergies();
                al.AllergyName = alg.AllergyName;
                al.Id = alg.Id;
                allergies.Add(al);
            }
            return allergies;
        }

    }
}
