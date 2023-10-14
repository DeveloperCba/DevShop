using DevShop.Identity.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevShop.Identity.Infrastructure.Mappings;

public class ApplicationUserLoginMapping : IEntityTypeConfiguration<ApplicationUserLogin>
{
    public void Configure(EntityTypeBuilder<ApplicationUserLogin> builder)
    {
        builder.ToTable("AuthUserLogin");
        builder.HasKey(x => new { x.LoginProvider, x.ProviderKey });
        builder.HasIndex(x => x.UserId, name: "IX_AuthUserLogin_UserId");

        builder.Property(x => x.LoginProvider).HasMaxLength(450).IsRequired();
        builder.Property(x => x.ProviderKey).HasMaxLength(450).IsRequired();
        builder.Property(x => x.ProviderDisplayName);
        builder.Property(x => x.UserId).HasMaxLength(450).IsRequired();
    }
}