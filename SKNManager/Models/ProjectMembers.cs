using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SKNManager.Models
{
    public class ProjectMembers
    {

        [Key]
        public int Id { get; set; }

        [Column("UserId")]
        [Display(Name = "Użytkownik")]
        public string UserId { get; set; }

        
        [Column("ProjectId")]
        [Display(Name = "Nazwa projektu")]
        public int ProjectId { get; set; }

        [ForeignKey("ProjectId")]
        public virtual Project Project { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }


    }


}

