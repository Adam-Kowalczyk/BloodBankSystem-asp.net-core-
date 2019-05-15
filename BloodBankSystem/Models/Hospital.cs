using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BloodBankSystem.Models
{
    public class Hospital
    {
        public int HospitalID { get; set; }
        [Required]
        [Display(Name = "Hospital Name")]
        [StringLength(100)]
        public string Name { get; set; }
        [Required]
        [StringLength(100)]
        public string Address { get; set; }

        public ICollection<Nurse> Nurses { get; set; }
        
        //public BloodLevel BloodLevel { get; set; }
    }
}
