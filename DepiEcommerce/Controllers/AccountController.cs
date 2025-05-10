using DepiEcommerce.Models;
using DepiEcommerce.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DepiEcommerce.Controllers
{
    public class AccountController : Controller
    {

        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ApplicationDbContext _context;



        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }





        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel UserViewModel)
        {
            if (ModelState.IsValid)
            {
                // Mapping
                ApplicationUser user = new ApplicationUser()
                {
                    UserName = UserViewModel.UserName,
                    FirstName = UserViewModel.FirstName,
                    LastName = UserViewModel.LastName,
                };

                // Save in DB
                IdentityResult result = await userManager.CreateAsync(user, UserViewModel.Password);
                if (result.Succeeded)
                {
                    if (userManager.Users.Count() == 1)
                    {
                        if (!await roleManager.RoleExistsAsync("Admin"))
                        {
                            await roleManager.CreateAsync(new IdentityRole("Admin"));
                        }

                        await userManager.AddToRoleAsync(user, "Admin");
                    }

                    await signInManager.SignInAsync(user, false);

                    //Authorized action
                    if (User.IsInRole("Admin"))
                    {
                        return RedirectToAction("Index", "Dashboard");
                    }
                    return RedirectToAction("Index", "Home");
                }

                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
            }

            return View(UserViewModel);
        }







        public async Task<IActionResult> SignOut()
        {
            //Destroy cookie
            await signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }



        [HttpGet]
        public IActionResult Login()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginUserViewModel UserViewModel)
        {

            if (ModelState.IsValid)
            {
                //check Found
                ApplicationUser appUser = await userManager.FindByNameAsync(UserViewModel.UserName);
                if (appUser != null)
                {
                    //Check Password
                    bool Found = await userManager.CheckPasswordAsync(appUser, UserViewModel.Password);
                    if (Found)
                    {

                        //Create cookie
                        await signInManager.SignInAsync(appUser, UserViewModel.RememberMe);
                        //Authorized action
                        if (User.IsInRole("Admin"))
                        {
                            return RedirectToAction("Index", "Dashboard");
                        }


                        return RedirectToAction("Index", "Home");
                    }
                }
                ModelState.AddModelError("", "UserName or Password is not right !");
            }
            return View(UserViewModel);
        }




        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            ApplicationUser currentuser = await userManager.GetUserAsync(HttpContext.User);
            ProfileViewModel profileViewModel = new ProfileViewModel();
            profileViewModel.PhoneNumber = currentuser.PhoneNumber;
            profileViewModel.FirstName = currentuser.FirstName;
            profileViewModel.LastName = currentuser.LastName;
            profileViewModel.Username = currentuser.UserName;
            return View(profileViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Profile(ProfileViewModel profileViewModel)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser currentuser = await userManager.GetUserAsync(HttpContext.User);
                currentuser.PhoneNumber = profileViewModel.PhoneNumber;
                currentuser.FirstName = profileViewModel.FirstName;
                currentuser.LastName = profileViewModel.LastName;
                await userManager.UpdateAsync(currentuser);
            }
            return RedirectToAction(nameof(Profile));

        }
    }
}
