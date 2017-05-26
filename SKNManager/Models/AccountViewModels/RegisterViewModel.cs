using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SKNManager.Models.AccountViewModels
{
    public class RegisterViewModel
    {
        public string UserId { get; set; }
        public string Code { get; set; }

        //[Required(ErrorMessage = "Pole jest wymagane")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Pole jest wymagane")]
        [StringLength(100, ErrorMessage = "{0} musi mieć od {2} do {1} znaków.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Potwierdź hasło")]
        [Compare("Password", ErrorMessage = "Zawartość pól \"Hasło\" i \"Potwierdź hasło\" musi być identyczne")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Imię jest wymagane!")]
        [Display(Name = "Imię")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Nazwisko jest wymagane!")]
        [Display(Name = "Nazwisko")]
        public string LastName { get; set; }
    }
}
