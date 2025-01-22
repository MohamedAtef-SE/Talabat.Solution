namespace Talabat.Core.Domain.Entities.Identity
{
    public class Address
    {
        public string Id { get; set; } = null!;
        public required string FirstName { get; set; }
        public string? LastName { get; set; }
        public required string City { get; set; }
        public required string Street { get; set; }
        public required string Country { get; set; }
        public string ApplicationUserId { get; set; } = null!; // FK
        public virtual ApplicationUser User { get; set; } = null!; // Navigational Property 
    }
}