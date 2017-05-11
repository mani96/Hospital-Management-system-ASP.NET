using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Assignment3.Models
{
    public class ViewModel
    {
        public IEnumerable<User> Doctors { get; set; }
        public IEnumerable<Patient> Patients { get; set; }

    }
}