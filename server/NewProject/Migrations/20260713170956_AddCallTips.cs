using Microsoft.EntityFrameworkCore.Migrations;

namespace NewProject.Migrations
{
    public partial class AddCallTips : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CallTips",
                columns: table => new
                {
                    TipId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IssueId = table.Column<int>(type: "int", nullable: false),
                    Tip = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CallTips", x => x.TipId);
                    table.ForeignKey(
                        name: "FK_CallTips_Issues_IssueId",
                        column: x => x.IssueId,
                        principalTable: "Issues",
                        principalColumn: "IssueId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "CallTips",
                columns: new[] { "TipId", "IssueId", "SortOrder", "Tip" },
                values: new object[,]
                {
                    { 1, 1, 1, "Verify the customer's identity before making any account changes — ask for full name, SSN last 4, and date of birth." },
                    { 2, 1, 2, "If the customer is locked out, confirm whether they are using the correct username/email before resetting." },
                    { 3, 1, 3, "For password resets, remind the customer the link expires in 24 hours and to check their spam folder." },
                    { 4, 1, 4, "If the customer reports unauthorized access, escalate immediately to a supervisor." },
                    { 5, 2, 1, "Confirm the payment amount and due date before processing any changes." },
                    { 6, 2, 2, "If a payment was returned, ask the customer to verify their bank account and routing number." },
                    { 7, 2, 3, "Payment extensions must be approved by a supervisor — do not promise an extension without approval." },
                    { 8, 2, 4, "Always confirm the customer's preferred payment method and check if it is on file." },
                    { 9, 3, 1, "Explain that we report to credit bureaus monthly and changes may take 30-60 days to reflect." },
                    { 10, 3, 2, "If the customer disputes a credit report item, direct them to dispute through the credit bureau directly." },
                    { 11, 3, 3, "Do not make promises about credit score improvements — explain factors that generally affect scores." },
                    { 12, 3, 4, "If requesting a credit report, confirm the customer's mailing address is current before sending." },
                    { 13, 4, 1, "Verify the customer's current loan balance and payoff amount before discussing refinance options." },
                    { 14, 4, 2, "For payoff quotes, remind the customer the quote is valid for 10 days and interest accrues daily." },
                    { 15, 4, 3, "Refinance inquiries should be referred to the branch manager — do not quote rates directly." },
                    { 16, 4, 4, "If the account is charged off, do not discuss settlement options without supervisor approval." }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CallTips_IssueId",
                table: "CallTips",
                column: "IssueId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CallTips");
        }
    }
}
