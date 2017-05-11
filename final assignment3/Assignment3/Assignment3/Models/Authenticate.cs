using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Assignment3.Models
{
    public class Authenticate //model to validate username and password on login activity
    {
        [Required(ErrorMessage = "Username is required to login!")]
        [StringLength(50,ErrorMessage = "The username must be greater than {2} and smaller than {0} characters", MinimumLength = 2)]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password is required to login!")]
        [StringLength(50, ErrorMessage = "The password must be greater than {2} and smaller than {0} characters", MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}