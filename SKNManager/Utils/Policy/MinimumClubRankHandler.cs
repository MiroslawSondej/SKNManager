using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using SKNManager.Utils.Identity;

namespace SKNManager.Utils.Policy
{
    public class MinimumClubRankHandler : AuthorizationHandler<MinimumClubRankRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumClubRankRequirement requirement)
        {
            if(context.User.IsInRole("Administrator"))
            {
                context.Succeed(requirement);
            }

            if (!context.User.HasClaim(c => c.Type == "ClubRank"))
            {
                return Task.CompletedTask;
            }

            try
            {
                string rankName = context.User.FindFirst(c => c.Type == "ClubRank").Value;
                if (requirement.GetMinimumRank() == ClubRolesFactory.GetId(rankName) || (int)requirement.GetMinimumRank() >= (int)ClubRolesFactory.GetId(rankName))
                {
                    context.Succeed(requirement);
                }
            } catch { }

            return Task.CompletedTask;
        }
    }
}
