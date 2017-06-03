using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using SKNManager.Data;
using SKNManager.Models;
using SKNManager.Services;
using SKNManager.Utils.Identity;
using SKNManager.Utils.Policy;
using Microsoft.AspNetCore.Authorization;

namespace SKNManager
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see https://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets<Startup>();
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySql(Configuration.GetConnectionString("MySQLConnection")));


            // If you will ever attempting to change TokenProvider, don't forget to change it in "Account/Invite" too
            services.AddIdentity<ApplicationUser, IdentityRole>(config => {
                config.SignIn.RequireConfirmedEmail = true;
            }).AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders()
            .AddErrorDescriber<CustomIdentityErrorDescriber>();

            // Add authorization rules
            services.AddAuthorization(options =>
            {
                options.AddPolicy("SupervisorClubRank", policy => policy.Requirements.Add(new MinimumClubRankRequirement(ClubRolesFactory.Role.SUPERVISOR)));
                options.AddPolicy("PresidentClubRank", policy => policy.Requirements.Add(new MinimumClubRankRequirement(ClubRolesFactory.Role.PRESIDENT)));
                options.AddPolicy("VicePresidentClubRank", policy => policy.Requirements.Add(new MinimumClubRankRequirement(ClubRolesFactory.Role.VICE_PRESIDENT)));
                options.AddPolicy("SecretaryClubRank", policy => policy.Requirements.Add(new MinimumClubRankRequirement(ClubRolesFactory.Role.SECRETARY)));
                options.AddPolicy("TreasurerClubRank", policy => policy.Requirements.Add(new MinimumClubRankRequirement(ClubRolesFactory.Role.TREASURER)));
                options.AddPolicy("PhotographerClubRank", policy => policy.Requirements.Add(new MinimumClubRankRequirement(ClubRolesFactory.Role.PHOTOGRAPHER)));
                options.AddPolicy("MemberClubRank", policy => policy.Requirements.Add(new MinimumClubRankRequirement(ClubRolesFactory.Role.MEMBER)));
            });
            services.AddSingleton<IAuthorizationHandler, MinimumClubRankHandler>();

            services.AddMvc();

            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseIdentity();

            // Add external authentication middleware below. To configure them please see https://go.microsoft.com/fwlink/?LinkID=532715

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
