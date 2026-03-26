using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityPractice.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Vacation> Vacations { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder); // Important — sets up Identity tables

            builder.Entity<Vacation>()
                .HasOne(v => v.Traveler)
                .WithMany(u => u.Vacations)
                .HasForeignKey(v => v.TravelerId);
        }
    }
}
