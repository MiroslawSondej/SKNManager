using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SKNManager.Models
{
    public class ProjectMembers
    {
        public ProjectMembers(){

            this.Project = new HashSet<Project>();
        }


        [Key]
        public string UserId { get; set; }

        public int ProjectId { get; set; }

        public virtual ICollection<Project> Project { get; set; }
    }


}

