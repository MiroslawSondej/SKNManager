using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SKNManager.Data;
using SKNManager.Models;
using Microsoft.EntityFrameworkCore;

namespace SKNManager.Controllers
{
    public class DelegationController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public DelegationController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: Delegation/Index
        public IActionResult Index()
        {
            Delegation[] delegation = _dbContext.Delegation.Include(d => d.Category).Include(d => d.Member).ThenInclude(d => d.User).ToArray();
            delegation = delegation.OrderByDescending(d => d.StartDate).ToArray();

            return View(delegation);
        }

        // GET: Delegation/Details/5
        public IActionResult Details(int id)
        {
            Delegation delegation;

            try
            {
                delegation = _dbContext.Delegation.Where(d => d.Id == id).Include(d => d.Category).Include(d => d.Member).ThenInclude(d => d.User).First();
                return View(delegation);
            }
            catch
            {
                return View("Error");
            }
        }

        // GET: Delegation/Edit/5
        public IActionResult Edit(int id)
        {
            Delegation delegation;

            try
            {
                delegation = _dbContext.Delegation.Where(d => d.Id == id).Include(d => d.Category).Include(d => d.Member).ThenInclude(d => d.User).First();
                ViewBag.Category = _dbContext.DelegationCategory.ToArray();
                return View(delegation);
            }
            catch
            {
                return View("Error");
            }
        }

        // POST: Delegation/Edit/5
        [HttpPost]
        public IActionResult Edit(int id, Delegation model)
        {
            Delegation delegation;

            try
            {
                delegation = _dbContext.Delegation.Where(d => d.Id == id).Include(d => d.Category).Include(d => d.Member).ThenInclude(d => d.User).First();

                if (ModelState.IsValid)
                {
                    delegation.Name = model.Name;
                    delegation.StartDate = model.StartDate;
                    delegation.EndDate = model.EndDate;
                    delegation.CategoryId = model.CategoryId;

                    _dbContext.Delegation.Update(delegation);
                    _dbContext.SaveChanges();

                    return RedirectToAction("Index");
                }
                else
                {
                    foreach(var a in ModelState.ToArray())
                    {
                        if(a.Value.Errors.ToArray().Length > 0)
                        {

                        }
                    }
                    ViewBag.Category = _dbContext.DelegationCategory.ToArray();
                    return View(delegation);
                }
            }
            catch
            {
                return View("Error");
            }
        }

        // GET: Delegation/Delete/5
        public IActionResult Delete(int id)
        {
            try
            {
                Delegation delegation = _dbContext.Delegation.Where(d => d.Id == id).Include(d => d.Member).First();
                _dbContext.Delegation.Remove(delegation);
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View("Error");
            }
        }
    }
}