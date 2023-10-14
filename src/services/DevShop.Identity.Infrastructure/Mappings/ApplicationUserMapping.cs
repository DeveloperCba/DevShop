using DevShop.Identity.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevShop.Identity.Infrastructure.Mappings;

public class ApplicationUserMapping : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.ToTable("AuthUser");
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.NormalizedEmail, name: "EmailIndex");
        builder.HasIndex(x => x.NormalizedUserName, name: "UserNameIndex").IsUnique();

        builder.Property(x => x.Id).HasMaxLength(450).IsRequired();
        builder.Property(x => x.UserName).HasMaxLength(256);
        builder.Property(x => x.NormalizedUserName).HasMaxLength(256);
        builder.Property(x => x.Email).HasMaxLength(256);
        builder.Property(x => x.NormalizedEmail).HasMaxLength(256);
        builder.Property(x => x.EmailConfirmed).IsRequired();
        builder.Property(x => x.PasswordHash).HasMaxLength(8000);
        builder.Property(x => x.SecurityStamp).HasMaxLength(8000);
        builder.Property(x => x.ConcurrencyStamp).HasMaxLength(8000);
        builder.Property(x => x.PhoneNumber).HasMaxLength(16);
        builder.Property(x => x.PhoneNumberConfirmed).IsRequired();
        builder.Property(x => x.Name).HasMaxLength(256);
        builder.Property(x => x.Document).HasMaxLength(14);
        builder.Property(x => x.Street).HasMaxLength(200);
        builder.Property(x => x.Number).HasMaxLength(30);
        builder.Property(x => x.ZipCode).HasMaxLength(8);
        builder.Property(x => x.Neighborhood).HasMaxLength(200);
        builder.Property(x => x.Complement).HasMaxLength(200);
        builder.Property(x => x.City).HasMaxLength(200);
        builder.Property(x => x.State).HasMaxLength(2);

        builder.Property(x => x.TwoFactorEnabled).IsRequired();
        builder.Property(x => x.LockoutEnd);
        builder.Property(x => x.LockoutEnabled).IsRequired();
        builder.Property(x => x.AccessFailedCount).IsRequired();

        // Each User can have many UserClaims
        builder.HasMany(e => e.Claims)
            .WithOne(e => e.User)
            .HasForeignKey(uc => uc.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        // Each User can have many UserLogins
        builder.HasMany(e => e.Logins)
            .WithOne(e => e.User)
            .HasForeignKey(ul => ul.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        // Each User can have many UserTokens
        builder.HasMany(e => e.Tokens)
            .WithOne(e => e.User)
            .HasForeignKey(ut => ut.UserId)
            .IsRequired();

        // Each User can have many entries in the UserRole join table
        builder.HasMany(e => e.UserRoles)
            .WithOne(e => e.User)
            .HasForeignKey(ur => ur.UserId)
            .IsRequired();
    }
}