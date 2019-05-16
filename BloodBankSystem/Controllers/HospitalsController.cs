using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BloodBankSystem.Data;
using BloodBankSystem.Models;
using static BloodBankSystem.Models.BBSViewModels.HospitalDetails;
using BloodBankSystem.Models.BBSViewModels;

namespace BloodBankSystem.Controllers
{
    public class HospitalsController : Controller
    {
        private readonly BBSContext _context;

        public HospitalsController(BBSContext context)
        {
            _context = context;
        }

        // GET: Hospitals
        public async Task<IActionResult> Index(string searchString)
        {
            ViewData["currentFilter"] = "";
            var hospitals = from h in _context.Hospitals select h;
            if (!String.IsNullOrEmpty(searchString))
            {
                hospitals = hospitals.Where(h => h.Name.Contains(searchString));
                ViewData["currentFilter"] = searchString;
            }
                return View(await hospitals.OrderBy(h => h.Name).AsNoTracking().ToListAsync());
        }

        // GET: Hospitals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            

            var hospital = await _context.Hospitals
                .Include(h => h.Nurses)
                .AsNoTracking() 
                .FirstOrDefaultAsync(m => m.HospitalID == id);
            if (hospital == null)
            {
                return NotFound();
            }

            var bloodQ =await _context.BloodDonations
                            .Include(b => b.Nurse)
                            .Include(b => b.Donor)
                            .Where(b => b.Nurse.HospitalID == id)
                            .GroupBy(b => b.Donor.BloodType)
                            .Select(g => new BL
                            {
                                Type = g.Key,
                                Amount = g.Sum(o => o.Volume)
                            }).ToArrayAsync();

            

            

            return View(new HospitalDetails { Hospital = hospital, BloodLevels = bloodQ});
        }

        // GET: Hospitals/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Hospitals/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HospitalID,Name,Address")] Hospital hospital)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hospital);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(hospital);
        }

        // GET: Hospitals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hospital = await _context.Hospitals.FindAsync(id);
            if (hospital == null)
            {
                return NotFound();
            }
            return View(hospital);
        }

        // POST: Hospitals/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("HospitalID,Name,Address")] Hospital hospital)
        {
            if (id != hospital.HospitalID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hospital);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HospitalExists(hospital.HospitalID))
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
            return View(hospital);
        }

        // GET: Hospitals/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hospital = await _context.Hospitals
                .FirstOrDefaultAsync(m => m.HospitalID == id);
            if (hospital == null)
            {
                return NotFound();
            }

            return View(hospital);
        }

        // POST: Hospitals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hospital = await _context.Hospitals.FindAsync(id);
            _context.Hospitals.Remove(hospital);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HospitalExists(int id)
        {
            return _context.Hospitals.Any(e => e.HospitalID == id);
        }
    }
}
