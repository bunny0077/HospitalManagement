using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HospitalManagement.ViewModels
{
    public class Users
    {

        public int UserId { get; set; }
        [Required(ErrorMessage ="Please Enter Name.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please Enter Email.")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" + @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" + @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",ErrorMessage ="Email Not Valid(example@abc.com)")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please Enter Password.")]
        [StringLength(18, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        [RegularExpression(@"^((?=.*[a-z])(?=.*[A-Z])(?=.*\d)).+$", ErrorMessage = "Password Not Valid(8 characters ,one capital, one numeric and one special character)")] 
        public string Password { get; set; }

        [Required(ErrorMessage = "Please Re-enter Password.")]
        //[StringLength(18, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        //[RegularExpression(@"^((?=.*[a-z])(?=.*[A-Z])(?=.*\d)).+$",ErrorMessage ="Password Not Valid(8 characters ,one capital, one numeric and one special character)")]
        [Compare("Password",ErrorMessage ="Password Doesn't Match")]

        public string ConfirmPassword { get; set; }
    }
}