namespace Talabat.Core.Application.Abstractions.DTOModels.Auth
{
    public class UserDTO
    {
        public string DisplayName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Token { get; set; } = null!;
    }
}
