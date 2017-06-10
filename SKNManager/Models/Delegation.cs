using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SKNManager.Models
{
    public class Delegation
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Nazwa")]
        [Required(ErrorMessage = "Nazwa jest wymagana")]
        public string Name { get; set; }

        [Display(Name = "Data rozpoczęcia")]
        [DataType(DataType.Date, ErrorMessage = "Proszę wprowadzić poprawną datę (DD.MM.RRRR)")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Data rozpoczęcia jest wymagana")]
        public DateTime StartDate { get; set; }

        [Display(Name = "Data zakończenia")]
        [DataType(DataType.Date, ErrorMessage = "Proszę wprowadzić poprawną datę (DD.MM.RRRR)")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Data zakończenia jest wymagana")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Kategoria jest wymagana")]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public DelegationCategory Category { get; set; }

        public List<DelegationMember> Member { get; set; }
    }
}
