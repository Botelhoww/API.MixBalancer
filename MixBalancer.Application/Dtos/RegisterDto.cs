using System.ComponentModel.DataAnnotations;

namespace MixBalancer.Application.Dtos
{
    public class RegisterDto
    {
        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, MinLength(3)]
        public string Username { get; set; }

        [Required, MinLength(8)]
        public string Password { get; set; }
    }
}