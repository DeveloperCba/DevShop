using DevShop.Core.Datas.Interfaces;
using DevShop.Core.Datas.Mappings;
using DevShop.Core.DomainObjects;
using DevShop.Identity.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace DevShop.Identity.Infrastructure.Context;

public class LogDbContext : DbContext, IUnitOfWork
{

    public DbSet<LogError> LogErrors { get; set; }
    public DbSet<LogRequest> LogRequests { get; set; }

    public LogDbContext(DbContextOptions<LogDbContext> options) : base(options) { }
    // public LogDbContext(){ }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.ConfigureColumnTypeConvention();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //var conn = "Server=pgsql.oficinadev.kinghost.net; Database=oficinadev; Port=5432;User Id=oficinadev;Password=Estadao101322";
        //optionsBuilder.UseNpgsql(conn, x => x.MigrationsHistoryTable("_LogMigration"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new LogErrorMapping());
        modelBuilder.ApplyConfiguration(new LogRequestMapping());
        //modelBuilder.ToSnakeCaseNames();
    }

    public async Task<bool> Commit()
    {

        foreach (var entry in ChangeTracker.Entries()
                   .Where(entry => entry.Entity.GetType().GetProperty("CreatedAt") != null))
        {

            if (entry.State == EntityState.Added)
                entry.Property("CreatedAt").CurrentValue = DateTime.UtcNow;

            if (entry.State == EntityState.Modified)
                entry.Property("CreatedAt").IsModified = false;

        }
        var success = await base.SaveChangesAsync() > 0;
        return success;

    }
}