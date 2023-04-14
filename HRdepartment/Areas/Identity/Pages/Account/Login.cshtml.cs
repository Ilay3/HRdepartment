using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using HRdepartment.Models;
using System.Net.Http;
using Newtonsoft.Json;
using HRdepartment.Data;

namespace HRdepartment.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        public ApplicationDbContext db;

        public LoginModel(SignInManager<ApplicationUser> signInManager,
            ILogger<LoginModel> logger,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            db = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Введите Email")]
            [EmailAddress]
            public string Email { get; set; }

            [Required(ErrorMessage = "Введите пароль")]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            //await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    var response = await client.PostAsync("https://www.google.com/recaptcha/api/siteverify", new FormUrlEncodedContent(new Dictionary<string, string>
                    {
                        { "secret", "6Ld83FslAAAAAHcY0WG_e-9YMC59ZcvTcAFiLmTz" },
                        { "response", Request.Form["g-recaptcha-response"] }
                    }));

                    var json = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<RecaptchaResponse>(json);

                    if (!result.Success)
                    {
                        ModelState.AddModelError(string.Empty, "Ошибка.");
                        return Page();
                    }
                    else
                    {
                        // var resul = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                        Microsoft.AspNetCore.Identity.SignInResult resul = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, false, true);

                        if (resul.Succeeded)
                        {
                            var user = await _userManager.FindByEmailAsync(Input.Email);
                            await _signInManager.SignInAsync(user, isPersistent: Input.RememberMe);

                            var date = new UserLoginHistory
                            {
                                LoginTime = DateTime.Now,
                                ApplicationUserId = user.Id
                            };
                            db.UserLoginHistories.Add(date);
                            await db.SaveChangesAsync();

                            _logger.LogInformation("User logged in.");
                            return LocalRedirect(returnUrl);
                        }
                        if (resul.IsLockedOut)
                        {
                            ModelState.AddModelError("", "Вы были заблокированы на нашем майнкрафт сервере, приходи через 20 секунд и запомни, у тебя 3 попытки");
                        }


                    }

                }
            }
            return Page();
        }
    }
}
