using System.ComponentModel.DataAnnotations;

namespace APIBanco.DTOs
{
    public class ClienteDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string  UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string PasswordConfirmed { get; set; }
    }
}
