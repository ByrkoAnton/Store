using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Store.DataAccessLayer.Entities;
using static Store.DataAccessLayer.Enums.Enums.PrintingEditionEnums;

namespace Store.DataAccessLayer.AppContext
{
    public class ApplicationContext : IdentityDbContext<User, IdentityRole<long>, long>
    {

        public DbSet<PrintingEdition> PrintingEditions { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<AuthorPrintingEdition> AuthorPrintingEditions { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AuthorPrintingEdition>()
            .HasKey(t => new { t.AuthorId, t.PrintingEditionId });

            modelBuilder.Entity<AuthorPrintingEdition>()
                .HasOne(sc => sc.Author)
                .WithMany(s => s.AuthorPrintingEditions)
                .HasForeignKey(sc => sc.AuthorId);

            modelBuilder.Entity<AuthorPrintingEdition>()
                .HasOne(sc => sc.PrintingEdition)
                .WithMany(c => c.AuthorPrintingEditions)
                .HasForeignKey(sc => sc.PrintingEditionId);

            var author1 = new Author { Id = 1, Name = "Tolstoy" };
            var author2 = new Author { Id = 2, Name = "Gogol" };
            var author3 = new Author { Id = 3, Name = "Pushkin" };
            var author4 = new Author { Id = 4, Name = "Gorkiy" };

            modelBuilder.Entity<Author>().HasData(author1);
            modelBuilder.Entity<Author>().HasData(author2);
            modelBuilder.Entity<Author>().HasData(author3);
            modelBuilder.Entity<Author>().HasData(author4);

            var user1 = new User { Id = 1, FirstName = "Ivan", LastName = "Ivanov", IsBlocked = false, Email = "123@gmail.com" };
            var user2 = new User { Id = 2, FirstName = "Petr", LastName = "Petrov", IsBlocked = false, Email = "1235@gmail.com" };
            var user5 = new User { Id = 3, FirstName = "Vova", LastName = "Sidorov", IsBlocked = false, Email = "123555@gmail.com" };

            modelBuilder.Entity<User>().HasData(user1);
            modelBuilder.Entity<User>().HasData(user2);
            modelBuilder.Entity<User>().HasData(user5);

            var pe1 = new PrintingEdition
            {
                Id = 1,
                Description = "Anna Karenina",
                Prise = 5,
                IsRemoved = false,
                Status = "Avalible",
                Currency = Currency.UAH,
                Type = Type.Book
            };
            var pe2 = new PrintingEdition
            {
                Id = 2,
                Description = "Kosaks",
                Prise = 5,
                IsRemoved = false,
                Status = "Avalible",
                Currency = Currency.UAH,
                Type = Type.Book
            };
            var pe3 = new PrintingEdition
            {
                Id = 3,
                Description = "Diablo",
                Prise = 4,
                IsRemoved = false,
                Status = "Avalible",
                Currency = Currency.UAH,
                Type = Type.Book
            };
            var pe4 = new PrintingEdition
            {
                Id = 4,
                Description = "The Captain's Daughter",
                Prise = 5,
                IsRemoved = false,
                Status = "Avalible",
                Currency = Currency.JPY,
                Type = Type.Book
            };
            var pe5 = new PrintingEdition
            {
                Id = 5,
                Description = "Eugene Onegin",
                Prise = 5,
                IsRemoved = false,
                Status = "Avalible",
                Currency = Currency.UAH,
                Type = Type.Book
            };

            modelBuilder.Entity<PrintingEdition>().HasData(pe1);
            modelBuilder.Entity<PrintingEdition>().HasData(pe2);
            modelBuilder.Entity<PrintingEdition>().HasData(pe3);
            modelBuilder.Entity<PrintingEdition>().HasData(pe4);
            modelBuilder.Entity<PrintingEdition>().HasData(pe5);

            modelBuilder.Entity<AuthorPrintingEdition>().HasData(new AuthorPrintingEdition { PrintingEditionId = pe1.Id, AuthorId = author1.Id });
            modelBuilder.Entity<AuthorPrintingEdition>().HasData(new AuthorPrintingEdition { PrintingEditionId = pe2.Id, AuthorId = author1.Id });
            modelBuilder.Entity<AuthorPrintingEdition>().HasData(new AuthorPrintingEdition { PrintingEditionId = pe3.Id, AuthorId = author1.Id });
            modelBuilder.Entity<AuthorPrintingEdition>().HasData(new AuthorPrintingEdition { PrintingEditionId = pe4.Id, AuthorId = author3.Id });
            modelBuilder.Entity<AuthorPrintingEdition>().HasData(new AuthorPrintingEdition { PrintingEditionId = pe5.Id, AuthorId = author3.Id });
            modelBuilder.Entity<AuthorPrintingEdition>().HasData(new AuthorPrintingEdition { PrintingEditionId = pe1.Id, AuthorId = author2.Id });

            base.OnModelCreating(modelBuilder);
        }
    }
}
