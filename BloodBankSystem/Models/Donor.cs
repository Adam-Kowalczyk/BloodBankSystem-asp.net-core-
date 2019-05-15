using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BloodBankSystem.Models
{
    
    public class Donor
    {
        public int DonorID { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        [StringLength(50)]
        public string LastName { get; set; }
        [Required]
        [Display(Name = "First Name")]
        [StringLength(50)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(11)]
        [RegularExpression(@"^[0-9]{11}$")]
        public string PESEL { get; set; }
        [Required]
        [Display(Name = "Blood Type")]
        public string BloodType { get; set; }
        [Display(Name = "Full Name")]
        public string FullName
        {
            get
            {
                return LastName + ", " + FirstName;
            }
        }

        public ICollection<BloodDonation> BloodDonations { get; set; }
    }
}
