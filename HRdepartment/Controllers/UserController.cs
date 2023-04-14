using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using HRdepartment.Data;
using HRdepartment.Models;
using Microsoft.AspNetCore.Identity;
using static HRdepartment.Controllers.UserController;

namespace HRdepartment.Controllers
{
    public class UserController : Controller
    {
        private ApplicationDbContext db;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            db = context;
        }
        public async Task<IActionResult> ListUser(string searchString)
        {
            var user = await db.ApplicationUser
            .Include(p => p.Post)
            .Include(p => p.Department)
            .ToListAsync();


            if (!string.IsNullOrEmpty(searchString))
            {
                user = user.Where(v => v.FirstName.Contains(searchString)).ToList();
            }
            else if (!string.IsNullOrEmpty(searchString))
            {
                user = user.Where(v => v.LastName.Contains(searchString)).ToList();
            }
            else if (!string.IsNullOrEmpty(searchString))
            {
                user = user.Where(v => v.Patronymic.Contains(searchString)).ToList();
            }





            return View(user.ToList());
        }
        public record SelectOptions
        {
            public int value { get; set; }
            public string text { get; set; }
        }


        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {model.Id} cannot be found";
                return View("NotFound");
            }
            else
            {
                user.Adress = model.Adress;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Patronymic = model.Patronymic;
                user.PhoneNumber = model.NumberPhone;
                user.Id_Post = model.Id_Post;
                user.Department_Id = model.Department_Id;
                user.Passport_number = model.Passport_number;
                user.Passport_series = model.Passport_series;
                user.Date_birth = model.Date_birth;
                user.Education = model.Education;
                user.Driver_license = model.Driver_license;
                user.Rank = model.Rank;

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListUser");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            var post = await db.Post
                  .Select(s => new SelectOptions
                  {
                      value = s.Id_Post,
                      text = s.Title_Post,
                  })
                  .ToListAsync();
            ViewData["Post"] = post;

            var department = await db.Department
                .Select(s => new SelectOptions
                {
                    value = s.Department_Id,
                    text = s.Department_Title,
                })
                .ToListAsync();
            ViewData["Department"] = department;

            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                return View("NotFound");
            }

            // GetClaimsAsync retunrs the list of user Claims
            var userClaims = await _userManager.GetClaimsAsync(user);
            // GetRolesAsync returns the list of user Roles
            var userRoles = await _userManager.GetRolesAsync(user);

            var model = new EditUserViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Patronymic = user.Patronymic,
                Adress = user.Adress,
                NumberPhone = user.PhoneNumber,
                Id_Post = user.Id_Post,
                Department_Id = user.Department_Id,
                Passport_series = user.Passport_series,
                Passport_number = user.Passport_number,
                Date_birth = user.Date_birth,
                Education = user.Education,
                Driver_license = user.Driver_license,
                Rank = user.Rank,



                Claims = userClaims.Select(c => c.Value).ToList(),
                Roles = userRoles
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {

            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"Пользователь с идентификатором =  {id}  не найден";
                return View("NotFound");
            }
            else
            {


                var result = await _userManager.DeleteAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListUser");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View("ListUsers");
            }
        }
    }
}
