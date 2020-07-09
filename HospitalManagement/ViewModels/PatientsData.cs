using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using HospitalManagement.Models;

namespace HospitalManagement.ViewModels
{
    public class PatientsData
    {
        public int PatientId { get; set; }
        [Required(ErrorMessage ="Please Enter a Name.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please Enter a Email.")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" + @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" + @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Email Not Valid(example@abc.com)")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please Enter a Phone Number.")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Invalid Phone number")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Please Enter a DOB.")]

        public string DOB { get; set; }
        [Required(ErrorMessage = "Please Enter Address.")]
        [MaxLength(100, ErrorMessage ="Character Limit Exceed(100 Characters Only)")]
        public string Address { get; set; }

        public int CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> DeletedBy { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public virtual ICollection<PatientAllergyData> AllergiesData { get; set; }
    }
}