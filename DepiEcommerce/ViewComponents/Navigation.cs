using DepiEcommerce.Models;
using DepiEcommerce.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DepiEcommerce.ViewComponents
{

    [ViewComponent(Name = "Navigation")]
    public class Navigation : ViewComponent
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public Navigation(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {








            var currentuser = await _userManager.GetUserAsync(HttpContext.User);
            if (currentuser == null)
            {
                var model1 = new NavigationViewModel { };
                return View("Index", model1);
            }
            var cart = await _context.Carts
                .Where(x => x.UserId == currentuser.Id)
                .ToListAsync();

            var model = new NavigationViewModel
            {
                CartCount = cart.Count
            };



            




            return View("Index", model);
        }



    }
}
