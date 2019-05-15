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
    public class NursesController : Controller
    {
        private readonly BBSContext _context;

        public NursesController(BBSContext context)
        {
            _context = context;
        }

        // GET: Nurses
        public async Task<IActionResult> Index()
        {
            var bBSContext = _context.Nurses.Include(n => n.Hospital);
            return View(await bBSContext.ToListAsync());
        }

        // GET: Nurses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nurse = await _context.Nurses
                .Include(n => n.Hospital)
                .Include(n=> n.BloodDonations)
                    .ThenInclude(b => b.Donor)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.NurseID == id);
            if (nurse == null)
            {
                return NotFound();
            }

            return View(nurse);
        }

        // GET: Nurses/Create
        public IActionResult Create()
        {
            ViewData["HospitalID"] = new SelectList(_context.Hospitals, "HospitalID", "Name");
            return View();
        }

        // POST: Nurses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind(",LastName,FirstName,PESEL,HireDate,HospitalID")] Nurse nurse)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(nurse);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                ViewData["HospitalID"] = new SelectList(_context.Hospitals, "HospitalID", "Name", nurse.HospitalID);
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }
            return View(nurse);
        }

        // GET: Nurses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nurse = await _context.Nurses.FindAsync(id);
            if (nurse == null)
            {
                return NotFound();
            }
            ViewData["HospitalID"] = new SelectList(_context.Hospitals, "HospitalID", "Name", nurse.HospitalID);
            return View(nurse);
        }

        // POST: Nurses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("LastName,FirstName,PESEL,HireDate,HospitalID")] Nurse nurse)
        //{
        //    if (id != nurse.NurseID)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(nurse);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!NurseExists(nurse.NurseID))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["HospitalID"] = new SelectList(_context.Hospitals, "HospitalID", "Address", nurse.HospitalID);
        //    return View(nurse);
        //}

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var nurseToUpdate = await _context.Nurses.FirstOrDefaultAsync(s => s.NurseID == id);
            if (await TryUpdateModelAsync<Nurse>(
                nurseToUpdate,
                "",
                s => s.FirstName, s => s.LastName, s=> s.HireDate, s => s.PESEL, s => s.HospitalID))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
            }
            return View(nurseToUpdate);
        }

        // GET: Nurses/Delete/5
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nurse = await _context.Nurses
                .AsNoTracking()
                .Include(n => n.Hospital)
                .FirstOrDefaultAsync(m => m.NurseID == id);
            if (nurse == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                    "Delete failed. Try again, and if the problem persists " +
                    "see your system administrator.";
            }

            return View(nurse);
        }

        // POST: Nurses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var nurse = await _context.Nurses.FindAsync(id);
            if (nurse == null)
            {
                return RedirectToAction(nameof(Index));
            }
            try
            {
                _context.Nurses.Remove(nurse);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }

        private bool NurseExists(int id)
        {
            return _context.Nurses.Any(e => e.NurseID == id);
        }
    }
}
