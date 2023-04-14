using HRdepartment.Data;
using HRdepartment.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace HRdepartment.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db;

        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            db = context;
            _userManager = userManager;
        }


        public async Task<IActionResult> Privacy()
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);

            // Отфильтровать товары, чтобы выбрать только те, у которых ApplicationUserId совпадает с Id текущего пользователя      
            if (currentUser != null)
            {
                var cartItems = await db.ApplicationUser
              .Include(c => c.Post)
              .Include(c => c.Department)
              .Where(c => c.Id == currentUser.Id)
              .ToListAsync();

                return View(cartItems);
            }
            else
            {
                return View();
            }
        }

        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
