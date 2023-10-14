using DevShop.Core.DomainObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevShop.Core.Datas.Mappings;

public class LogRequestMapping : IEntityTypeConfiguration<LogRequest>
{
    public void Configure(EntityTypeBuilder<LogRequest> builder)
    {
        builder.ToTable("__LogRequests");
        builder.HasKey(t => t.Id).HasName("Pk_LogRequests_Id");
        builder.Property(t => t.Device).HasMaxLength(600);
        builder.Property(t => t.Host).HasMaxLength(600);
        builder.Property(t => t.Method).HasMaxLength(600);
        builder.Property(t => t.Path).HasMaxLength(600);
        builder.Property(t => t.Url).HasMaxLength(600);
        builder.Property(t => t.Query).HasMaxLength(int.MaxValue);
        builder.Property(t => t.Header).HasMaxLength(int.MaxValue);
        builder.Property(t => t.Body).HasMaxLength(int.MaxValue);
        builder.Property(t => t.Ip).HasMaxLength(200);
        builder.Property(t => t.StatusCode);
    }
}