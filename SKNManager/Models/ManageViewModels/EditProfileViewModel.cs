using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SKNManager.Models.ManageViewModels
{
    public class EditProfileViewModel
    {
        [Display(Name = "Imię")]
        [Required(ErrorMessage = "Pole jest wymagane.")]
        public string FirstName { get; set; }

        [Display(Name = "Nazwisko")]
        [Required(ErrorMessage = "Pole jest wymagane.")]
        public string LastName { get; set; }

        [Display(Name = "Numer telefonu")]
        [Required(ErrorMessage = "Pole jest wymagane.")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Nieprawidłowy format numeru telefonu.")]
        public string PhoneNumber{ get; set; }
    }
}
