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

using SKNManager.Utils.Identity;
using Microsoft.EntityFrameworkCore;

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

        // GET: Member/Add
        [Authorize(Policy = "SecretaryClubRank")]
        public ActionResult Add()
        {
            return View();
        }

        // GET: Member/Details/5
        [Authorize(Policy = "SecretaryClubRank")]
        //[Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Details(string id)
        {
            if (id == null || id.Length <= 0)
            {
                return View("Error");
            }

            ApplicationUser user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return View("Error");
            }


            #region GetUserClubRank
            string userClubRole = "Brak";

            IList<Claim> claim = await _userManager.GetClaimsAsync(user);
            Claim[] userClaim = claim.Where(u => u.Type == "ClubRank").ToArray();

            if (userClaim != null && userClaim.Length > 0 && userClaim[0].Value.Length > 0)
                userClubRole = userClaim[0].Value;
            #endregion

            var delegations = new Delegation[] { };
            try
            {
                delegations = _dbContext.Delegation.Include(d => d.Category).Include(d => d.Member).ThenInclude(d => d.User).Where(d => d.Member.Any(m => m.User.Id == id)).ToArray();
            } catch { }

            var projects = new Project[] { };
            try
            {
                projects = _dbContext.Project.Include(p => p.ProjectMembers).ThenInclude(d => d.ApplicationUser).Where(d => d.ProjectMembers.Any(m => m.ApplicationUser.Id == id) || d.ApplicationUser.Id == id).ToArray();
            }
            catch { }

            return View(new DetailsViewModel() { User = user, ClubRankName = userClubRole, Delegation = delegations, Project = projects });
        }

        // GET: Member/Edit/5
        [Authorize(Policy = "VicePresidentClubRank")]
        public async Task<ActionResult> Edit(string id)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return View("Error");

            ViewBag.Id = id;
            ViewBag.FirstName = user.FirstName;
            ViewBag.LastName = user.LastName;
            ViewBag.Email = user.Email;

            #region GetUserClubRank
            string userClubRole = "Brak";

            IList<Claim> claim = await _userManager.GetClaimsAsync(user);
            Claim[] userClaim = claim.Where(u => u.Type == "ClubRank").ToArray();

            if (userClaim != null && userClaim.Length > 0 && userClaim[0].Value.Length > 0)
                userClubRole = userClaim[0].Value;
            #endregion

            ViewBag.UserRank = userClubRole;
            ViewBag.ClubRoles = ClubRolesFactory.GetAll();

            return View(new EditMemberViewModel() { Id = user.Id });
        }

        // POST: Member/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "VicePresidentClubRank")]
        public async Task<ActionResult> Edit(string id, EditMemberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Error");
            }

            // TODO: add security (user with lower rank cannot change user with higher rank)

            string clubRankName = ClubRolesFactory.GetName(ClubRolesFactory.GetId(model.ClubRank)); // verify if rank from form is "real"
            ApplicationUser user = await _userManager.FindByIdAsync(id);

            #region GetUserClubRank

            IList<Claim> claim = await _userManager.GetClaimsAsync(user);
            Claim[] userClaim = claim.Where(u => u.Type == "ClubRank").ToArray();


            if (userClaim != null && userClaim.Length > 0 && userClaim[0].Value.Length > 0)
                await _userManager.ReplaceClaimAsync(user, userClaim[0], new Claim("ClubRank", clubRankName));
            else
                await _userManager.AddClaimAsync(user, new Claim("ClubRank", clubRankName));
            #endregion
            return RedirectToAction("Index");
        }

        // GET: Member/Delete/5
        [Authorize(Policy = "VicePresidentClubRank")]
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null || id.Length <= 0)
            {
                return View("Error");
            }

            try
            {
                ApplicationUser user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    return View("Error");
                }

                ViewBag.FirstName = user.FirstName;
                ViewBag.LastName = user.LastName;
                ViewBag.Email = user.Email;

                return View();
            }
            catch { }

            return View("Error");
        }

        // POST: Member/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "VicePresidentClubRank")]
        public async Task<ActionResult> Delete(string id, IFormCollection collection)
        {
            if (id == null || id.Length <= 0)
            {
                return View("Error");
            }

            try
            {
                ApplicationUser user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    return View("Error");
                }

                bool canDelete = false; // stores information, if there is any user able to add new users except the one, that will be delete (prevents situation, when there is no user able to add or invite a new one)
                foreach (ApplicationUser u in _userManager.Users)
                {
                    if (u.Id == user.Id)
                    {
                        continue;
                    }
                    else if (await _userManager.IsInRoleAsync(user, "Administrator"))
                    {
                        canDelete = true;
                        break;
                    }

                    var claim = await _userManager.GetClaimsAsync(u);
                    if (claim.Where(c => c.Type == "ClubRank" && c.Value == ClubRolesFactory.GetName(ClubRolesFactory.Role.PRESIDENT)).Count<Claim>() > 0)
                    {
                        canDelete = true;
                        break;
                    }

                }

                if (!canDelete)
                {
                    return View("Error");
                }

                await _userManager.DeleteAsync(user);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}