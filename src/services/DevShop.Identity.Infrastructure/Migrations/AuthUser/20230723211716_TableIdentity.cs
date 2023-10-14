#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace DevShop.Identity.Infrastructure.Migrations.AuthUser;

/// <inheritdoc />
public partial class TableIdentity : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "AuthRole",
            columns: table => new
            {
                Id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "character varying(8000)", maxLength: 8000, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AuthRole", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "AuthUser",
            columns: table => new
            {
                Id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                Document = table.Column<string>(type: "character varying(14)", maxLength: 14, nullable: false),
                Street = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                Complement = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                Number = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                ZipCode = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                Neighborhood = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                City = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                State = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                PasswordHash = table.Column<string>(type: "character varying(8000)", maxLength: 8000, nullable: true),
                SecurityStamp = table.Column<string>(type: "character varying(8000)", maxLength: 8000, nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "character varying(8000)", maxLength: 8000, nullable: true),
                PhoneNumber = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: true),
                PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AuthUser", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "RefreshTokens",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                UserName = table.Column<string>(type: "text", nullable: false),
                Token = table.Column<Guid>(type: "uuid", nullable: false),
                ExpirationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_RefreshTokens", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "AuthRoleClaims",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                RoleId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                ClaimType = table.Column<string>(type: "character varying(8000)", maxLength: 8000, nullable: false),
                ClaimValue = table.Column<string>(type: "character varying(8000)", maxLength: 8000, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AuthRoleClaims", x => x.Id);
                table.ForeignKey(
                    name: "FK_AuthRoleClaims_AuthRole_RoleId",
                    column: x => x.RoleId,
                    principalTable: "AuthRole",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "AuthUserClaim",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                UserId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                ClaimType = table.Column<string>(type: "character varying(8000)", maxLength: 8000, nullable: true),
                ClaimValue = table.Column<string>(type: "character varying(8000)", maxLength: 8000, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AuthUserClaim", x => x.Id);
                table.ForeignKey(
                    name: "FK_AuthUserClaim_AuthUser_UserId",
                    column: x => x.UserId,
                    principalTable: "AuthUser",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "AuthUserLogin",
            columns: table => new
            {
                LoginProvider = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                ProviderKey = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                UserId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AuthUserLogin", x => new { x.LoginProvider, x.ProviderKey });
                table.ForeignKey(
                    name: "FK_AuthUserLogin_AuthUser_UserId",
                    column: x => x.UserId,
                    principalTable: "AuthUser",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "AuthUserRole",
            columns: table => new
            {
                UserId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                RoleId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AuthUserRole", x => new { x.UserId, x.RoleId });
                table.ForeignKey(
                    name: "FK_AuthUserRole_AuthRole_RoleId",
                    column: x => x.RoleId,
                    principalTable: "AuthRole",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_AuthUserRole_AuthUser_UserId",
                    column: x => x.UserId,
                    principalTable: "AuthUser",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "AuthUserToken",
            columns: table => new
            {
                UserId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                LoginProvider = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                Name = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                Value = table.Column<string>(type: "character varying(8000)", maxLength: 8000, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AuthUserToken", x => new { x.UserId, x.LoginProvider, x.Name });
                table.ForeignKey(
                    name: "FK_AuthUserToken_AuthUser_UserId",
                    column: x => x.UserId,
                    principalTable: "AuthUser",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "RoleNameIndex",
            table: "AuthRole",
            column: "NormalizedName",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_AuthRoleClaims_RoleId",
            table: "AuthRoleClaims",
            column: "RoleId");

        migrationBuilder.CreateIndex(
            name: "EmailIndex",
            table: "AuthUser",
            column: "NormalizedEmail");

        migrationBuilder.CreateIndex(
            name: "UserNameIndex",
            table: "AuthUser",
            column: "NormalizedUserName",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_AuthUserClaim_UserId",
            table: "AuthUserClaim",
            column: "UserId");

        migrationBuilder.CreateIndex(
            name: "IX_AuthUserLogin_UserId",
            table: "AuthUserLogin",
            column: "UserId");

        migrationBuilder.CreateIndex(
            name: "IX_AuthUserRole_RoleId",
            table: "AuthUserRole",
            column: "RoleId");

        migrationBuilder.CreateIndex(
            name: "IX_AuthUserRole_UserId",
            table: "AuthUserRole",
            column: "UserId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "AuthRoleClaims");

        migrationBuilder.DropTable(
            name: "AuthUserClaim");

        migrationBuilder.DropTable(
            name: "AuthUserLogin");

        migrationBuilder.DropTable(
            name: "AuthUserRole");

        migrationBuilder.DropTable(
            name: "AuthUserToken");

        migrationBuilder.DropTable(
            name: "RefreshTokens");

        migrationBuilder.DropTable(
            name: "AuthRole");

        migrationBuilder.DropTable(
            name: "AuthUser");
    }
}