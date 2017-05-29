using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using SKNManager.Models;
using SKNManager.Models.AccountViewModels;
using SKNManager.Services;
using SKNManager.Data;
using Microsoft.AspNetCore.Builder;
using System.IO;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace SKNManager.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ISmsSender _smsSender;
        private readonly ILogger _logger;
        private readonly string _externalCookieScheme;
        private readonly ApplicationDbContext _dbContext;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<ApplicationUser> signInManager,
            IOptions<IdentityCookieOptions> identityCookieOptions,
            EmailTokenProvider<ApplicationUser> tokenProvider,
            IEmailSender emailSender,
            ISmsSender smsSender,
            ILoggerFactory loggerFactory,
            ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _externalCookieScheme = identityCookieOptions.Value.ExternalCookieAuthenticationScheme;
            _emailSender = emailSender;
            _smsSender = smsSender;
            _logger = loggerFactory.CreateLogger<AccountController>();
            _dbContext = dbContext;
        }

        //
        // GET: /Account/Login
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.Authentication.SignOutAsync(_externalCookieScheme);

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation(1, "Zalogowano.");
                    return RedirectToLocal(returnUrl);
                }
                /*if (result.RequiresTwoFactor)
                {
                    return RedirectToAction(nameof(SendCode), new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                }*/
                if (result.IsLockedOut)
                {
                    _logger.LogWarning(2, "Konto użytkownika zablokowane.");
                    return View("Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Nieudana próba logowania.");
                    return View(model);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation(4, "Wylogowano.");
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        //
        // GET: /Account/ForgotPassword
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=532713
                // Send an email with this link
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.Action(nameof(ResetPassword), "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
                await _emailSender.SendEmailAsync(model.Email, "Resetowanie hasła",
                   $"Aby zresetować hasło do konta w systemie SKNManager kliknij: <a href='{callbackUrl}'>tutaj</a>");
                return View("ForgotPasswordConfirmation");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string code = null)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction(nameof(AccountController.ResetPasswordConfirmation), "Account");
            }
            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(AccountController.ResetPasswordConfirmation), "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        /*[HttpGet]                                         // FOR TEST ONLY
        public async Task<IActionResult> AddRole()
        {
            var user = await _userManager.FindByEmailAsync("test@test.pl");
            if (user == null)
            {
                return View("Error");
            }
            await _roleManager.CreateAsync(new IdentityRole("Administrator"));
            await _userManager.AddToRoleAsync(user, "Administrator");
            return null;
        }*/

        //
        // GET: /Account/SendCode
        /*[HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl = null, bool rememberMe = false)
        {
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                return View("Error");
            }
            var userFactors = await _userManager.GetValidTwoFactorProvidersAsync(user);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }*/

        //
        // POST: /Account/SendCode
        /*[HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                return View("Error");
            }

            // Generate the token and send it
            var code = await _userManager.GenerateTwoFactorTokenAsync(user, model.SelectedProvider);
            if (string.IsNullOrWhiteSpace(code))
            {
                return View("Error");
            }

            var message = "Twój kod to: " + code;
            if (model.SelectedProvider == "Email")
            {
                await _emailSender.SendEmailAsync(await _userManager.GetEmailAsync(user), "Security Code", message);
            }
            else if (model.SelectedProvider == "Phone")
            {
                await _smsSender.SendSmsAsync(await _userManager.GetPhoneNumberAsync(user), message);
            }

            return RedirectToAction(nameof(VerifyCode), new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }*/

        //
        // GET: /Account/VerifyCode
        /*[HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyCode(string provider, bool rememberMe, string returnUrl = null)
        {
            // Require that the user has already logged in via username/password or external login
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }*/

        //
        // POST: /Account/VerifyCode
        /*[HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes.
            // If a user enters incorrect codes for a specified amount of time then the user account
            // will be locked out for a specified amount of time.
            var result = await _signInManager.TwoFactorSignInAsync(model.Provider, model.Code, model.RememberMe, model.RememberBrowser);
            if (result.Succeeded)
            {
                return RedirectToLocal(model.ReturnUrl);
            }
            if (result.IsLockedOut)
            {
                _logger.LogWarning(7, "Konto użytkownika zablokowane.");
                return View("Lockout");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Kod nieprawidłowy.");
                return View(model);
            }
        }*/

        //
        // POST /Account/Invite
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Invite(InviteViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                // Create new user object
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email }; 

                // Attempt to put user into database
                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    // Generate "Email confirmation" token and URL
                    var codeKey = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Action(nameof(Register), "Account", new { userId = user.Id, code = codeKey }, protocol: HttpContext.Request.Scheme);

                    // Get data of user who invites
                    ApplicationUser inviter = await _userManager.GetUserAsync(HttpContext.User);
                    
                    // Prepare email message from file template
                    #region Message
                    string message = "";
                    using (FileStream fileStream = new FileStream("EmailContents/InvitationEmail.cshtml", FileMode.Open)) {
                        StreamReader reader = new StreamReader(fileStream);
                        message = reader.ReadToEnd();
                    }
                    message = message.Replace("{Url}", callbackUrl);
                    message = message.Replace("{UserName}", inviter.FirstName + " " + inviter.LastName);
                    #endregion

                    // Finally, send email
                    await _emailSender.SendEmailAsync(model.Email, "Zaproszenie do systemu SKNManager - PWSZ Krosno", message);
                    return RedirectToLocal(returnUrl);
                }

                // We don't want to mess with Identity in Memeber/Add so we return errors as string's array
                AddErrors(result);
                ViewData["Message"] = "Następujące błędy wystąpiły podczas generowania zaproszenia:";
                return View("AddInviteUserError");
            }
            // If we got this far, something failed
            return RedirectToAction("Add", "Member");
        }

        // GET: /Account/Register
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Register(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View("Error");
            }

            if(user.EmailConfirmed)
            {
                ViewData["ErrorID"] = "UserAlreadyActivated";
                return View("RegistrationFailed");
            }

            return View(new RegisterViewModel { Email = user.Email, UserId = userId, Code = code }); // Inject prefilled model to view
        }

        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.UserId == null || model.Code == null)
                {
                    return View("Error");
                }

                var user = await _userManager.FindByIdAsync(model.UserId);
                if(user == null)
                {
                    return View("Error");
                }

                // Checking if account isn't already registered
                if (user.EmailConfirmed) 
                {
                    ViewData["ErrorID"] = "UserAlreadyActivated";
                    return View("RegistrationFailed");
                }

                // Verifying user token
                bool tokenVerificationResult = await _userManager.VerifyUserTokenAsync(user, TokenOptions.DefaultProvider, "EmailConfirmation", model.Code);
                if(!tokenVerificationResult)
                {
                    ViewData["ErrorID"] = "IncorrectToken";
                    return View("RegistrationFailed");
                }

                // setting password
                var result = await _userManager.AddPasswordAsync(user, model.Password);
                if (result.Succeeded) // Checking if password in accordance with password policy
                {
                    user.EmailConfirmed = true;
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;

                    await _userManager.AddClaimAsync(user, new Claim("ClubRank", "Członek"));

                    _dbContext.Users.Update(user); // Update db's context
                    _dbContext.SaveChanges(); // Save changes to db
                    _logger.LogInformation(3, "User created and activated."); 
                    return View("RegistrationCompleted");
                }
                else if(result.Errors.Where((e) => e.Code == "UserAlreadyHasPassword").Any()) // On "AddPassword" failure, check if user already have password
                {
                    ViewData["ErrorID"] = "UserAlreadyActivated";
                    return View("RegistrationFailed"); // If we got here, there was sth wrong with previous registration attempt
                }
                AddErrors(result); // Add "AddPassword" errors descriptions to view (example: "Password must have at least 8 characters")
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET /Account/AccessDenied
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }
        #endregion
    }
}
