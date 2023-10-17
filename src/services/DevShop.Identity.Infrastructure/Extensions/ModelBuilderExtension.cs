using Microsoft.EntityFrameworkCore;

namespace DevShop.Identity.Infrastructure.Extensions;

public static class ModelBuilderExtension
{

    public static ModelConfigurationBuilder ConfigureColumnTypeConvention(this ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<string>().AreUnicode(false);
        configurationBuilder.Properties<string>().AreUnicode(false).HaveColumnType("character varying");

        return configurationBuilder;
    }

    public static ModelBuilder ConfigureEntityRelationship(this ModelBuilder modelBuilder, DeleteBehavior behavior)
    {
        foreach (var relationship in modelBuilder.Model.GetEntityTypes()
                     .SelectMany(e => e.GetForeignKeys()))
            relationship.DeleteBehavior = behavior;

        return modelBuilder;
    }

}