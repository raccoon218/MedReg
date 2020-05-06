using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedReg.Models
{
    public class Visit
    {
        public int Id { get; set; }
        public string VisitDate { get; set; }
        public string VisitType { get; set; }
        public string Diagnosis { get; set; }
        //introduced new unique field - snils
        public string Snils { get; set; }
    }
}
