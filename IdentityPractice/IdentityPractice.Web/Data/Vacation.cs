namespace IdentityPractice.Web.Data
{
    public class Vacation
    {
        public int Id { get; set; }
        public string Place { get; set; } = string.Empty;
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }

        // FK to ApplicationUser
        public string TravelerId { get; set; } = string.Empty;  // IdentityUser PKs are strings (GUID)
        public ApplicationUser Traveler { get; set; } = null!;
    }
}
