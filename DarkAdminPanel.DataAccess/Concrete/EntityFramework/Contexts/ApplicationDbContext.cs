using DarkAdminPanel.DataAccess.Concrete.EntityFramework.Configurations;
using DarkAdminPanel.DataAccess.Concrete.EntityFramework.IdentityModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DarkAdminPanel.DataAccess.Concrete.EntityFramework.Contexts
{
    public class ApplicationDbContext : IdentityDbContext<
                                            ApplicationUser, ApplicationRole, string,
                                            ApplicationUserClaim, ApplicationUserRole, ApplicationUserLogin,
                                            ApplicationRoleClaim, ApplicationUserToken>
    {
        public ApplicationDbContext()
        {

        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            :base(options)
        {
        }

        public virtual DbSet<ApplicationUserToken> ApplicationUserToken { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //Startup da sql configration edilmeyibse buradan edirik
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new ApplicationUserConfiguration());
            builder.ApplyConfiguration(new ApplicationRoleConfiguration());
        }
    }
}
