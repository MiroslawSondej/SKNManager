using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SKNManager.Models.EquipmentViewModels
{
    public class Equipment
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Podaj nazwe sprzętu")]
        [Display(Name = "Nazwa")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Podaj ilość sprzętu")]
        [Range(1, 2147483647, ErrorMessage = "Podaj wartość wiekszą od 0")]
        [Display(Name = "Ilosc")]
        public int Amount { get; set ; }

        [Display(Name = "Dostepna ilosc")]
        public int AvailableAmount { get; set; }

        [Display(Name = "Zestaw")]
        public Nullable<int> EquipmentSetId { get; set; }

        [Display(Name = "Zestaw")]
        [ForeignKey("EquipmentSetId")]
        public virtual EquipmentSet EquipmentSet { get; set; }
    }
}
