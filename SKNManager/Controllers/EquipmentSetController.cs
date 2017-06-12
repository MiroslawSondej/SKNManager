using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SKNManager.Data;
using SKNManager.Models.EquipmentViewModels;
using SKNManager.Models.EquipmentSetViewModels;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace SKNManager.Controllers
{
    [Authorize]
    public class EquipmentSetController : Controller
    {
        private readonly ApplicationDbContext _context;
        //private readonly ILoggerFactory _loggerFactory;

        public EquipmentSetController(ApplicationDbContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            //_loggerFactory = loggerFactory;
        }

        // GET: EquipmentSet
        public async Task<IActionResult> Index()
        {
            return View(await _context.EquipmentSet.ToListAsync());
        }


        // GET: EquipmentSet/Details/5
        public async Task<IActionResult> Details(int? id, Equipment equipment)
        {
            if (id == null)
            {
                return NotFound();
            }

            var equipmentSet = await _context.EquipmentSet.SingleOrDefaultAsync(m => m.Id == id);
            var eqSet = _context.Equipment.Where(e => e.EquipmentSetId == id).ToArray();
            if (equipmentSet == null)
            {
                return NotFound();
            }
            
            return View(new EquipmentSetDetailsViewModel() { EquipmentSet = equipmentSet, Equipment = eqSet });
        }

        // GET: EquipmentSet/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: EquipmentSet/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] EquipmentSet equipmentSet)
        {
            if (ModelState.IsValid)
            {
                _context.Add(equipmentSet);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(equipmentSet);
        }

        // GET: EquipmentSet/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var equipmentSet = await _context.EquipmentSet.SingleOrDefaultAsync(m => m.Id == id);
            if (equipmentSet == null)
            {
                return NotFound();
            }
            return View(equipmentSet);
        }

        // POST: EquipmentSet/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] EquipmentSet equipmentSet)
        {
            if (id != equipmentSet.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(equipmentSet);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EquipmentSetExists(equipmentSet.Id))
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
            return View(equipmentSet);
        }

        // GET: EquipmentSet/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var equipmentSet = await _context.EquipmentSet
                .SingleOrDefaultAsync(m => m.Id == id);
            if (equipmentSet == null)
            {
                return NotFound();
            }

            return View(equipmentSet);
        }

        // POST: EquipmentSet/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var equipmentSet = await _context.EquipmentSet.SingleOrDefaultAsync(m => m.Id == id);
            _context.EquipmentSet.Remove(equipmentSet);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool EquipmentSetExists(int id)
        {
            return _context.EquipmentSet.Any(e => e.Id == id);
        }
    }
}
