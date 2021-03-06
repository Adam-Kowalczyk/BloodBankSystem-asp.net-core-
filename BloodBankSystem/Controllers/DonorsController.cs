﻿using System;
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
    public class DonorsController : Controller
    {
        public string[] bloodTypes = new string[]
        {
            "0 Rh-","0 Rh+",
            "A Rh-","A Rh+",
            "B Rh-","B Rh+",
            "AB Rh-","AB Rh+"
        };
        private readonly BBSContext _context;

        public DonorsController(BBSContext context)
        {
            _context = context;
        }

        // GET: Donors
        public async Task<IActionResult> Index(string searchString)
        {
            ViewData["currentFilter"] = "";
            var donors = from h in _context.Donors select h;
            if (!String.IsNullOrEmpty(searchString))
            {
                donors = donors.Where(h => h.FirstName.Contains(searchString) || h.LastName.Contains(searchString));
                ViewData["currentFilter"] = searchString;
            }
            return View(await donors.AsNoTracking().ToListAsync());
        }

        // GET: Donors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donor = await _context.Donors
                .Include(d=>d.BloodDonations)
                    .ThenInclude(b => b.Nurse)
                        .ThenInclude(n => n.Hospital)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.DonorID == id);
            if (donor == null)
            {
                return NotFound();
            }

            ViewData["GivenBloodVolume"] = _context.BloodDonations
                                            .Where(b => b.DonorID == id)
                                            .Sum(b => b.Volume);
            
            return View(donor);
        }

        // GET: Donors/Create
        public IActionResult Create()
        {
            ViewData["BloodTypes"] = new SelectList(bloodTypes, "BloodType");
            return View();
        }

        // POST: Donors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DonorID,LastName,FirstName,PESEL,BloodType")] Donor donor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(donor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(donor);
        }

        // GET: Donors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donor = await _context.Donors.FindAsync(id);
            if (donor == null)
            {
                return NotFound();
            }
            ViewData["BloodTypes"] = new SelectList(bloodTypes, "BloodType");
            return View(donor);
        }

        // POST: Donors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DonorID,LastName,FirstName,PESEL,BloodType")] Donor donor)
        {
            if (id != donor.DonorID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(donor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DonorExists(donor.DonorID))
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
            return View(donor);
        }

        // GET: Donors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donor = await _context.Donors
                .FirstOrDefaultAsync(m => m.DonorID == id);
            if (donor == null)
            {
                return NotFound();
            }

            return View(donor);
        }

        // POST: Donors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var donor = await _context.Donors.FindAsync(id);
            _context.Donors.Remove(donor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DonorExists(int id)
        {
            return _context.Donors.Any(e => e.DonorID == id);
        }
    }
}
