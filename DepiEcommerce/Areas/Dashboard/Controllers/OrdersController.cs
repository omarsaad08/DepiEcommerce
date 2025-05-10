using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DepiEcommerce.Models;
using Microsoft.AspNetCore.Authorization;
using DepiEcommerce.Areas.Dashboard.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace DepiEcommerce.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    [Authorize(Roles = "Admin")]

    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrdersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Dashboard/Orders
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Orders
                .Include(x => x.orderProducts)
                .Include(o => o.Address)
                .Include(o => o.User)
                .Where(O => O.Status == "Order Placed");
            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> Index2()
        {
            var applicationDbContext = _context.Orders
                .Include(x => x.orderProducts)
                .Include(o => o.Address)
                .Include(o => o.User)
                .Where(O => O.Status == "Pending");
            return View("Index", await applicationDbContext.ToListAsync());
        }


        public async Task<IActionResult> Index3()
        {
            var applicationDbContext = _context.Orders
                .Include(x => x.orderProducts)
                .Include(o => o.Address)
                .Include(o => o.User)
                .Where(O => O.Status == "Rejected");
            return View("Index", await applicationDbContext.ToListAsync());
        }



        public async Task<IActionResult> Confirm(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(op => op.orderProducts)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }
            order.Status = "Order Placed";
            _context.Update(order);
            _context.SaveChanges();


            foreach (var OrderProduct in order.orderProducts)
            {

                Product Product = await _context.Products.FirstOrDefaultAsync(P => P.Id == OrderProduct.ProductId);


                if (Product != null)
                {
                    if (Product.ProductsInStock >= OrderProduct.Qty)
                    {
                        Product.ProductsInStock -= OrderProduct.Qty;
                    }
                    else
                    {
                        throw new InvalidOperationException($"Not enough stock for product: {Product.Name}");
                    }
                }
                _context.Update(Product);
                _context.SaveChanges();

            }

            return RedirectToAction("Index2");
        }

        public async Task<IActionResult> Reject(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }
            order.Status = "Rejected";
            _context.Update(order);
            _context.SaveChanges();

            return RedirectToAction("Index3");
        }




        [HttpGet]
        public async Task<IActionResult> Notify(int? id)
        {

            var userId = _context.Orders.FirstOrDefault(m => m.Id == id).UserId;
            var user = await _userManager.FindByIdAsync(userId);

            RejectMessage rejectMessage = new RejectMessage { UserName = user.UserName, OrderId = id };
            return View(rejectMessage);

        }


        [HttpPost]
        public async Task<IActionResult> Notify(RejectMessage rejectMessage)
        {


            UserAlert alert1 = new UserAlert
            {
                UserName = rejectMessage.UserName,
                CreatedAt = DateTime.Now,
                Message = rejectMessage.RejectMsg,
            };
            _context.Add(alert1);

            var order = _context.Orders.FirstOrDefault(o => o.Id == rejectMessage.OrderId);
            _context.Remove(order);
            _context.SaveChanges();



            return RedirectToAction("Index3");
        }




        // GET: Dashboard/Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(x => x.orderProducts)
                .ThenInclude(x => x.Product)
                .Include(o => o.Address)
                .Include(o => o.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Dashboard/Orders/Create
        public IActionResult Create()
        {
            ViewData["AddressId"] = new SelectList(_context.Addresses, "Id", "AddresssLine");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName");
            return View();
        }

        // POST: Dashboard/Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,AddressId,Amount,Status,CreatedAt")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AddressId"] = new SelectList(_context.Addresses, "Id", "Id", order.AddressId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", order.UserId);
            return View(order);
        }

        // GET: Dashboard/Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["AddressId"] = new SelectList(_context.Addresses, "Id", "Id", order.AddressId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", order.UserId);
            return View(order);
        }

        // POST: Dashboard/Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,AddressId,Amount,Status,CreatedAt")] Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AddressId"] = new SelectList(_context.Addresses, "Id", "Id", order.AddressId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", order.UserId);
            return View(order);
        }

        // GET: Dashboard/Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Address)
                .Include(o => o.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Dashboard/Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}
