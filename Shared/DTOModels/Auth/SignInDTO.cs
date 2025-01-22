using System.ComponentModel.DataAnnotations;

namespace Talabat.Shared.DTOModels.Auth
{
    public class SignInDTO
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
        
        [Required]
        public required string Password { get; set; }
    }
}
