using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NewProject.Migrations
{
    public partial class ConsolidateDatabases : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BranchLocations",
                columns: table => new
                {
                    BranchId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    BranchName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    State = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    ZipCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BranchLocations", x => x.BranchId);
                });

            migrationBuilder.CreateTable(
                name: "LoanAccounts",
                columns: table => new
                {
                    LoanId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    LoanClass = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    AccountNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    StatusCodes = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TaxId = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: true),
                    StreetAddress = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    MobilePhone = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    HomePhone = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    OriginationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LoanAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    NetProceeds = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PrincipalBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PayoffAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DelinquentAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ChargeOffDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FirstPaymentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ScheduledPayment = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AmountDue = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    NextDueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NextDueAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    LastDueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastPaymentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastPaymentAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    LoanReference = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoanAccounts", x => x.LoanId);
                });

            migrationBuilder.CreateTable(
                name: "PaymentTransactions",
                columns: table => new
                {
                    TransactionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoanReference = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TransactionCode = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    ConfirmationNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    TransactionAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PrincipalApplied = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaidThroughDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FeesApplied = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentTransactions", x => x.TransactionId);
                });

            migrationBuilder.InsertData(
                table: "BranchLocations",
                columns: new[] { "BranchId", "Address", "BranchCode", "BranchName", "City", "IsActive", "Phone", "State", "ZipCode" },
                values: new object[,]
                {
                    { 1, "123 Main Street", "0101", "Downtown Financial Center", "Springfield", true, "2175550101", "IL", "62701" },
                    { 2, "456 West Oak Avenue", "0202", "Westside Lending Branch", "Riverside", true, "9515550202", "CA", "92501" },
                    { 3, "789 North Park Blvd", "0303", "Northgate Financial", "Portland", true, "5035550303", "OR", "97201" },
                    { 4, "321 East Commerce St", "0404", "Eastside Loan Center", "Nashville", true, "6155550404", "TN", "37201" },
                    { 5, "654 South Elm Street", "0505", "Southpark Branch", "Charlotte", true, "7045550505", "NC", "28201" },
                    { 6, "987 Lakewood Drive", "0606", "Lakewood Financial Center", "Denver", true, "3035550606", "CO", "80201" },
                    { 7, "147 Midtown Plaza", "0707", "Midtown Lending Office", "Atlanta", true, "4045550707", "GA", "30301" },
                    { 8, "258 River Road", "0808", "Riverside Branch", "Columbus", true, "6145550808", "OH", "43201" },
                    { 9, "369 Highland Avenue", "0909", "Highland Park Office", "Birmingham", true, "2055550909", "AL", "35201" },
                    { 10, "741 Greenfield Parkway", "1010", "Greenfield Lending Center", "Greenville", true, "8645551010", "SC", "29601" }
                });

            migrationBuilder.InsertData(
                table: "LoanAccounts",
                columns: new[] { "LoanId", "AccountNumber", "AmountDue", "BranchCode", "ChargeOffDate", "DelinquentAmount", "FirstName", "FirstPaymentDate", "HomePhone", "LastDueDate", "LastName", "LastPaymentAmount", "LastPaymentDate", "LoanAmount", "LoanClass", "LoanReference", "MobilePhone", "NetProceeds", "NextDueAmount", "NextDueDate", "OriginationDate", "PayoffAmount", "PrincipalBalance", "ScheduledPayment", "StatusCodes", "StreetAddress", "TaxId" },
                values: new object[,]
                {
                    { 5, "40002", 195.00m, "0202", null, 0m, "MARIA", new DateTime(2023, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(2025, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "GARCIA", 195.00m, new DateTime(2025, 1, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), 4200.00m, "1", "0202-40002", "9515554567", 3500.00m, 195.00m, new DateTime(2025, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3310.00m, 3200.00m, 195.00m, "PB", "19 SUNSET BLVD, RIVERSIDE CA 92501", "456789012" },
                    { 4, "40001", 0m, "0202", null, 0m, "MARIA", new DateTime(2020, 6, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(2021, 5, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "GARCIA", 95.00m, new DateTime(2021, 5, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 1500.00m, "1", "0202-40001", "9515554567", 1200.00m, null, null, new DateTime(2020, 5, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 0m, 95.00m, "PO", "19 SUNSET BLVD, RIVERSIDE CA 92501", "456789012" },
                    { 1, "10001", 175.00m, "0404", null, 0m, "JOHN", new DateTime(2023, 7, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "6155554321", new DateTime(2025, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "MILLER", 175.00m, new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 3500.00m, "1", "0404-10001", "6155551234", 2800.00m, 175.00m, new DateTime(2025, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 1920.00m, 1850.00m, 175.00m, "PB", "415 MAPLE STREET, NASHVILLE TN 37201", "123456789" },
                    { 2, "20001", 890.00m, "0505", new DateTime(2023, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 890.00m, "SARAH", new DateTime(2021, 4, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(2023, 6, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "JOHNSON", 210.00m, new DateTime(2023, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 5000.00m, "1", "0505-20001", "7045552345", 4200.00m, null, null, new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 3450.00m, 3100.00m, 210.00m, "CO", "822 OAK LANE, CHARLOTTE NC 28201", "234567890" },
                    { 3, "30001", 0m, "0707", null, 0m, "ROBERT", new DateTime(2022, 2, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "4045556543", new DateTime(2024, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "DAVIS", 120.00m, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 2000.00m, "1", "0707-30001", "4045553456", 1650.00m, null, null, new DateTime(2022, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 0m, 120.00m, "PO", "331 PEACHTREE RD, ATLANTA GA 30301", "345678901" }
                });

            migrationBuilder.InsertData(
                table: "PaymentTransactions",
                columns: new[] { "TransactionId", "ConfirmationNumber", "FeesApplied", "LoanReference", "PaidThroughDate", "PrincipalApplied", "TransactionAmount", "TransactionCode", "TransactionDate" },
                values: new object[,]
                {
                    { 10, "30011001", 0m, "0707-30001", new DateTime(2024, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 119.00m, 120.00m, "PO", new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 15, "40021001", 0m, "0202-40002", new DateTime(2025, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 155.00m, 195.00m, "PA", new DateTime(2025, 1, 28, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 14, "40011002", 0m, "0202-40001", new DateTime(2021, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), 92.00m, 95.00m, "PA", new DateTime(2021, 4, 5, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 13, "40011001", 0m, "0202-40001", new DateTime(2021, 5, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), 94.00m, 95.00m, "PO", new DateTime(2021, 5, 3, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 12, "30011003", 0m, "0707-30001", new DateTime(2023, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 113.00m, 120.00m, "PA", new DateTime(2023, 11, 13, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 11, "30011002", 0m, "0707-30001", new DateTime(2023, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 116.00m, 120.00m, "PA", new DateTime(2023, 12, 18, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 9, "20011004", 25.00m, "0505-20001", null, 0m, 25.00m, "LF", new DateTime(2023, 1, 9, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 1, "10011001", 0m, "0404-10001", new DateTime(2025, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 142.00m, 175.00m, "PA", new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 7, "20011002", 0m, "0505-20001", new DateTime(2023, 4, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 158.00m, 210.00m, "PA", new DateTime(2023, 4, 3, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 6, "20011001", 0m, "0505-20001", new DateTime(2023, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 160.00m, 210.00m, "PA", new DateTime(2023, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, "10011005", 0m, "0404-10001", new DateTime(2024, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 134.00m, 175.00m, "PA", new DateTime(2024, 10, 14, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, "10011004", 0m, "0404-10001", new DateTime(2024, 11, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 136.00m, 175.00m, "PA", new DateTime(2024, 11, 11, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, "10011003", 0m, "0404-10001", new DateTime(2024, 12, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 138.00m, 175.00m, "PA", new DateTime(2024, 12, 9, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, "10011002", 0m, "0404-10001", new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 140.00m, 175.00m, "PA", new DateTime(2025, 1, 12, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 16, "40021002", 0m, "0202-40002", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 153.00m, 195.00m, "PA", new DateTime(2024, 12, 30, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 8, "20011003", 0m, "0505-20001", new DateTime(2023, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 156.00m, 210.00m, "PA", new DateTime(2023, 3, 6, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 17, "40021003", 0m, "0202-40002", new DateTime(2024, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 151.00m, 195.00m, "PA", new DateTime(2024, 11, 27, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BranchLocations");

            migrationBuilder.DropTable(
                name: "LoanAccounts");

            migrationBuilder.DropTable(
                name: "PaymentTransactions");
        }
    }
}
