using DevShop.Core.DomainObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevShop.Core.Datas.Mappings;

public class LogErrorMapping : IEntityTypeConfiguration<LogError>
{
    public void Configure(EntityTypeBuilder<LogError> builder)
    {
        builder.ToTable("__LogErrors");
        builder.HasKey(t => t.Id).HasName("Pk_LogErrors_Id");

        builder.Property(t => t.Method).HasMaxLength(100);
        builder.Property(t => t.Path).HasMaxLength(200);
        builder.Property(t => t.Erro).HasMaxLength(int.MaxValue);
        builder.Property(t => t.ErroCompleto).HasMaxLength(int.MaxValue);
        builder.Property(t => t.Query).HasMaxLength(int.MaxValue);
    }
}