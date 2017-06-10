﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SKNManager.Models;

namespace SKNManager.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Delegation> Delegation { get; set; }
        public DbSet<DelegationCategory> DelegationCategory { get; set; }
        public DbSet<DelegationMember> DelegationMember { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>().ToTable("Identity_Users");
            builder.Entity<IdentityRole>().ToTable("Identity_Roles");
            builder.Entity<IdentityUserClaim<string>>().ToTable("Identity_UserClaims");
            builder.Entity<IdentityUserRole<string>>().ToTable("Identity_UserRoles");
            builder.Entity<IdentityUserLogin<string>>().ToTable("Identity_UserLogins");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("Identity_RoleClaims");
            builder.Entity<IdentityUserToken<string>>().ToTable("Identity_UserToken");

            builder.Entity<Delegation>().ToTable("Club_Delegations");
            builder.Entity<DelegationCategory>().ToTable("Club_DelegationCategories");
            builder.Entity<DelegationMember>().ToTable("Club_DelegationMembers");
        }
    }
}
