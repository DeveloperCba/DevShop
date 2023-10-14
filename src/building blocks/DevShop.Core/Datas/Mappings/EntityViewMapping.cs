using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevShop.Core.Datas.Mapping;

public abstract class EntityViewMapping<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : class
{
    private readonly string _tableName;

    protected EntityViewMapping(string tableName = "") => _tableName = tableName;

    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        if (!string.IsNullOrEmpty(_tableName))
            builder.ToView(_tableName);

        builder.HasNoKey();
    }
}