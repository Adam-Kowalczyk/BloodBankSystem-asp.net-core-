using BloodBankSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloodBankSystem.Data
{
    public class DBInitializer
    {
        public static void Initialize(BBSContext context)
        {
            context.Database.EnsureCreated();

            // Look for any students.
            if(context.Hospitals.Any())
            {
                return; //DB seeded
            }

            var hospitals = new Hospital[]
            {
                new Hospital { Name = "New York Presbyterian Med Center", Address = "2960 Broadway, New York, NY 10027"},
                new Hospital { Name = "Mount Sinai Hospital", Address = "1468 Madison Ave, New York, NY 10029"}

            };
            
            foreach(var h in hospitals)
            {
                context.Hospitals.Add(h);
            }
            context.SaveChanges();

            var nurses = new Nurse[]
            {
                new Nurse { FirstName = "Adela", LastName = "Rosario", PESEL = "95072495285",
                    HireDate = DateTime.Parse("2012-09-01"),
                    HospitalID =  hospitals.Single(h => h.Name == "New York Presbyterian Med Center").HospitalID},
                new Nurse { FirstName = "Kimberly", LastName = "Garrison", PESEL = "81080785425",
                    HireDate = DateTime.Parse("2005-12-05"),
                    HospitalID =  hospitals.Single(h => h.Name == "Mount Sinai Hospital").HospitalID},
                new Nurse { FirstName = "Stephen", LastName = "Wells", PESEL = "88060999799",
                    HireDate = DateTime.Parse("2012-03-07"),
                    HospitalID =  hospitals.Single(h => h.Name == "New York Presbyterian Med Center").HospitalID},
            };

            foreach(var n in nurses)
            {
                context.Nurses.Add(n);
            }
            context.SaveChanges();

            var donors = new Donor[]
            {
                new Donor { FirstName = "Jaiden", LastName = "Mathis", BloodType = "0 Rh+", PESEL = "97122144198"},
                new Donor { FirstName = "Taylor", LastName = "Bird", BloodType = "A Rh-", PESEL = "87051874938"},
                new Donor { FirstName = "Beth", LastName = "Emery", BloodType = "B Rh-", PESEL = "00271433989"},
                new Donor { FirstName = "Chelsea", LastName = "Whitfield", BloodType = "AB Rh+", PESEL = "92111041681"}
            };

            foreach(var d in donors)
            {
                context.Donors.Add(d);
            }
            context.SaveChanges();

            var bloodDonations = new BloodDonation[]
            {
                new BloodDonation{
                    Volume = 500,
                    DonationDate = DateTime.Parse("2019-05-06"),
                    DonorID = donors.Single( d => d.PESEL == "97122144198").DonorID,
                    NurseID = nurses.Single( n => n.PESEL == "95072495285").NurseID
                    
                },
                new BloodDonation{
                    Volume = 400,
                    DonationDate = DateTime.Parse("2019-05-08"),
                    DonorID = donors.Single( d => d.PESEL == "87051874938").DonorID,
                    NurseID = nurses.Single( n => n.PESEL == "95072495285").NurseID
                },
                 new BloodDonation{
                    Volume = 300,
                    DonationDate = DateTime.Parse("2019-02-01"),
                    DonorID = donors.Single( d => d.PESEL == "00271433989").DonorID,
                    NurseID = nurses.Single( n => n.PESEL == "81080785425").NurseID
                },
                  new BloodDonation{
                    Volume = 600,
                    DonationDate = DateTime.Parse("2019-05-13"),
                    DonorID = donors.Single( d => d.PESEL == "00271433989").DonorID,
                    NurseID = nurses.Single( n => n.PESEL == "81080785425").NurseID
                },
                   new BloodDonation{
                    Volume = 340,
                    DonationDate = DateTime.Parse("2019-05-08"),
                    DonorID = donors.Single( d => d.PESEL == "92111041681").DonorID,
                    NurseID = nurses.Single( n => n.PESEL == "88060999799").NurseID
                }

            };

            foreach(var b in bloodDonations)
            {
                context.BloodDonations.Add(b);
            }
            context.SaveChanges();
        }
    }
}
