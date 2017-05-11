using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SKNManager.Models.MemberViewModels
{
    public class AddMemberViewModel
    {
        [Required]
        [Display(Name = "Imię i nazwisko")]
        public string Name { get; set; }
    }
}
