using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Store.DataAccessLayer.Entities;
using System;
using static Store.DataAccessLayer.Enums.Enums;

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
            Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Author>().Property(b => b.DateOfCreation).HasDefaultValue(DateTime.MaxValue);
            modelBuilder.Entity<PrintingEdition>().Property(b => b.DateOfCreation).HasDefaultValue(DateTime.MaxValue);
            modelBuilder.Entity<Payment>().Property(b => b.DateOfCreation).HasDefaultValue(DateTime.MaxValue);
            modelBuilder.Entity<Order>().Property(b => b.DateOfCreation).HasDefaultValue(DateTime.MaxValue);
            modelBuilder.Entity<OrderItem>().Property(b => b.DateOfCreation).HasDefaultValue(DateTime.MaxValue);
            modelBuilder.Entity<User>().Property(b => b.DateOfCreation).HasDefaultValue(DateTime.MaxValue);

            modelBuilder.Entity<Author>()
            .HasData(
            new Author
            {
                Id = 1L,
                Name = "FirstAuthor"
            });

            modelBuilder.Entity<PrintingEdition>()
            .HasData(
            new PrintingEdition
            {
                Id = 1L,
                Description = "FirstEdition",
                Currency = Currency.UAH,
                Prise = 5,
                Status = "Avalible",
                Type = PrintingEditionType.Book,
            });

            modelBuilder.Entity<Author>()
           .HasMany(author => author.PrintingEditions)
           .WithMany(edition => edition.Authors)
           .UsingEntity(join => join.HasData(new { AuthorsId = 1L, PrintingEditionsId = 1L }));

            base.OnModelCreating(modelBuilder);
        }
    }
}
