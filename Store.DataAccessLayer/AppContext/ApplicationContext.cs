using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Store.DataAccessLayer.Entities;
using System;
using static Store.DataAccessLayer.Enums.Enums.PrintingEditionEnums;
using Type = Store.DataAccessLayer.Enums.Enums.PrintingEditionEnums.Type;

namespace Store.DataAccessLayer.AppContext
{
    public class ApplicationContext : IdentityDbContext<User, IdentityRole<long>, long>
    {

        public DbSet<PrintingEdition> PrintingEditions { get; set; }
        public DbSet<Author> Authors { get; set; }
        

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            //Database.EnsureDeleted();
            //Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PrintingEdition>()
                .HasMany(c => c.Authors)
                .WithMany(s => s.PrintingEditions)
                .UsingEntity(j => j.ToTable("AuthorPrintingEdition"));

            modelBuilder.Entity<Author>()
            .HasData(
            new Author
            {
                Id = 1L,
                Name = "TestAuthor"
            });

            modelBuilder.Entity<PrintingEdition>()
            .HasData(
            new PrintingEdition
            {
                Id = 1L,
                Description = "init desc",
                Currency = Currency.UAH,
                Prise = 5,
                Status = "Avalible",
                Type = Type.Book,

            });
           
        
            var author1 = new Author { Id = 2, Name = "Tolstoy" };
            var author2 = new Author { Id = 3, Name = "Gogol" };
            var author3 = new Author { Id = 4, Name = "Pushkin" };
            var author4 = new Author { Id = 5, Name = "Gorkiy" };

            modelBuilder.Entity<Author>().HasData(author1);
            modelBuilder.Entity<Author>().HasData(author2);
            modelBuilder.Entity<Author>().HasData(author3);
            modelBuilder.Entity<Author>().HasData(author4);

            //var user1 = new User { Id = 1, FirstName = "Ivan", LastName = "Ivanov", IsBlocked = false, Email = "123@gmail.com" };
            //var user2 = new User { Id = 2, FirstName = "Petr", LastName = "Petrov", IsBlocked = false, Email = "1235@gmail.com" };
            //var user5 = new User { Id = 3, FirstName = "Vova", LastName = "Sidorov", IsBlocked = false, Email = "123555@gmail.com" };

            //modelBuilder.Entity<User>().HasData(user1);
            //modelBuilder.Entity<User>().HasData(user2);
            //modelBuilder.Entity<User>().HasData(user5);

            var pe1 = new PrintingEdition
            {
                Id = 2,
                Description = "Anna Karenina",
                Prise = 5,
                IsRemoved = false,
                Status = "Avalible",
                Currency = Currency.UAH,
                Type = Type.Book
            };
            var pe2 = new PrintingEdition
            {
                Id = 3,
                Description = "Kosaks",
                Prise = 5,
                IsRemoved = false,
                Status = "Avalible",
                Currency = Currency.UAH,
                Type = Type.Book
            };
            var pe3 = new PrintingEdition
            {
                Id = 4,
                Description = "Diablo",
                Prise = 4,
                IsRemoved = false,
                Status = "Avalible",
                Currency = Currency.UAH,
                Type = Type.Book
            };
            var pe4 = new PrintingEdition
            {
                Id = 5,
                Description = "The Captain's Daughter",
                Prise = 5,
                IsRemoved = false,
                Status = "Avalible",
                Currency = Currency.JPY,
                Type = Type.Book
            };
            var pe5 = new PrintingEdition
            {
                Id = 6,
                Description = "Eugene Onegin",
                Prise = 5,
                IsRemoved = false,
                Status = "Avalible",
                Currency = Currency.UAH,
                Type = Type.Book
            };

            var pe6 = new PrintingEdition
            {
                Id = 7,
                Description = "Boris Godynov",
                Prise = 50,
                IsRemoved = false,
                Status = "Avalible",
                Currency = Currency.GBP,
                Type = Type.Book
            };

            modelBuilder.Entity<PrintingEdition>().HasData(pe1);
            modelBuilder.Entity<PrintingEdition>().HasData(pe2);
            modelBuilder.Entity<PrintingEdition>().HasData(pe3);
            modelBuilder.Entity<PrintingEdition>().HasData(pe4);
            modelBuilder.Entity<PrintingEdition>().HasData(pe5);
            modelBuilder.Entity<PrintingEdition>().HasData(pe6);

            modelBuilder.Entity<Author>()
           .HasMany(author => author.PrintingEditions)
           .WithMany(edition => edition.Authors)
           .UsingEntity(join => join.HasData(new { AuthorsId = 2L, PrintingEditionsId = 2L }));

            base.OnModelCreating(modelBuilder);
        }
    }
}
