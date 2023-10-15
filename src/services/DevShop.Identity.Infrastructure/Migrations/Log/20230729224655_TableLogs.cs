#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace DevShop.Identity.Infrastructure.Migrations.Log;

/// <inheritdoc />
public partial class TableLogs : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "__LogErrors",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                Method = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                Path = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                Erro = table.Column<string>(type: "text", maxLength: 2147483647, nullable: true),
                ErroCompleto = table.Column<string>(type: "text", maxLength: 2147483647, nullable: true),
                Query = table.Column<string>(type: "text", maxLength: 2147483647, nullable: true),
                CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("Pk_LogErrors_Id", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "__LogRequests",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                Device = table.Column<string>(type: "character varying(600)", maxLength: 600, nullable: true),
                Host = table.Column<string>(type: "character varying(600)", maxLength: 600, nullable: true),
                Method = table.Column<string>(type: "character varying(600)", maxLength: 600, nullable: true),
                Path = table.Column<string>(type: "character varying(600)", maxLength: 600, nullable: true),
                Url = table.Column<string>(type: "character varying(600)", maxLength: 600, nullable: true),
                Header = table.Column<string>(type: "text", maxLength: 2147483647, nullable: true),
                Body = table.Column<string>(type: "text", maxLength: 2147483647, nullable: true),
                Query = table.Column<string>(type: "text", maxLength: 2147483647, nullable: true),
                Ip = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                StatusCode = table.Column<int>(type: "integer", nullable: false),
                ExecutionTime = table.Column<TimeSpan>(type: "interval", nullable: false),
                Response = table.Column<string>(type: "text", nullable: true),
                CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("Pk_LogRequests_Id", x => x.Id);
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "__LogErrors");

        migrationBuilder.DropTable(
            name: "__LogRequests");
    }
}