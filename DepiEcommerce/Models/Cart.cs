﻿using Microsoft.AspNetCore.Identity;

namespace DepiEcommerce.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Qty { get; set; }
        public string UserId { get; set; }
        public Product Product { get; set; }
        public ApplicationUser User { get; set; }
    }
}
