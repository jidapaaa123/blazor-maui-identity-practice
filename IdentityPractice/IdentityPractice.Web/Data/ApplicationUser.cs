using Microsoft.AspNetCore.Identity;

namespace IdentityPractice.Web.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; } = string.Empty;
        public DateTimeOffset? Birthday { get; set; }

        // Navigation property
        public ICollection<Vacation> Vacations { get; set; } = new List<Vacation>();
    }
}
