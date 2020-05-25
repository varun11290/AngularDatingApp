using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.DTO
{
    public class CreateUserDTO
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [StringLength(8,MinimumLength=4,ErrorMessage="Password between 4 and 8 char")]
        public string Password { get; set; }
    }
}