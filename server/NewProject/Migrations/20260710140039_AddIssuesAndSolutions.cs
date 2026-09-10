using Microsoft.EntityFrameworkCore.Migrations;

namespace NewProject.Migrations
{
    public partial class AddIssuesAndSolutions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Issues",
                columns: table => new
                {
                    IssueId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Issues", x => x.IssueId);
                });

            migrationBuilder.CreateTable(
                name: "Solutions",
                columns: table => new
                {
                    SolutionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IssueId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Solutions", x => x.SolutionId);
                    table.ForeignKey(
                        name: "FK_Solutions_Issues_IssueId",
                        column: x => x.IssueId,
                        principalTable: "Issues",
                        principalColumn: "IssueId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Issues",
                columns: new[] { "IssueId", "IsActive", "Name" },
                values: new object[,]
                {
                    { 1, true, "Online Account" },
                    { 2, true, "Payment" },
                    { 3, true, "Credit Score" },
                    { 4, true, "Loan Inquiry" }
                });

            migrationBuilder.InsertData(
                table: "Solutions",
                columns: new[] { "SolutionId", "IsActive", "IssueId", "Name" },
                values: new object[,]
                {
                    { 1, true, 1, "Reset Password" },
                    { 2, true, 1, "Unlock Account" },
                    { 3, true, 1, "Update Email" },
                    { 4, true, 2, "Change Payment Method" },
                    { 5, true, 2, "Payment Extension" },
                    { 6, true, 2, "Payment Reversal" },
                    { 7, true, 3, "Request Credit Report" },
                    { 8, true, 3, "Dispute Credit Score" },
                    { 9, true, 4, "Payoff Quote" },
                    { 10, true, 4, "Refinance Information" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Solutions_IssueId",
                table: "Solutions",
                column: "IssueId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Solutions");

            migrationBuilder.DropTable(
                name: "Issues");
        }
    }
}
