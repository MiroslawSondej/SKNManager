using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SKNManager.Models
{
    public class DelegationCategory
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Nazwa")]
        [Required(ErrorMessage = "Pole jest wymagane")]
        public string Name { get; set; }
    }
}
