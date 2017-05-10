using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SKNManager.Models.AccountViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Proszę podać adres email")]
        [EmailAddress(ErrorMessage = "Wprowadź poprawny adres email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Proszę podać hasło")]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string Password { get; set; }

        [Display(Name = "Zapamiętaj mnie!")]
        public bool RememberMe { get; set; }
    }
}
