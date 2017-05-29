using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace SKNManager.Models.MemberViewModels
{
    public class IndexMemberViewModel
    {
        public Tuple<ApplicationUser, string>[] UserTuple { get; set; }
    }
}
