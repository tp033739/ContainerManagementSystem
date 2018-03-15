using ContainerManagementSystem.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ContainerManagementSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public DbSet<ContainerManagementSystem.Models.Vessel> Vessel { get; set; }

        public DbSet<ContainerManagementSystem.Models.Customer> Customer { get; set; }

        public DbSet<ContainerManagementSystem.Models.ShippingSchedule> ShippingSchedule { get; set; }

        public DbSet<ContainerManagementSystem.Models.Booking> Booking { get; set; }

    }
}
