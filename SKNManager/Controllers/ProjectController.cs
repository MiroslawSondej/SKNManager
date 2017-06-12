using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SKNManager.Data;
using SKNManager.Models;
using Microsoft.AspNetCore.Authorization;

namespace SKNManager.Controllers
{
    [Authorize]
    public class ProjectController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProjectController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Project
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Project.Include(p => p.ApplicationUser);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Project/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null || id == 0)
            {
                return View("Error");
            }

            var project = _context.Project.Where(d => d.Id == id).Include(p => p.ApplicationUser).Include(p => p.ProjectMembers).ThenInclude(p => p.ApplicationUser).First();
            if (project == null)
            {
                return View("Error");
            }

            return View(project);
        }

        // GET: Project/Create
        public IActionResult Create()
        {
            ViewData["ProjectLeaderId"] = new SelectList(_context.Users, "Id", "Id");
            ViewBag.ProjectLeader = _context.Users.ToArray();
            return View();
        }

        // POST: Project/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,StartDate,EndDate,ProjectLeaderId")] Project project)
        {
            if (ModelState.IsValid)
            {
                _context.Add(project);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["ProjectLeaderId"] = new SelectList(_context.Users, "Id", "Id", project.ProjectLeaderId);
            ViewBag.ProjectLeader = _context.Users.ToArray();
            return View(project);
        }

        // GET: Project/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var project = await _context.Project.SingleOrDefaultAsync(m => m.Id == id);
            ViewBag.ProjectLeader = _context.Users.ToArray();
            if (project == null)
            {
                return View("Error");
            }
            ViewData["ProjectLeaderId"] = new SelectList(_context.Users, "Id", "Id", project.ProjectLeaderId);
            return View(project);
        }

        // POST: Project/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,StartDate,EndDate,ProjectLeaderId")] Project project)
        {
            if (id != project.Id)
            {
                return View("Error");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(project);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(project.Id))
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
            ViewData["ProjectLeaderId"] = new SelectList(_context.Users, "Id", "Id", project.ProjectLeaderId);
            ViewBag.ProjectLeader = _context.Users.ToArray();
            return View(project);
        }

        // GET: Project/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var project = await _context.Project
                .Include(p => p.ApplicationUser)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (project == null)
            {
                return View("Error");
            }

            return View(project);
        }

        // POST: Project/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var project = await _context.Project.SingleOrDefaultAsync(m => m.Id == id);
            _context.Project.Remove(project);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool ProjectExists(int id)
        {
            return _context.Project.Any(e => e.Id == id);
        }
    }
}
