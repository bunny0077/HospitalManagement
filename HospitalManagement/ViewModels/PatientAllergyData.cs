using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalManagement.ViewModels
{
    public class PatientAllergyData
    {
        public int PatientAllergyId { get; set; }
        public Nullable<int> PatientId { get; set; }
        public Nullable<int> AllergyId { get; set; }
    }
}