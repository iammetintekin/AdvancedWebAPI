using System.ComponentModel.DataAnnotations;

namespace Project.Entity.DTOs.Identity
{
    public record UserForAuthenticationDto
    {
        [Required]
        public string Username { get; init; }
        [Required] 
        public string Password { get; init; }
    }
}
