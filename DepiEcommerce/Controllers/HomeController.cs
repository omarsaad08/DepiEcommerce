using System.Diagnostics;
using DepiEcommerce.Models;
using DepiEcommerce.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DepiEcommerce.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;


        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {

            if (User.Identity.IsAuthenticated)
            {



                if (User.IsInRole("Admin"))
                {
                    return RedirectToAction("Index", "Dashboard");
                }
                var username = User.Identity?.Name;
                var user = await _userManager.GetUserAsync(User);



                if (!string.IsNullOrWhiteSpace(username))
                {

                    var alert = _context.UserAlerts.Where(a => a.UserName == username).ToList();
                    foreach (var i in alert)
                        if (alert != null)
                        {
                            CookieOptions options = new CookieOptions { Expires = DateTime.Now.AddMinutes(2) };
                            Response.Cookies.Append($"RM_{user.Id}_{i.Id}", i.Message, options);
                        }
                    foreach (var i in alert)
                    {
                        _context.UserAlerts.Remove(i);
                    }
                    _context.SaveChanges();

                }
            }

            var products = _context.Products.ToList();
            var model = new HomeViewModel { Products = products };

            return View(model);
        }

        public IActionResult Privacy()
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
