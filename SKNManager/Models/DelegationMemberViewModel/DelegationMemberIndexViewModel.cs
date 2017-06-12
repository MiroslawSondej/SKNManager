using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SKNManager.Models.DelegationMemberViewModel
{
    public class DelegationMemberIndexViewModel
    {
        public int DelegationId { get; set; }
        public string DelegationName { get; set; }
        public ApplicationUser[] User { get; set; }
        public DelegationMember[] Member { get; set; }
    }
}
