using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Store.DataAccessLayer.Entities;
using Store.DataAccessLayer.Initialization;

namespace Store.DataAccessLayer.AppContext
{
    public class ApplicationContext : IdentityDbContext<User, IdentityRole<long>, long>
    {
        public DbSet<PrintingEdition> PrintingEditions { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Payment> Payments { get; set; } //TODO unused property. If u use EF and dapper at the same time remove this one. If no leave a comment. It's need for more clear understanding project's logic---
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; } //TODO unused property. If u use EF and dapper at the same time remove this one. If no leave a comment----

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
           Database.Migrate();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Seed();
            base.OnModelCreating(modelBuilder);
        }
    }
}
