using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SKNManager.Models.AccountViewModels
{
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "Proszę podać adres email")]
        [EmailAddress(ErrorMessage = "Wprowadź poprawny adres email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Proszę podać nowe hasło")]
        [StringLength(100, ErrorMessage = "{0} musi mieć co najmniej {2} i nie więcej niż {1} znaków.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Potwierdź hasło")]
        [Compare("Password", ErrorMessage = "Podane hasła nie są identyczne.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }
}
