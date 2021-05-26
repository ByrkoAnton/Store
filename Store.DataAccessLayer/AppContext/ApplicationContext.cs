using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Store.DataAccessLayer.Entities;
using Store.PresentationLayer;
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

            modelBuilder.Seed();
           // modelBuilder.Entity<Author>()
           // .HasData(
           // new Author
           // {
           //     Id = 1L,
           //     Name = "FirstAuthor"
           // });

           // modelBuilder.Entity<PrintingEdition>()
           // .HasData(
           // new PrintingEdition
           // {
           //     Id = 1L,
           //     Description = "FirstEdition",
           //     Currency = Currency.UAH,
           //     Price = 5,
           //     Status = "Avalible",
           //     Type = PrintingEditionType.Book,
           // });

           // modelBuilder.Entity<Author>()
           //.HasMany(author => author.PrintingEditions)
           //.WithMany(edition => edition.Authors)
           //.UsingEntity(join => join.HasData(new { AuthorsId = 1L, PrintingEditionsId = 1L }));

            base.OnModelCreating(modelBuilder);
        }
    }
}
