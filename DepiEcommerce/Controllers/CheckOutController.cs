using DepiEcommerce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DepiEcommerce.Controllers
{
    [Authorize]
    public class CheckOutController : Controller
    {


        public readonly ApplicationDbContext _context;
        public readonly UserManager<ApplicationUser> _userManager;

        public CheckOutController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()

        {

            var currentuser = await _userManager.GetUserAsync(HttpContext.User);

            var addresses = await _context.Addresses
            .Include(x => x.User)
            .Where(x => x.UserId == currentuser.Id)
            .ToListAsync();

            ViewBag.Addresses = addresses;

            return View();

        }

        [HttpPost]
        public async Task<IActionResult> Index(Address address)
        {
            if (ModelState.IsValid)
            {
                var currentuser = await _userManager.GetUserAsync(HttpContext.User);

                address.UserId = currentuser.Id;

                _context.Addresses.Add(address);

                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return View(address);

        }



        public async Task<IActionResult> Delete(int addressId)
        {

            var address = await _context.Addresses.Where(x => x.Id == addressId).FirstOrDefaultAsync();

            if (address == null)
            {
                return BadRequest();
            }
            _context.Addresses.Remove(address);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Confirm(int addressId)
        {

            var address = await _context.Addresses.Where(x => x.Id == addressId).FirstOrDefaultAsync();

            if (address == null)
            {
                return BadRequest();
            }

            var currentuser = await _userManager.GetUserAsync(HttpContext.User);
            double orderCost = 0;

            var carts = await _context.Carts
            .Include(x => x.Product)
            .Where(x => x.UserId == currentuser.Id).ToListAsync();


            foreach (var cart in carts)
            {
                orderCost += (cart.Product.Price * cart.Qty);
            }

            var order = new Order
            {
                AddressId = addressId,
                CreatedAt = DateTime.Now,
                Status = "Pending",
                UserId = currentuser.Id,
                Amount = orderCost,
            };
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();


            foreach (var cart in carts)
            {

                var orderProduct = new OrderProduct
                {
                    ProductId = cart.ProductId,
                    OrderId = order.Id,
                    Price = cart.Product.Price,
                    Qty = cart.Qty
                };

                _context.Add(orderProduct);

            }
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(ThankYou));
        }

        public async Task<IActionResult> ThankYou()
        {
            return View();
        }
    }
}
