using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedReg.Models
{
    public class Patient
    {
        public int Id { get; set; }
        public string Family { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string Birthday { get; set; }
        public string Adress { get; set; }
        public string PhoneNumber { get; set; }
        //introduced new unique field - snils
        public string Snils { get; set; }
        //uncoment two line next for change the model
        //public string Height { get; set; }
        //public string Weight { get; set; }

    }
}
