using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APIBanco.Migrations
{
    /// <inheritdoc />
    public partial class FixCreditCard : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CardTransactions_CreditCards_CreditCardId",
                table: "CardTransactions");

            migrationBuilder.AlterColumn<int>(
                name: "CreditCardId",
                table: "CardTransactions",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_CardTransactions_CreditCards_CreditCardId",
                table: "CardTransactions",
                column: "CreditCardId",
                principalTable: "CreditCards",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CardTransactions_CreditCards_CreditCardId",
                table: "CardTransactions");

            migrationBuilder.AlterColumn<int>(
                name: "CreditCardId",
                table: "CardTransactions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CardTransactions_CreditCards_CreditCardId",
                table: "CardTransactions",
                column: "CreditCardId",
                principalTable: "CreditCards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
