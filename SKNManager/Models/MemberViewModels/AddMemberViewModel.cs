using System.ComponentModel.DataAnnotations;

namespace SKNManager.Models.MemberViewModels
{
    public class AddMemberViewModel
    {
        [Required]
        [Display(Name = "Imię i nazwisko")]
        public string Name { get; set; }
    }
}
