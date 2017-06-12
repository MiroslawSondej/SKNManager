using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SKNManager.Data;
using SKNManager.Models.EquipmentViewModels;
using Microsoft.AspNetCore.Authorization;

namespace SKNManager.Controllers
{
    [Authorize]
    public class EquipmentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EquipmentController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Equipment
        public async Task<IActionResult> Index(string findEquipent)
        {
            if (string.IsNullOrEmpty(findEquipent))
            {
                var applicationDbContext = _context.Equipment.Include(e => e.EquipmentSet).OrderBy(e => e.Name);
                return View(await applicationDbContext.ToListAsync());
            }else
            {
                var applicationDbContext = _context.Equipment.Include(e => e.EquipmentSet).Where(e => e.Name.Contains(findEquipent)).OrderBy(e => e.Name);
                return View(await applicationDbContext.ToListAsync());
            }
        }

        // GET: Equipment/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var equipment = await _context.Equipment
                .Include(e => e.EquipmentSet)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (equipment == null)
            {
                return View("Error");
            }

            return View(equipment);
        }

        //[Authorize(Roles =)]
        // GET: Equipment/Create
        public IActionResult Create()
        {
            ViewData["EquipmentSetId"] = new SelectList(_context.EquipmentSet, "Id", "Name");
            return View();
        }

        // POST: Equipment/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Amount,EquipmentSetId")] Equipment equipment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(equipment);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["EquipmentSetId"] = new SelectList(_context.EquipmentSet, "Id", "Name", equipment.EquipmentSetId);
            return View(equipment);
        }

        // GET: Equipment/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
               return View("Error");
            }

            var equipment = await _context.Equipment.SingleOrDefaultAsync(m => m.Id == id);
            if (equipment == null)
            {
                return View("Error");
            }
            ViewData["EquipmentSetId"] = new SelectList(_context.EquipmentSet, "Id", "Name", equipment.EquipmentSetId);
            return View(equipment);
        }

        // POST: Equipment/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Amount,EquipmentSetId")] Equipment equipment)
        {
            if (id != equipment.Id)
            {
                return View("Error");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(equipment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EquipmentExists(equipment.Id))
                    {
                        return View("Error");
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            ViewData["EquipmentSetId"] = new SelectList(_context.EquipmentSet, "Id", "Name", equipment.EquipmentSetId);
            return View(equipment);
        }

        // GET: Equipment/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var equipment = await _context.Equipment
                .Include(e => e.EquipmentSet)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (equipment == null)
            {
                return View("Error");
            }

            return View(equipment);
        }

        // POST: Equipment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var equipment = await _context.Equipment.SingleOrDefaultAsync(m => m.Id == id);
            _context.Equipment.Remove(equipment);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool EquipmentExists(int id)
        {
            return _context.Equipment.Any(e => e.Id == id);
        }
    }
}
