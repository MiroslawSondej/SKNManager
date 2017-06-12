using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SKNManager.Data;
using SKNManager.Models;
using Microsoft.EntityFrameworkCore;
using SKNManager.Models.DelegationMemberViewModel;

namespace SKNManager.Controllers
{
    public class DelegationMemberController : Controller
    {

        private readonly ApplicationDbContext _dbContext;

        public DelegationMemberController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        //
        // GET: DelegationMember/Index/5
        public IActionResult Index(int id)
        {
            try
            {
                if (_dbContext.Delegation.Any(d => d.Id == id))
                {
                    string delegationName = _dbContext.Delegation.Where(d => d.Id == id).First().Name;
                    List<ApplicationUser> user = _dbContext.Users.ToList();
                    DelegationMember[] delegationMember = { };
                    List<ApplicationUser> nonparticipant = new List<ApplicationUser>();

                    if (_dbContext.DelegationMember.Where(d => d.DelegationId == id).Any())
                    {
                        delegationMember = _dbContext.DelegationMember.Where(d => d.DelegationId == id).Include(d => d.User).ToArray();
                    }

                    foreach (ApplicationUser u in user)
                    {
                        if (!delegationMember.Any(d => d.UserId == u.Id))
                        {
                            nonparticipant.Add(u);
                        }
                    }

                    return View(new DelegationMemberIndexViewModel() { DelegationId = id, DelegationName = delegationName, Member = delegationMember, User = nonparticipant.ToArray() });
                }
            }
            catch
            {
                return View("Error");
            }

            return View("Error");
        }

        //
        // GET: DelegationMember/Add/5/5
        public IActionResult Add(int delegationId, string userId)
        {
            try
            {
                if (_dbContext.Delegation.Any(d => d.Id == delegationId) && _dbContext.Users.Any(d => d.Id == userId))
                {
                    if (!_dbContext.DelegationMember.Any(d => d.DelegationId == delegationId && d.UserId == userId))
                    {
                        _dbContext.DelegationMember.Add(new DelegationMember() { DelegationId = delegationId, UserId = userId });
                        _dbContext.SaveChanges();

                    }
                    return RedirectToAction("Index", new { id = delegationId });
                }
            }
            catch
            {
                return View("Error");
            }
            return View("Error");
        }

        //
        // GET: DelegationMember/Delete/5/5
        public IActionResult Delete(int delegationId, int memberId)
        {
            try
            {
                if (_dbContext.DelegationMember.Any(d => d.Id == memberId))
                {
                    DelegationMember member = _dbContext.DelegationMember.Where(d => d.Id == memberId).First();
                    _dbContext.DelegationMember.Remove(member);
                    _dbContext.SaveChanges();
                    return RedirectToAction("Index", new { id = delegationId });
                }
            }
            catch
            {
                return View("Error");
            }
            return View("Error");
        }
    }
}