using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SKNManager.Models.ManageViewModels
{
    public class VerifyChangeEmailViewModel
    {
        public string UserId { get; set; }
        public string Code { get; set; }
        public string Email { get; set; }

        [DataType(DataType.EmailAddress, ErrorMessage = "Proszę ponownie wprowadzić prawidłowy adres email")]
        [Display(Name = "Potwierdź adres email")]
        [Compare("Email", ErrorMessage = "Podany adres musi być identyczny jak ten podany podczas generowania prośby zmiany")]
        public string ConfirmEmail { get; set; }

        [Display(Name = "Hasło")]
        [Required(ErrorMessage = "Pole jest wymagane")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
