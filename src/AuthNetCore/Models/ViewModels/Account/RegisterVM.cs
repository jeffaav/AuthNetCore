using System.ComponentModel.DataAnnotations;

namespace AuthNetCore.Models.ViewModels.Account
{
    public class RegisterVM
    {
        [Required]
        [EmailAddress]
        [MaxLength(256)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [MaxLength(9)]
        public string Cellphone { get; set; }

        [Required]
        [MaxLength(50)]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        public string Errors { get; set; }
    }
}
