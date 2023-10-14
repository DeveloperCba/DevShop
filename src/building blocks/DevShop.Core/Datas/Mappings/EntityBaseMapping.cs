using DevShop.Core.DomainObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevShop.Core.Datas.Mapping;

public abstract class EntityBaseMapping<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : Entity
{
    private readonly string _tableName;
    protected int decimalInit = 19;
    protected int decimalEnd = 2;

    protected EntityBaseMapping(string tableName = "") => _tableName = tableName;

    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        if (!string.IsNullOrEmpty(_tableName))
            builder.ToTable(_tableName);

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasMaxLength(36);
    }
}