using DevShop.Identity.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevShop.Identity.Infrastructure.Mappings;

public class ApplicationUserTokenMapping : IEntityTypeConfiguration<ApplicationUserToken>
{
    public void Configure(EntityTypeBuilder<ApplicationUserToken> builder)
    {
        builder.ToTable("AuthUserToken");
        builder.HasKey(x => new { x.UserId, x.LoginProvider, x.Name });

        builder.Property(x => x.UserId).HasMaxLength(450).IsRequired();
        builder.Property(x => x.LoginProvider).HasMaxLength(450).IsRequired();
        builder.Property(x => x.Name).HasMaxLength(450).IsRequired();
        builder.Property(x => x.Value).HasMaxLength(8000);
    }
}