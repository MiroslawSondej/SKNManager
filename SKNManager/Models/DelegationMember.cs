using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SKNManager.Models
{
    public class DelegationMember
    {
        [Key]
        public int Id { get; set; }

        public int DelegationId { get; set; }
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        [ForeignKey("DelegationId")]
        public Delegation Delegation { get; set; }
    }
}
