using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SKNManager.Models.HomeViewModels;
using SKNManager.Data;
using SKNManager.Utils.Identity;
using Microsoft.EntityFrameworkCore;

namespace SKNManager.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public HomeController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        //
        // GET: /Home/Index
        public IActionResult Index()
        {
            IndexViewModel model = new IndexViewModel();

            try
            {
                model.ClubName = "Studenckie Koło Naukowe NEO";

                // -------------------------- USERS INFO ------------------------------
                var users = _dbContext.Users.Include(u => u.Claims);
                model.MembersCount = users.Where(u =>u.Claims.Any(c =>c.ClaimType == "ClubRank")).Count();

                if (users.Any(u => u.Claims.Any(c => c.ClaimValue == ClubRolesFactory.GetName(ClubRolesFactory.Role.PRESIDENT)))) {
                    var president = users.Where(u => u.Claims.Any(c => c.ClaimValue == ClubRolesFactory.GetName(ClubRolesFactory.Role.PRESIDENT))).First();
                    model.President = president.FirstName + " " + president.LastName;
                }

                if (users.Any(u => u.Claims.Any(c => c.ClaimValue == ClubRolesFactory.GetName(ClubRolesFactory.Role.VICE_PRESIDENT))))
                {
                    var vicepresident = users.Where(u => u.Claims.Any(c => c.ClaimValue == ClubRolesFactory.GetName(ClubRolesFactory.Role.VICE_PRESIDENT))).First();
                    model.VicePresident = vicepresident.FirstName + " " + vicepresident.LastName;
                }

                if (users.Any(u => u.Claims.Any(c => c.ClaimValue == ClubRolesFactory.GetName(ClubRolesFactory.Role.SECRETARY))))
                {
                    var secretary = users.Where(u => u.Claims.Any(c => c.ClaimValue == ClubRolesFactory.GetName(ClubRolesFactory.Role.SECRETARY))).First();
                    model.Secretary = secretary.FirstName + " " + secretary.LastName;
                }

                if (users.Any(u => u.Claims.Any(c => c.ClaimValue == ClubRolesFactory.GetName(ClubRolesFactory.Role.TREASURER))))
                {
                    var treasurer = users.Where(u => u.Claims.Any(c => c.ClaimValue == ClubRolesFactory.GetName(ClubRolesFactory.Role.TREASURER))).First();
                    model.VicePresident = treasurer.FirstName + " " + treasurer.LastName;
                }

                // -------------------------- STATISTICS ------------------------------
                var projects = _dbContext.Project;

                model.ProjectCount = projects.Count();
                model.ProjectInProgressCount = projects.Where(p => p.EndDate <= DateTime.Now).Count();

                model.EquipmentCount = _dbContext.Equipment.Count();
                model.EquipmentSetsCount = _dbContext.EquipmentSet.Count();
                //model.EquipmentLoansInProgressCount = 0;
                var upcomingDelegation = _dbContext.Delegation.OrderByDescending(d => d.StartDate).Where(d => d.StartDate >= (DateTime.Now.AddDays(-1))).First();
                if(upcomingDelegation != null)
                {
                    model.UpcomingEvent = upcomingDelegation.Name + " (";
                    if (DateTime.Compare(upcomingDelegation.StartDate, upcomingDelegation.EndDate) != 0)
                        model.UpcomingEvent += ("" + upcomingDelegation.StartDate.ToString("dd-MM-yyyy") + " - " + upcomingDelegation.EndDate.ToString("dd-MM-yyyy"));
                    else
                        model.UpcomingEvent += upcomingDelegation.StartDate.ToString("dd-MM-yyyy");
                    model.UpcomingEvent += ")";
                }

            }
            catch { }

            return View(model);
        }

        //
        // GET: /Home/Error
        public IActionResult Error()
        {
            return View();
        }
    }
}
