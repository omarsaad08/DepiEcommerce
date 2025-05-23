﻿using DepiEcommerce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace DepiEcommerce.Controllers
{
    [Authorize]
    public class CartController : Controller
    {


        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;


        public CartController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var currentuser = await _userManager.GetUserAsync(User);
            var cart = await _context.Carts
                .Include(x => x.Product)
                .Where(x => x.UserId == currentuser.Id)
                .ToListAsync();

            double totalCost = 0;
            foreach (var cartItem in cart)
            {
                totalCost += cartItem.Product.Price * cartItem.Qty;
            }

            ViewBag.TotalCost = totalCost;

            return View(cart);
        }



        public async Task<IActionResult> Remove(int id)

        {

            var cartItem = await _context.Carts.FindAsync(id);

            if (cartItem == null)
            {
                return BadRequest();
            }

            _context.Carts.Remove(cartItem);

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }





        public async Task<IActionResult> UpdateQty(int productId, int qty)
        {
            var product = await _context.Products.Where(x => x.Id == productId).FirstOrDefaultAsync();

            if (product == null)

            {

                return BadRequest();
            }

            var currentuser = await _userManager.GetUserAsync(HttpContext.User);

            var cartItem = await _context.Carts.Where(x => x.UserId == currentuser.Id)
            .Where(x => x.ProductId == productId)
            .FirstOrDefaultAsync();

            if (cartItem == null)
            {

                return BadRequest();
            }

            cartItem.Qty = qty;
            _context.Carts.Update(cartItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }





        public async Task<IActionResult> AddToCart(int productId, int qty = 1)
        {

            var currentuser = await _userManager.GetUserAsync(HttpContext.User);

            var product = await _context.Products.Where(x => x.Id == productId).FirstOrDefaultAsync();

            if (product == null)
            {
                return BadRequest();

            }

            var cart = new Cart { ProductId = productId, Qty = qty, UserId = currentuser.Id };

            _context.Add(cart);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
    }
}
