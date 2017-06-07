using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace SKNManager.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            this.Project = new HashSet<Project>();
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public HashSet<Project> Project { get; set; }

        //public virtual Project Project { get; set; }
    }
}
