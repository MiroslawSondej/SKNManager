using System.Linq;
using SKNManager.Models;
using SKNManager.Models.MemberViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SKNManager.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SKNManager.Controllers
{
    [Authorize]
    public class MemberController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILoggerFactory _loggerFactory;


        public MemberController(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, ILoggerFactory loggerFactory)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _loggerFactory = loggerFactory;
        }

        // GET: Member
        public async Task<ActionResult> Index()
        {
            List<Tuple<ApplicationUser, string>> userTuple = new List<Tuple<ApplicationUser, string>>();

            var query = (from u in _dbContext.Users select u).ToArray<ApplicationUser>();
            if (query != null)
            {
                foreach (ApplicationUser user in query)
                {
                    string userClubRole = "Brak";

                    IList<Claim> claim = await _userManager.GetClaimsAsync(user);
                    Claim[] userClaim = claim.Where(u => u.Type == "ClubRank").ToArray();
                    
                    if (userClaim != null && userClaim.Length > 0 && userClaim[0].Value.Length > 0)
                        userClubRole = userClaim[0].Value;

                    userTuple.Add(new Tuple<ApplicationUser, string>(user, userClubRole));
                }
            }
            return View(new IndexMemberViewModel { UserTuple = userTuple.ToArray() });
        }

        // GET: Member/Details/5
        public ActionResult Details(string id)
        {
            return View();
        }

        // GET: Member/Add
        public ActionResult Add()
        {
            return View();
        }

        // POST: Member/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Member/Edit/5
        public ActionResult Edit(string id)
        {
            return View();
        }

        // POST: Member/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Member/Delete/5
        public ActionResult Delete(string  id)
        {
            return View();
        }

        // POST: Member/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}