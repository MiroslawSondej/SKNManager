using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SKNManager.Models.AccountViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Proszę podać adres email")]
        [EmailAddress(ErrorMessage = "Wprowadź poprawny adres email")]
        public string Email { get; set; }
    }
}
