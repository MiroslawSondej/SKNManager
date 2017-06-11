using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SKNManager.Models.EquipmentViewModels
{
    public class EquipmentSet
    {
        public EquipmentSet()
        {
            this.Equipment = new HashSet<Equipment>();
        }
        
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Podaj nazwe zestawu")]
        [Display(Name = "Nazwa")]
        public string Name { get; set; }

        public virtual ICollection<Equipment> Equipment { get; set; }
    }
}
