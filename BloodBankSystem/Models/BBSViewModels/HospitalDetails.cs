using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloodBankSystem.Models.BBSViewModels
{
    public class HospitalDetails
    {
        public Hospital Hospital { get; set; }
        public class BL
        {
            public string Type;
            public double Amount;
        }

        public BL[] BloodLevels { get; set; }
    }
}
