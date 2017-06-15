using SKNManager.Models.EquipmentViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
//using SKNManager.Utils.Validation;

namespace SKNManager.Models
{
    public class EquipmentLoans
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Użytkownik")]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }

        [Display(Name = "Sprzet")]
        public int? EquipmentId { get; set; }

        [Display(Name = "Sprzet")]
        [ForeignKey("EquipmentId")]
        public virtual Equipment Equipment { get; set; }

        [Display(Name = "Ilość")]
        public int Amount { get; set; }

        [Display(Name = "Zestaw")]
        public Nullable<int> EquipmentSetId { get; set; }

        [Display(Name = "Zestaw")]
        [ForeignKey("EquipmentSetId")]
        public virtual EquipmentSet EquipmentSet { get; set; }

        [Display(Name = "Data wyporzyczenia")]
        [DataType(DataType.Date, ErrorMessage = "Proszę wprowadzić poprawną datę (DD.MM.RRRR)")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Data wyporzyczenia jest wymagana")]
        public DateTime LoanDate { get; set; }

        [Display(Name = "Data oddania")]
        [DataType(DataType.Date, ErrorMessage = "Proszę wprowadzić poprawną datę (DD.MM.RRRR)")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Data zakończenia jest wymagana")]
        //[IsDateAfter("LoanDate", ErrorMessage = "Data zakończenia nie może być wcześniejsza niż data rozpoczęcia")]
        public DateTime ReturnDate { get; set; }

        [Display(Name = "Komentarz")]
        public String Comments { get; set; }
    }
}
