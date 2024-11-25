namespace Talabat.Core.Application.Abstractions.DTOModels
{
    public class AddressDTO
    {
        public string? Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Street { get; set; } = null!;
        public string Country { get; set; } = null!;
    }
}
