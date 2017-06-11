using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SKNManager.Models
{
    public class Project
    {

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Proszę podać nazwę projektu")]
        [Display(Name = "Nazwa projektu")]
        public string Name { get; set; }

        [Display(Name = "Opis")]
        public string Description { get; set; }

        [Display(Name = "Rozpoczęcie projektu")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? StartDate { get; set; }

        [Display(Name = "Zakończenie projektu")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? EndDate { get; set; }

        [Display(Name = "Opiekun projektu")]
        public string ProjectLeaderId { get; set; }

        [ForeignKey("ProjectLeaderId")]
        [Display(Name="Opiekun projektu")]
        public virtual ApplicationUser ApplicationUser { get; set; }

        public List<ProjectMembers> ProjectMembers { get; set; }
    }
}
