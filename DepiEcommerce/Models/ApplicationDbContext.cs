using DepiEcommerce.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DepiEcommerce.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }





        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Address
            modelBuilder.Entity<Address>()
                .HasKey(a => a.Id);

            modelBuilder.Entity<Address>()
                .HasOne(a => a.User)
                .WithMany() // or WithMany(u => u.Addresses) if ApplicationUser has ICollection<Address>
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Address>()
                .Property(a => a.AddresssLine)
                .IsRequired()
                .HasMaxLength(255);

            modelBuilder.Entity<Address>()
                .Property(a => a.City)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Address>()
                .Property(a => a.State)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Address>()
                .Property(a => a.PinCode)
                .IsRequired()
                .HasMaxLength(20);

            // Cart
            modelBuilder.Entity<Cart>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<Cart>()
                .HasOne(c => c.Product)
                .WithMany()
                .HasForeignKey(c => c.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Cart>()
                .HasOne(c => c.User)
                .WithMany() // or WithMany(u => u.Carts) if ApplicationUser has ICollection<Cart>
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Order
            modelBuilder.Entity<Order>()
                .HasKey(o => o.Id);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany() // or WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Address)
                .WithMany()
                .HasForeignKey(o => o.AddressId)
                .OnDelete(DeleteBehavior.Restrict); // To prevent deleting address if order exists

            modelBuilder.Entity<Order>()
                .Property(o => o.Status)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Order>()
                .Property(o => o.Amount)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Order>()
                .Property(o => o.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            // Product
            modelBuilder.Entity<Product>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<Product>()
                .Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(150);

            modelBuilder.Entity<Product>()
                .Property(p => p.Description)
                .HasMaxLength(1000);

            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Product>()
                .Property(p => p.Image)
                .HasMaxLength(255);

            // OrderProduct
            modelBuilder.Entity<OrderProduct>()
                .HasKey(op => op.Id);

            modelBuilder.Entity<OrderProduct>()
                .HasOne(op => op.Product)
                .WithMany()
                .HasForeignKey(op => op.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderProduct>()
                .HasOne(op => op.Order)
                .WithMany(o => o.orderProducts)
                .HasForeignKey(op => op.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderProduct>()
                .Property(op => op.Price)
                .HasColumnType("decimal(18,2)");
        }


        public DbSet<Product> Products { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderProduct> orderProducts { get; set; }
        public DbSet<UserAlert> UserAlerts { get; set; }



    }
}
