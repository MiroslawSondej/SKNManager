using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SKNManager.Models.MemberViewModels
{
    public class DetailsViewModel
    {
        public ApplicationUser User { get; set; }
        public string ClubRankName { get; set; }
        // public Delegation[] Delegation { get; set; }
        // public EquipmentLoan[] Loan { get; set; } 
        // public Project[] Project { get; set; }        
    }
}
