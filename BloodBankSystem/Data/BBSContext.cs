using BloodBankSystem.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloodBankSystem.Data
{
    public class BBSContext: DbContext
    {
        public BBSContext(DbContextOptions<BBSContext> options) : base(options)
        {
        }
        public DbSet<Hospital> Hospitals { get; set; }
        public DbSet<Nurse> Nurses { get; set; }
        public DbSet<Donor> Donors { get; set; }
        public DbSet<BloodDonation> BloodDonations { get; set; }
        //public DbSet<BloodLevel> BloodLevels { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Hospital>().ToTable("Hospital");
            modelBuilder.Entity<Nurse>().ToTable("Nurse");
            modelBuilder.Entity<Donor>().ToTable("Donor");
            modelBuilder.Entity<BloodDonation>().ToTable("BloodDonation");
        }
    }
}
