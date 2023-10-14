using DevShop.Core.Datas.Mappings;
using DevShop.Core.DomainObjects;
using Microsoft.EntityFrameworkCore;

namespace DevShop.Identity.Infrastructure.Context;

public class LogDbContext : DbContext
{

    public DbSet<LogError> LogErrors { get; set; }
    public DbSet<LogRequest> LogRequests { get; set; }

    public LogDbContext(DbContextOptions<LogDbContext> options) : base(options){}
    // public LogDbContext(){ }
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

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries().Where(entry => entry.Entity.GetType().GetProperty("DataCadastro") != null))
        {

            if (entry.State == EntityState.Modified)
                entry.Property("UpdatedAt").CurrentValue = DateTime.UtcNow;
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}