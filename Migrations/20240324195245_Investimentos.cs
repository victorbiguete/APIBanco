using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APIBanco.Migrations
{
    /// <inheritdoc />
    public partial class Investimentos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Investments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    MaintenanceFee = table.Column<decimal>(type: "TEXT", nullable: false),
                    MinContribution = table.Column<decimal>(type: "TEXT", nullable: false),
                    MinRedemptionTerm = table.Column<DateTime>(type: "TEXT", nullable: false),
                    MaxRedemptionTerm = table.Column<DateTime>(type: "TEXT", nullable: false),
                    MinRedemptionValue = table.Column<decimal>(type: "TEXT", nullable: false),
                    RateYield = table.Column<double>(type: "REAL", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    BankAccountId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Investments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Investments_BankAccounts_BankAccountId",
                        column: x => x.BankAccountId,
                        principalTable: "BankAccounts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Investments_BankAccountId",
                table: "Investments",
                column: "BankAccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Investments");
        }
    }
}
