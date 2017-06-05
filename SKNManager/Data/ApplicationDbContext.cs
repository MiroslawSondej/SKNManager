using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SKNManager.Models;
using SKNManager.Models.EquipmentViewModels;

namespace SKNManager.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Equipment> Equipment { get; set; }
        public DbSet<EquipmentSet> EquipmentSet { get; set; }

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
            builder.Entity<Equipment>().ToTable("club_equipments");
            builder.Entity<EquipmentSet>().ToTable("club_equipmentsets");
        }

        
    }
}
