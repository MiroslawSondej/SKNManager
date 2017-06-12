using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SKNManager.Models;
using SKNManager.Models.ManageViewModels;
using SKNManager.Services;
using System.IO;
using SKNManager.Data;

namespace SKNManager.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly string _externalCookieScheme;
        private readonly IEmailSender _emailSender;
        private readonly ISmsSender _smsSender;
        private readonly ILogger _logger;
        private readonly ApplicationDbContext _dbContext;

        public ManageController(
          UserManager<ApplicationUser> userManager,
          SignInManager<ApplicationUser> signInManager,
          IOptions<IdentityCookieOptions> identityCookieOptions,
          IEmailSender emailSender,
          ISmsSender smsSender,
          ILoggerFactory loggerFactory,
          ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _externalCookieScheme = identityCookieOptions.Value.ExternalCookieAuthenticationScheme;
            _emailSender = emailSender;
            _smsSender = smsSender;
            _logger = loggerFactory.CreateLogger<ManageController>();
            _dbContext = dbContext;
        }

        //
        // GET: /Manage/Index
        [HttpGet]
        public async Task<IActionResult> Index(ManageMessageId? message = null)
        {
            ViewData["StatusMessage"] =
                message == ManageMessageId.ChangePasswordSuccess ? "Twoje hasło zostało zmienione."
                : message == ManageMessageId.EmailChangeSuccess ? "Twój adres email został zmieniony."
                : message == ManageMessageId.EditProfileSuccess ? "Dane personalne zostały zaaktualizowane"
                : message == ManageMessageId.Error ? "Wystąpił błąd. Spróbuj ponownie później."
                : "";

            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return View("Error");
            }
            var model = new ManageIndexViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = await _userManager.GetPhoneNumberAsync(user),
                Email = user.Email,
            };
            return View(model);
        }

        //
        // GET: /Manage/EditProfile
        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                return View(new EditProfileViewModel() { FirstName = user.FirstName, LastName = user.LastName, PhoneNumber = user.PhoneNumber });
            }
            return View("Error");
        }

        //
        // GET: /Manage/EditProfile
        [HttpPost]
        public async Task<IActionResult> EditProfile(EditProfileViewModel model)
        {
            if(ModelState.IsValid)
            {
                var user = await GetCurrentUserAsync();
                if (user != null)
                {
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.PhoneNumber = model.PhoneNumber;

                    _dbContext.Users.Update(user);
                    _dbContext.SaveChanges();

                    return RedirectToAction("Index", new { message = ManageMessageId.EditProfileSuccess });
                }
            }
            return View(model);
        }

        //
        // GET: /Manage/ChangeEmail
        [HttpGet]
        public IActionResult ChangeEmail()
        {
            return View();
        }

        // POST: /Manage/ChangeEmail
        [HttpPost]
        public async Task<IActionResult> ChangeEmail(ChangeEmailViewModel model)
        {
            if(ModelState.IsValid)
            {
                var user = await GetCurrentUserAsync();
                if (user == null)
                {
                    return View("Error");
                }

                // Generate email change token
                string codeKey = await _userManager.GenerateChangeEmailTokenAsync(user, model.Email);

                string callbackUrl = Url.Action(nameof(VerifyEmailChange), "Manage", new { userId = user.Id, code = codeKey, newEmail = model.Email }, protocol: HttpContext.Request.Scheme);

                // Prepare email message from file template
                #region Message
                string message = "";
                using (FileStream fileStream = new FileStream("EmailContents/ConfirmationEmailChange.cshtml", FileMode.Open))
                {
                    StreamReader reader = new StreamReader(fileStream);
                    message = reader.ReadToEnd();
                }
                message = message.Replace("{Url}", callbackUrl);
                #endregion

                // Finally, send email
                await _emailSender.SendEmailAsync(model.Email, "Weryfikacja nowego adresu email. SKNManager - PWSZ Krosno", message);

                // Logout user
                await _signInManager.SignOutAsync();
                return View("EmailChanged");
            }

            return View();
        }

        //
        // GET: /Manage/VerifyEmailChange
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyEmailChange(string userId, string code, string newEmail)
        {
            if(!String.IsNullOrEmpty(userId) && !String.IsNullOrEmpty(code) && !String.IsNullOrEmpty(newEmail))
            {
                if(await _userManager.FindByIdAsync(userId) != null)
                {
                    ViewData["UserId"] = userId;
                    ViewData["Code"] = code;
                    ViewData["NewEmail"] = newEmail;
                    return View();
                }
            }
            return View("Error");
        }

        //
        // POST: /Manage/VerifyEmailChange
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyEmailChange(VerifyChangeEmailViewModel model)
        {
            if (!String.IsNullOrEmpty(model.UserId) && !String.IsNullOrEmpty(model.Code) && model != null)
            {
                ApplicationUser user = await _userManager.FindByIdAsync(model.UserId);
                if (user != null)
                {
                    if (ModelState.IsValid)
                    {
                        if (!await _userManager.CheckPasswordAsync(user, model.Password))
                        {
                            ModelState.AddModelError("PasswordVerification", "Podane hasło jest nieprawidłowe!");
                            ViewData["UserId"] = model.UserId;
                            ViewData["Code"] = model.Code;
                            ViewData["NewEmail"] = model.Email;
                            return View();
                        }
                        else
                        {
                            IdentityResult result = await _userManager.ChangeEmailAsync(user, model.Email, model.Code);
                            if (result.Succeeded)
                            {
                                await _signInManager.SignInAsync(user, false);
                                return RedirectToAction("Index", new { message = ManageMessageId.EmailChangeSuccess });
                            }
                            else
                            {
                                AddErrors(result);
                                ViewData["UserId"] = model.UserId;
                                ViewData["Code"] = model.Code;
                                ViewData["NewEmail"] = model.Email;
                                return View();
                            }
                        }
                    }
                    else
                    {
                        ViewData["UserId"] = model.UserId;
                        ViewData["Code"] = model.Code;
                        ViewData["NewEmail"] = model.Email;
                        return View();
                    }
                }
            }

            return View("Error");
        }

        //
        // GET: /Manage/ChangePassword
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction(nameof(Index), new { Message = ManageMessageId.ChangePasswordSuccess });
                }
                AddErrors(result);
                return View(model);
            }
            return RedirectToAction(nameof(Index), new { Message = ManageMessageId.Error });
        }

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            EditProfileSuccess,
            EmailChangeSuccess,
            Error
        }

        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }

        #endregion
    }
}
