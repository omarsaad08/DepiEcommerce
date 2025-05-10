using DepiEcommerce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DepiEcommerce.Controllers
{
    public class OrdersController : Controller
    {


        public readonly ApplicationDbContext _context;
        public readonly UserManager<ApplicationUser> _userManager;

        public OrdersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var orders = await _context.Orders
                .Where(O => O.UserId == user.Id)
                .Where(O => O.Status != "Rejected")
                .ToListAsync();






            return View(orders);

        }

        public async Task<IActionResult> Details(int id)

        {
            var order = await _context.Orders
                .Include(x => x.orderProducts)
                .ThenInclude(x => x.Product)
                .Include(x => x.Address)
                .FirstOrDefaultAsync(x => x.Id == id);
            return View(order);
        }

    }
}
