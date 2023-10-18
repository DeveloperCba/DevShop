using DevShop.Core.Datas.Interfaces;
using DevShop.Identity.Domain.Models;
using DevShop.Identity.Infrastructure.Extensions;
using DevShop.Identity.Infrastructure.Mappings;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DevShop.Identity.Infrastructure.Context;

public class ApplicationIdentityDbContext : IdentityDbContext
<
    ApplicationUser,
    ApplicationRole,
    string,
    ApplicationUserClaim,
    ApplicationUserRole,
    ApplicationUserLogin,
    ApplicationRoleClaim,
    ApplicationUserToken
>, IUnitOfWork
{
    //public DbSet<SecurityKeyWithPrivate> SecurityKeys { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public virtual DbSet<ApplicationUser> ApplicationUser { get; set; }
    public virtual DbSet<ApplicationRole> ApplicationRole { get; set; }
    public virtual DbSet<ApplicationUserRole> ApplicationUserRole { get; set; }
    public virtual DbSet<ApplicationUserClaim> ApplicationUserClaim { get; set; }
    public virtual DbSet<ApplicationUserLogin> ApplicationUserLogin { get; set; }
    public virtual DbSet<ApplicationRoleClaim> ApplicationRoleClaim { get; set; }
    public virtual DbSet<ApplicationUserToken> ApplicationUserToken { get; set; }

    //public ApplicationIdentityDbContext(){ }

    public ApplicationIdentityDbContext(DbContextOptions<ApplicationIdentityDbContext> options) : base(options)
    { }

    //protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    //{
    //    configurationBuilder.ConfigureColumnTypeConvention();
    //}


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //var conn = "Server=pgsql.oficinadev.kinghost.net; Database=oficinadev; Port=5432;User Id=oficinadev;Password=Estadao101322";
        //optionsBuilder.UseNpgsql(conn, x => x.MigrationsHistoryTable("_AuthMigration"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ApplicationRoleClaimMapping());
        modelBuilder.ApplyConfiguration(new ApplicationRoleMapping());
        modelBuilder.ApplyConfiguration(new ApplicationUserClaimMapping());
        modelBuilder.ApplyConfiguration(new ApplicationUserLoginMapping());
        modelBuilder.ApplyConfiguration(new ApplicationUserMapping());
        modelBuilder.ApplyConfiguration(new ApplicationUserRoleMapping());
        modelBuilder.ApplyConfiguration(new ApplicationUserTokenMapping());

        //modelBuilder.ApplyConfigurationsFromAssembly(assembly: Assembly.GetExecutingAssembly());
    }

    public async Task<bool> Commit()
    {

        foreach (var entry in ChangeTracker.Entries()
                     .Where(entry => entry.Entity.GetType().GetProperty("CreatedAt") != null))
        {
            if (entry.State == EntityState.Added)
                entry.Property("CreatedAt").CurrentValue = DateTime.Now;

            if (entry.State == EntityState.Modified)
                entry.Property("CreatedAt").IsModified = false;

        }
        var success = await base.SaveChangesAsync() > 0;
        return success;

    }
}