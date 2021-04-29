using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Store.DataAccessLayer.Entities;
using System;


namespace Store.DataAccessLayer.AppContext
{
    public class ApplicationContext : IdentityDbContext<User, IdentityRole<long>, long>
    {
       public DbSet<PrintingEdition> PrintingEditions { get; set; }
       public DbSet<Author> Authors { get; set; }
       public DbSet<Payment> Payments { get; set; }
       public DbSet<Order> Orders { get; set; }
       public DbSet<OrderItem> OrderItems { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Author>().Property(b => b.DateOfCreation).HasDefaultValue(DateTime.MaxValue);
            modelBuilder.Entity<PrintingEdition>().Property(b => b.DateOfCreation).HasDefaultValue(DateTime.MaxValue);
            modelBuilder.Entity<Payment>().Property(b => b.DateOfCreation).HasDefaultValue(DateTime.MaxValue);
            modelBuilder.Entity<Order>().Property(b => b.DateOfCreation).HasDefaultValue(DateTime.MaxValue);
            modelBuilder.Entity<OrderItem>().Property(b => b.DateOfCreation).HasDefaultValue(DateTime.MaxValue);
            modelBuilder.Entity<User>().Property(b => b.DateOfCreation).HasDefaultValue(DateTime.MaxValue);
        }   
    }
}
