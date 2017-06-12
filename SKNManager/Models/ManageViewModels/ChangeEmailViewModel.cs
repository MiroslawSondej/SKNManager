using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SKNManager.Models.ManageViewModels
{
    public class ChangeEmailViewModel
    {
        [Required(ErrorMessage = "Pole jest wymagane")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Proszę wprowadzić prawidłowy adres email")]
        [Display(Name = "Adres email")]
        public string Email { get; set; }
    }
}
