using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SKNManager.Data;
using SKNManager.Models;

namespace SKNManager.Controllers
{
    public class EquipmentLoansController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EquipmentLoansController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: EquipmentLoans
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.EquipmentLoans.Include(e => e.ApplicationUser).Include(e => e.Equipment).Include(e => e.EquipmentSet);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: EquipmentLoans/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var equipmentLoans = await _context.EquipmentLoans
                .Include(e => e.ApplicationUser)
                .Include(e => e.Equipment)
                .Include(e => e.EquipmentSet)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (equipmentLoans == null)
            {
                return NotFound();
            }

            return View(equipmentLoans);
        }

        // GET: EquipmentLoans/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["EquipmentId"] = new SelectList(_context.Equipment, "Id", "Name");
            ViewData["EquipmentSetId"] = new SelectList(_context.EquipmentSet, "Id", "Name");
            return View();
        }

        // POST: EquipmentLoans/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,EquipmentId,Amount,EquipmentSetId,LoanDate,ReturnDate,Comments")] EquipmentLoans equipmentLoans)
        {
            if (ModelState.IsValid)
            {
                _context.Add(equipmentLoans);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", equipmentLoans.UserId);
            ViewData["EquipmentId"] = new SelectList(_context.Equipment, "Id", "Name", equipmentLoans.EquipmentId);
            ViewData["EquipmentSetId"] = new SelectList(_context.EquipmentSet, "Id", "Name", equipmentLoans.EquipmentSetId);
            return View(equipmentLoans);
        }

        // GET: EquipmentLoans/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var equipmentLoans = await _context.EquipmentLoans.SingleOrDefaultAsync(m => m.Id == id);
            if (equipmentLoans == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", equipmentLoans.UserId);
            ViewData["EquipmentId"] = new SelectList(_context.Equipment, "Id", "Name", equipmentLoans.EquipmentId);
            ViewData["EquipmentSetId"] = new SelectList(_context.EquipmentSet, "Id", "Name", equipmentLoans.EquipmentSetId);
            return View(equipmentLoans);
        }

        // POST: EquipmentLoans/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,EquipmentId,Amount,EquipmentSetId,LoanDate,ReturnDate,Comments")] EquipmentLoans equipmentLoans)
        {
            if (id != equipmentLoans.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(equipmentLoans);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EquipmentLoansExists(equipmentLoans.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", equipmentLoans.UserId);
            ViewData["EquipmentId"] = new SelectList(_context.Equipment, "Id", "Name", equipmentLoans.EquipmentId);
            ViewData["EquipmentSetId"] = new SelectList(_context.EquipmentSet, "Id", "Name", equipmentLoans.EquipmentSetId);
            return View(equipmentLoans);
        }

        // GET: EquipmentLoans/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var equipmentLoans = await _context.EquipmentLoans
                .Include(e => e.ApplicationUser)
                .Include(e => e.Equipment)
                .Include(e => e.EquipmentSet)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (equipmentLoans == null)
            {
                return NotFound();
            }

            return View(equipmentLoans);
        }

        // POST: EquipmentLoans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var equipmentLoans = await _context.EquipmentLoans.SingleOrDefaultAsync(m => m.Id == id);
            _context.EquipmentLoans.Remove(equipmentLoans);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool EquipmentLoansExists(int id)
        {
            return _context.EquipmentLoans.Any(e => e.Id == id);
        }
    }
}
