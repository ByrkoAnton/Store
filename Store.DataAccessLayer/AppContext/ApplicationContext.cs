using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Store.DataAccessLayer.Entities;
using Store.DataAccessLayer.Initialization;
using Store.Sharing.Constants;

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
           // Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Seed();
            base.OnModelCreating(modelBuilder);
        }
    }

    //public class DbContextFactory : IDesignTimeDbContextFactory<ApplicationContext>
    //{
      
    //    ApplicationContext IDesignTimeDbContextFactory<ApplicationContext>.CreateDbContext(string[] args)
    //    {
    //        var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
    //        optionsBuilder.UseSqlServer(Constants.Variables.CONNECTIONSTRING_NAME);

    //        return new ApplicationContext(optionsBuilder.Options);
    //    }
    //}
}
