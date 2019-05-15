using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BloodBankSystem.Models
{
    public class BloodDonation
    {
        public int BloodDonationID { get; set; }
        [Required]
        [Display(Name = "Donation Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DonationDate { get; set; }
        [Required]
        public double Volume { get; set; }
        [Required]
        public int NurseID { get; set; }
        [Required]
        public int DonorID { get; set; }
        public Nurse Nurse { get; set; }
        public Donor Donor { get; set; }
    }
}
