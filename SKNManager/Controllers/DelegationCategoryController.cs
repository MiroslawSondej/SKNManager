using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SKNManager.Data;
using SKNManager.Models;

namespace SKNManager.Controllers
{
    public class DelegationCategoryController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public DelegationCategoryController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        //
        // GET: DelegationCategory/Index
        public IActionResult Index()
        {
            try
            {
                DelegationCategory[] delegationCategory = _dbContext.DelegationCategory.ToArray();
                return View(delegationCategory);
            }
            catch { }

            return View("Error");
        }

        //
        // GET: DelegationCategory/Create
        public IActionResult Create()
        {
            return View();
        }

        //
        // POST: DelegationCategory/Create
        [HttpPost]
        public IActionResult Create(DelegationCategory model)
        {
            if (model == null)
            {
                return View("Error");
            }

            try
            {
                _dbContext.DelegationCategory.Add(new DelegationCategory() { Name = model.Name });
                _dbContext.SaveChanges();

                return RedirectToAction("Index");
            }
            catch { }
            return View("Error");
        }

        //
        // GET: DelegationCategory/Edit/5
        public IActionResult Edit(int id)
        {
            try
            {
                if (_dbContext.DelegationCategory.Any(d => d.Id == id))
                {
                    DelegationCategory delegationCategory = _dbContext.DelegationCategory.Where(d => d.Id == id).First();
                    return View(delegationCategory);
                }
            }
            catch { }

            return View("Error");
        }

        //
        // POST: DelegationCategory/Edit/5
        [HttpPost]
        public IActionResult Edit(int id, DelegationCategory model)
        {
            try
            {
                if(_dbContext.DelegationCategory.Any(d => d.Id == id))
                {
                    DelegationCategory delegationCategory = _dbContext.DelegationCategory.Where(d => d.Id == id).First();
                    delegationCategory.Name = model.Name;
                    _dbContext.DelegationCategory.Update(delegationCategory);
                    _dbContext.SaveChanges();

                    return RedirectToAction("Index");
                }
            }
            catch { }

            return View("Error");
        }

        //
        // GET: DelegationCategory/Delete/5
        public IActionResult Delete(int id)
        {
            try
            {
                if (_dbContext.DelegationCategory.Any(d => d.Id == id))
                {
                    DelegationCategory delegationCategory = _dbContext.DelegationCategory.Where(d => d.Id == id).First();
                    _dbContext.DelegationCategory.Remove(delegationCategory);
                    _dbContext.SaveChanges();

                    return RedirectToAction("Index");
                }
            }
            catch { }

            return View("Error");
        }
    }
}