using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BloodBankSystem.Data;
using BloodBankSystem.Models;

namespace BloodBankSystem.Controllers
{
    public class BloodDonationsController : Controller
    {
        private readonly BBSContext _context;

        public BloodDonationsController(BBSContext context)
        {
            _context = context;
        }

        // GET: BloodDonations
        public async Task<IActionResult> Index()
        {
            var bBSContext = _context.BloodDonations.Include(b => b.Donor).Include(b => b.Nurse);
            return View(await bBSContext.ToListAsync());
        }

        // GET: BloodDonations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bloodDonation = await _context.BloodDonations
                .Include(b => b.Donor)
                .Include(b => b.Nurse)
                .FirstOrDefaultAsync(m => m.BloodDonationID == id);
            if (bloodDonation == null)
            {
                return NotFound();
            }

            return View(bloodDonation);
        }

        // GET: BloodDonations/Create
        public IActionResult Create()
        {
            ViewData["DonorID"] = new SelectList(_context.Donors, "DonorID", "FullName");
            ViewData["NurseID"] = new SelectList(_context.Nurses, "NurseID", "FullName");
            return View();
        }

        // POST: BloodDonations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BloodDonationID,DonationDate,Volume,NurseID,DonorID")] BloodDonation bloodDonation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bloodDonation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DonorID"] = new SelectList(_context.Donors, "DonorID", "FullName", bloodDonation.DonorID);
            ViewData["NurseID"] = new SelectList(_context.Nurses, "NurseID", "FullName", bloodDonation.NurseID);
            return View(bloodDonation);
        }

        // GET: BloodDonations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bloodDonation = await _context.BloodDonations.FindAsync(id);
            if (bloodDonation == null)
            {
                return NotFound();
            }
            ViewData["DonorID"] = new SelectList(_context.Donors, "DonorID", "FullName", bloodDonation.DonorID);
            ViewData["NurseID"] = new SelectList(_context.Nurses, "NurseID", "FullName", bloodDonation.NurseID);
            return View(bloodDonation);
        }

        // POST: BloodDonations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BloodDonationID,DonationDate,Volume,NurseID,DonorID")] BloodDonation bloodDonation)
        {
            if (id != bloodDonation.BloodDonationID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bloodDonation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BloodDonationExists(bloodDonation.BloodDonationID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["DonorID"] = new SelectList(_context.Donors, "DonorID", "FullName", bloodDonation.DonorID);
            ViewData["NurseID"] = new SelectList(_context.Nurses, "NurseID", "FullName", bloodDonation.NurseID);
            return View(bloodDonation);
        }

        // GET: BloodDonations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bloodDonation = await _context.BloodDonations
                .Include(b => b.Donor)
                .Include(b => b.Nurse)
                .FirstOrDefaultAsync(m => m.BloodDonationID == id);
            if (bloodDonation == null)
            {
                return NotFound();
            }

            return View(bloodDonation);
        }

        // POST: BloodDonations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bloodDonation = await _context.BloodDonations.FindAsync(id);
            _context.BloodDonations.Remove(bloodDonation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BloodDonationExists(int id)
        {
            return _context.BloodDonations.Any(e => e.BloodDonationID == id);
        }
    }
}
