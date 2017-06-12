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
    public class ProjectMembersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProjectMembersController(ApplicationDbContext context)
        {
            _context = context;    
        }

        //GET: ProjectMembers
        public IActionResult Index()
        {
            
            return View("Error");
        }

        // GET: ProjectMembers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var projectMembers = await _context.ProjectMembers
                .Include(p => p.ApplicationUser)
                .Include(p => p.Project)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (projectMembers == null)
            {
                return View("Error");
            }

            return View(projectMembers);
        }

        // GET: ProjectMembers/Create/5
        public IActionResult Create(int? id)
        {
            if (id != null)
            {
                ViewData["UserId"] = new SelectList(_context.Users, "Id", "FirstName" );
                ViewBag.UsersName = _context.Users.ToArray();
                ViewBag.ProjectName = _context.Project.Where(d => d.Id == id).First().Name;
                ViewData["ProjectId"] = id;
                ViewBag.UsersExist = 0;
                return View();
            }
            else
                return View("Error"); ;
        }

        // POST: ProjectMembers/Create/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>Create([Bind("Id,UserId,ProjectId")] ProjectMembers projectMembers)
        {
            if (ModelState.IsValid && !(_context.ProjectMembers.Any(d => d.ProjectId == projectMembers.ProjectId) && _context.ProjectMembers.Any(d => d.UserId.Equals(projectMembers.UserId))))
            {
                projectMembers.Id = 0;
                _context.Add(projectMembers);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Project", new { id = projectMembers.ProjectId });
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", projectMembers.UserId);
            ViewData["ProjectId"] = projectMembers.ProjectId;
            ViewBag.UsersName = _context.Users.ToArray();
            ViewBag.ProjectName = _context.Project.Where(d => d.Id == projectMembers.ProjectId).First().Name;
            ViewBag.UsersExist= 1;
            return View(projectMembers);
        }

        // GET: ProjectMembers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var projectMembers = await _context.ProjectMembers
                .Include(p => p.ApplicationUser)
                .Include(p => p.Project)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (projectMembers == null)
            {
                return View("Error");
            }

            return View(projectMembers);
        }

        // POST: ProjectMembers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var projectMembers = await _context.ProjectMembers.SingleOrDefaultAsync(m => m.Id == id);
            _context.ProjectMembers.Remove(projectMembers);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Project", new { id = projectMembers.ProjectId });
        }

        private bool ProjectMembersExists(int id)
        {
            return _context.ProjectMembers.Any(e => e.Id == id);
        }
    }
}
