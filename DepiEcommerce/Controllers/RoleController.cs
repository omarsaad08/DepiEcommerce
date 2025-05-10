using DepiEcommerce.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;

namespace DepiEcommerce.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;

        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            this.roleManager = roleManager;
        }
        [HttpGet]
        public IActionResult AddRole()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddRole(RoleViewModel roleViewModel)
        {
            if (ModelState.IsValid)
            {
                //Mapping 
                IdentityRole role = new IdentityRole()
                {
                    Name = roleViewModel.RoleName
                };
                //Save DB
                IdentityResult result = await roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    return View();
                }
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
            }
            return View(roleViewModel);
        }
    }
}
