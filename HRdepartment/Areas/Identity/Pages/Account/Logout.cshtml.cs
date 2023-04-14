using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using HRdepartment.Data;
using HRdepartment.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HRdepartment.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LogoutModel : PageModel
    {
        private readonly ApplicationDbContext db;
   
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LogoutModel> _logger;

        public LogoutModel(SignInManager<ApplicationUser> signInManager, ILogger<LogoutModel> logger, ApplicationDbContext context)
        {
            _signInManager = signInManager;
            _logger = logger;
            db = context;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();
           
            _logger.LogInformation("User logged out.");
            if (returnUrl != null)
            {
                var lastUserLogin = db.UserLoginHistories.OrderByDescending(h => h.LoginTime).FirstOrDefault();
                if (lastUserLogin != null)
                {
                    lastUserLogin.LogoutTime = DateTime.Now;
                    await db.SaveChangesAsync();
                }
                return LocalRedirect(returnUrl);
            }

            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                return RedirectToPage();
            }
        }
    }
}
