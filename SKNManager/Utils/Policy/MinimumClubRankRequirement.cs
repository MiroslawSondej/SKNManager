using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SKNManager.Utils.Identity;

namespace SKNManager.Utils.Policy
{
    public class MinimumClubRankRequirement : IAuthorizationRequirement
    {
        protected ClubRolesFactory.Role minimumRank;

        public ClubRolesFactory.Role GetMinimumRank()
        {
            return minimumRank;
        }
        public MinimumClubRankRequirement(ClubRolesFactory.Role rank)
        {
            minimumRank = rank;
        }
    }
}
