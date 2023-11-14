using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Entity.DTOs.Identity
{
    public record UserForRegistrationDto
    {
        public string? Firstname { get; init; }
        public string? Lastname { get; init; }
        [Required]
        public string? Username { get; init; }
        [Required] 
        public string? Password { get; init; }
        public string? Email { get; init; }
        public string? Phonenumber { get; init; }
        public ICollection<string> Roles { get; init; }
    }
}
