using Microsoft.EntityFrameworkCore;
using IncidentManagement.Models;
using System;

namespace IncidentManagement.Data
{
    public class IncidentDbContext : DbContext
    {
        public IncidentDbContext(DbContextOptions<IncidentDbContext> options) : base(options) { }

        public DbSet<Incident> Incidents => Set<Incident>();
        public DbSet<IncidentStatTracking> IncidentStatTrackings => Set<IncidentStatTracking>();
        public DbSet<IncidentHistory> IncidentHistories => Set<IncidentHistory>();
        public DbSet<Issue> Issues => Set<Issue>();
        public DbSet<Solution> Solutions => Set<Solution>();
        public DbSet<IncidentNote> IncidentNotes => Set<IncidentNote>();
        public DbSet<CallTip> CallTips => Set<CallTip>();
        public DbSet<LoanAccount> LoanAccounts => Set<LoanAccount>();
        public DbSet<PaymentTransaction> PaymentTransactions => Set<PaymentTransaction>();
        public DbSet<BranchLocation> BranchLocations => Set<BranchLocation>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Incident>(e =>
            {
                e.HasIndex(i => i.Branch);
                e.HasIndex(i => i.SSN);
                e.HasIndex(i => i.CreatedAt);
                e.HasIndex(i => i.ClosedAt);
            });

            modelBuilder.Entity<IncidentStatTracking>(e =>
            {
                e.HasIndex(s => s.IncidentId);
                e.HasIndex(s => s.Username);

                e.HasOne(s => s.Incident)
                 .WithMany(i => i!.StatTrackings)
                 .HasForeignKey(s => s.IncidentId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<IncidentHistory>(e =>
            {
                e.HasNoKey();
                e.ToView("vw_IncidentHistory");
            });

            modelBuilder.Entity<Issue>(e =>
            {
                e.HasMany(i => i.Solutions)
                 .WithOne(s => s.Issue)
                 .HasForeignKey(s => s.IssueId)
                 .OnDelete(DeleteBehavior.Cascade);

                e.HasData(
                    new Issue { IssueId = 1, Name = "Online Account", IsActive = true },
                    new Issue { IssueId = 2, Name = "Payment", IsActive = true },
                    new Issue { IssueId = 3, Name = "Credit Score", IsActive = true },
                    new Issue { IssueId = 4, Name = "Loan Inquiry", IsActive = true }
                );
            });

            modelBuilder.Entity<Solution>(e =>
            {
                e.HasData(
                    new Solution { SolutionId = 1, IssueId = 1, Name = "Reset Password", IsActive = true },
                    new Solution { SolutionId = 2, IssueId = 1, Name = "Unlock Account", IsActive = true },
                    new Solution { SolutionId = 3, IssueId = 1, Name = "Update Email", IsActive = true },
                    new Solution { SolutionId = 4, IssueId = 2, Name = "Change Payment Method", IsActive = true },
                    new Solution { SolutionId = 5, IssueId = 2, Name = "Payment Extension", IsActive = true },
                    new Solution { SolutionId = 6, IssueId = 2, Name = "Payment Reversal", IsActive = true },
                    new Solution { SolutionId = 7, IssueId = 3, Name = "Request Credit Report", IsActive = true },
                    new Solution { SolutionId = 8, IssueId = 3, Name = "Dispute Credit Score", IsActive = true },
                    new Solution { SolutionId = 9, IssueId = 4, Name = "Payoff Quote", IsActive = true },
                    new Solution { SolutionId = 10, IssueId = 4, Name = "Refinance Information", IsActive = true }
                );
            });

            modelBuilder.Entity<CallTip>(e =>
            {
                e.HasIndex(t => t.IssueId);
                e.HasOne(t => t.Issue)
                .WithMany()
                .HasForeignKey(t => t.IssueId)
                .OnDelete(DeleteBehavior.Cascade);

                e.HasData(
                    // Online Account (IssueId: 1)
                    new CallTip { TipId = 1, IssueId = 1, Tip = "Verify the customer's identity before making any account changes — ask for full name, SSN last 4, and date of birth.", SortOrder = 1 },
                    new CallTip { TipId = 2, IssueId = 1, Tip = "If the customer is locked out, confirm whether they are using the correct username/email before resetting.", SortOrder = 2 },
                    new CallTip { TipId = 3, IssueId = 1, Tip = "For password resets, remind the customer the link expires in 24 hours and to check their spam folder.", SortOrder = 3 },
                    new CallTip { TipId = 4, IssueId = 1, Tip = "If the customer reports unauthorized access, escalate immediately to a supervisor.", SortOrder = 4 },

                    // Payment (IssueId: 2)
                    new CallTip { TipId = 5, IssueId = 2, Tip = "Confirm the payment amount and due date before processing any changes.", SortOrder = 1 },
                    new CallTip { TipId = 6, IssueId = 2, Tip = "If a payment was returned, ask the customer to verify their bank account and routing number.", SortOrder = 2 },
                    new CallTip { TipId = 7, IssueId = 2, Tip = "Payment extensions must be approved by a supervisor — do not promise an extension without approval.", SortOrder = 3 },
                    new CallTip { TipId = 8, IssueId = 2, Tip = "Always confirm the customer's preferred payment method and check if it is on file.", SortOrder = 4 },

                    // Credit Score (IssueId: 3)
                    new CallTip { TipId = 9,  IssueId = 3, Tip = "Explain that we report to credit bureaus monthly and changes may take 30-60 days to reflect.", SortOrder = 1 },
                    new CallTip { TipId = 10, IssueId = 3, Tip = "If the customer disputes a credit report item, direct them to dispute through the credit bureau directly.", SortOrder = 2 },
                    new CallTip { TipId = 11, IssueId = 3, Tip = "Do not make promises about credit score improvements — explain factors that generally affect scores.", SortOrder = 3 },
                    new CallTip { TipId = 12, IssueId = 3, Tip = "If requesting a credit report, confirm the customer's mailing address is current before sending.", SortOrder = 4 },

                    // Loan Inquiry (IssueId: 4)
                    new CallTip { TipId = 13, IssueId = 4, Tip = "Verify the customer's current loan balance and payoff amount before discussing refinance options.", SortOrder = 1 },
                    new CallTip { TipId = 14, IssueId = 4, Tip = "For payoff quotes, remind the customer the quote is valid for 10 days and interest accrues daily.", SortOrder = 2 },
                    new CallTip { TipId = 15, IssueId = 4, Tip = "Refinance inquiries should be referred to the branch manager — do not quote rates directly.", SortOrder = 3 },
                    new CallTip { TipId = 16, IssueId = 4, Tip = "If the account is charged off, do not discuss settlement options without supervisor approval.", SortOrder = 4 }
                );
            });

            modelBuilder.Entity<IncidentNote>(e =>
            {
                e.HasIndex(n => n.IncidentId);
                e.HasOne(n => n.Incident)
                 .WithMany()
                 .HasForeignKey(n => n.IncidentId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            // ── BranchLocation Seed ───────────────────────────────────────────────────
            modelBuilder.Entity<BranchLocation>().HasData(
                new BranchLocation { BranchId = 1,  BranchCode = "0101", BranchName = "Downtown Financial Center", Address = "123 Main Street",        City = "Springfield",   State = "IL", ZipCode = "62701", Phone = "2175550101", IsActive = true },
                new BranchLocation { BranchId = 2,  BranchCode = "0202", BranchName = "Westside Lending Branch",   Address = "456 West Oak Avenue",    City = "Riverside",     State = "CA", ZipCode = "92501", Phone = "9515550202", IsActive = true },
                new BranchLocation { BranchId = 3,  BranchCode = "0303", BranchName = "Northgate Financial",       Address = "789 North Park Blvd",    City = "Portland",      State = "OR", ZipCode = "97201", Phone = "5035550303", IsActive = true },
                new BranchLocation { BranchId = 4,  BranchCode = "0404", BranchName = "Eastside Loan Center",      Address = "321 East Commerce St",   City = "Nashville",     State = "TN", ZipCode = "37201", Phone = "6155550404", IsActive = true },
                new BranchLocation { BranchId = 5,  BranchCode = "0505", BranchName = "Southpark Branch",          Address = "654 South Elm Street",   City = "Charlotte",     State = "NC", ZipCode = "28201", Phone = "7045550505", IsActive = true },
                new BranchLocation { BranchId = 6,  BranchCode = "0606", BranchName = "Lakewood Financial Center", Address = "987 Lakewood Drive",     City = "Denver",        State = "CO", ZipCode = "80201", Phone = "3035550606", IsActive = true },
                new BranchLocation { BranchId = 7,  BranchCode = "0707", BranchName = "Midtown Lending Office",    Address = "147 Midtown Plaza",      City = "Atlanta",       State = "GA", ZipCode = "30301", Phone = "4045550707", IsActive = true },
                new BranchLocation { BranchId = 8,  BranchCode = "0808", BranchName = "Riverside Branch",          Address = "258 River Road",         City = "Columbus",      State = "OH", ZipCode = "43201", Phone = "6145550808", IsActive = true },
                new BranchLocation { BranchId = 9,  BranchCode = "0909", BranchName = "Highland Park Office",      Address = "369 Highland Avenue",    City = "Birmingham",    State = "AL", ZipCode = "35201", Phone = "2055550909", IsActive = true },
                new BranchLocation { BranchId = 10, BranchCode = "1010", BranchName = "Greenfield Lending Center", Address = "741 Greenfield Parkway", City = "Greenville",    State = "SC", ZipCode = "29601", Phone = "8645551010", IsActive = true }
            );
            
            // ── LoanAccount Seed ──────────────────────────────────────────────────────
            modelBuilder.Entity<LoanAccount>().HasData(
                // John Miller - active loan
                new LoanAccount
                {
                    LoanId = 1, BranchCode = "0404", LoanClass = "1", AccountNumber = "10001",
                    StatusCodes = "PB", FirstName = "JOHN", LastName = "MILLER",
                    TaxId = "123456789", StreetAddress = "415 MAPLE STREET, NASHVILLE TN 37201",
                    MobilePhone = "6155551234", HomePhone = "6155554321",
                    OriginationDate = new DateTime(2023, 6, 15),
                    LoanAmount = 3500.00m, NetProceeds = 2800.00m, PrincipalBalance = 1850.00m,
                    PayoffAmount = 1920.00m, DelinquentAmount = 0m, ChargeOffDate = null,
                    FirstPaymentDate = new DateTime(2023, 7, 15),
                    ScheduledPayment = 175.00m, AmountDue = 175.00m,
                    NextDueDate = new DateTime(2025, 3, 15), NextDueAmount = 175.00m,
                    LastDueDate = new DateTime(2025, 2, 15),
                    LastPaymentDate = new DateTime(2025, 2, 10), LastPaymentAmount = 175.00m,
                    LoanReference = "0404-10001"
                },
                // Sarah Johnson - charge-off
                new LoanAccount
                {
                    LoanId = 2, BranchCode = "0505", LoanClass = "1", AccountNumber = "20001",
                    StatusCodes = "CO", FirstName = "SARAH", LastName = "JOHNSON",
                    TaxId = "234567890", StreetAddress = "822 OAK LANE, CHARLOTTE NC 28201",
                    MobilePhone = "7045552345", HomePhone = null,
                    OriginationDate = new DateTime(2021, 3, 10),
                    LoanAmount = 5000.00m, NetProceeds = 4200.00m, PrincipalBalance = 3100.00m,
                    PayoffAmount = 3450.00m, DelinquentAmount = 890.00m,
                    ChargeOffDate = new DateTime(2023, 8, 1),
                    FirstPaymentDate = new DateTime(2021, 4, 10),
                    ScheduledPayment = 210.00m, AmountDue = 890.00m,
                    NextDueDate = null, NextDueAmount = null,
                    LastDueDate = new DateTime(2023, 6, 10),
                    LastPaymentDate = new DateTime(2023, 5, 5), LastPaymentAmount = 210.00m,
                    LoanReference = "0505-20001"
                },
                // Robert Davis - paid off
                new LoanAccount
                {
                    LoanId = 3, BranchCode = "0707", LoanClass = "1", AccountNumber = "30001",
                    StatusCodes = "PO", FirstName = "ROBERT", LastName = "DAVIS",
                    TaxId = "345678901", StreetAddress = "331 PEACHTREE RD, ATLANTA GA 30301",
                    MobilePhone = "4045553456", HomePhone = "4045556543",
                    OriginationDate = new DateTime(2022, 1, 20),
                    LoanAmount = 2000.00m, NetProceeds = 1650.00m, PrincipalBalance = 0m,
                    PayoffAmount = 0m, DelinquentAmount = 0m, ChargeOffDate = null,
                    FirstPaymentDate = new DateTime(2022, 2, 20),
                    ScheduledPayment = 120.00m, AmountDue = 0m,
                    NextDueDate = null, NextDueAmount = null,
                    LastDueDate = new DateTime(2024, 1, 20),
                    LastPaymentDate = new DateTime(2024, 1, 15), LastPaymentAmount = 120.00m,
                    LoanReference = "0707-30001"
                },
                // Maria Garcia - multiple loans (loan 1 - older closed)
                new LoanAccount
                {
                    LoanId = 4, BranchCode = "0202", LoanClass = "1", AccountNumber = "40001",
                    StatusCodes = "PO", FirstName = "MARIA", LastName = "GARCIA",
                    TaxId = "456789012", StreetAddress = "19 SUNSET BLVD, RIVERSIDE CA 92501",
                    MobilePhone = "9515554567", HomePhone = null,
                    OriginationDate = new DateTime(2020, 5, 8),
                    LoanAmount = 1500.00m, NetProceeds = 1200.00m, PrincipalBalance = 0m,
                    PayoffAmount = 0m, DelinquentAmount = 0m, ChargeOffDate = null,
                    FirstPaymentDate = new DateTime(2020, 6, 8),
                    ScheduledPayment = 95.00m, AmountDue = 0m,
                    NextDueDate = null, NextDueAmount = null,
                    LastDueDate = new DateTime(2021, 5, 8),
                    LastPaymentDate = new DateTime(2021, 5, 3), LastPaymentAmount = 95.00m,
                    LoanReference = "0202-40001"
                },
                // Maria Garcia - multiple loans (loan 2 - active)
                new LoanAccount
                {
                    LoanId = 5, BranchCode = "0202", LoanClass = "1", AccountNumber = "40002",
                    StatusCodes = "PB", FirstName = "MARIA", LastName = "GARCIA",
                    TaxId = "456789012", StreetAddress = "19 SUNSET BLVD, RIVERSIDE CA 92501",
                    MobilePhone = "9515554567", HomePhone = null,
                    OriginationDate = new DateTime(2023, 11, 1),
                    LoanAmount = 4200.00m, NetProceeds = 3500.00m, PrincipalBalance = 3200.00m,
                    PayoffAmount = 3310.00m, DelinquentAmount = 0m, ChargeOffDate = null,
                    FirstPaymentDate = new DateTime(2023, 12, 1),
                    ScheduledPayment = 195.00m, AmountDue = 195.00m,
                    NextDueDate = new DateTime(2025, 3, 1), NextDueAmount = 195.00m,
                    LastDueDate = new DateTime(2025, 2, 1),
                    LastPaymentDate = new DateTime(2025, 1, 28), LastPaymentAmount = 195.00m,
                    LoanReference = "0202-40002"
                }
            );
            
            // ── PaymentTransaction Seed ───────────────────────────────────────────────
            modelBuilder.Entity<PaymentTransaction>().HasData(
                // John Miller payments
                new PaymentTransaction { TransactionId = 1,  LoanReference = "0404-10001", TransactionDate = new DateTime(2025, 2, 10), TransactionCode = "PA", ConfirmationNumber = "10011001", TransactionAmount = 175.00m, PrincipalApplied = 142.00m, PaidThroughDate = new DateTime(2025, 2, 15), FeesApplied = 0m },
                new PaymentTransaction { TransactionId = 2,  LoanReference = "0404-10001", TransactionDate = new DateTime(2025, 1, 12), TransactionCode = "PA", ConfirmationNumber = "10011002", TransactionAmount = 175.00m, PrincipalApplied = 140.00m, PaidThroughDate = new DateTime(2025, 1, 15), FeesApplied = 0m },
                new PaymentTransaction { TransactionId = 3,  LoanReference = "0404-10001", TransactionDate = new DateTime(2024, 12, 9), TransactionCode = "PA", ConfirmationNumber = "10011003", TransactionAmount = 175.00m, PrincipalApplied = 138.00m, PaidThroughDate = new DateTime(2024, 12, 15), FeesApplied = 0m },
                new PaymentTransaction { TransactionId = 4,  LoanReference = "0404-10001", TransactionDate = new DateTime(2024, 11, 11), TransactionCode = "PA", ConfirmationNumber = "10011004", TransactionAmount = 175.00m, PrincipalApplied = 136.00m, PaidThroughDate = new DateTime(2024, 11, 15), FeesApplied = 0m },
                new PaymentTransaction { TransactionId = 5,  LoanReference = "0404-10001", TransactionDate = new DateTime(2024, 10, 14), TransactionCode = "PA", ConfirmationNumber = "10011005", TransactionAmount = 175.00m, PrincipalApplied = 134.00m, PaidThroughDate = new DateTime(2024, 10, 15), FeesApplied = 0m },
            
                // Sarah Johnson payments (before charge-off)
                new PaymentTransaction { TransactionId = 6,  LoanReference = "0505-20001", TransactionDate = new DateTime(2023, 5, 5),  TransactionCode = "PA", ConfirmationNumber = "20011001", TransactionAmount = 210.00m, PrincipalApplied = 160.00m, PaidThroughDate = new DateTime(2023, 5, 10), FeesApplied = 0m },
                new PaymentTransaction { TransactionId = 7,  LoanReference = "0505-20001", TransactionDate = new DateTime(2023, 4, 3),  TransactionCode = "PA", ConfirmationNumber = "20011002", TransactionAmount = 210.00m, PrincipalApplied = 158.00m, PaidThroughDate = new DateTime(2023, 4, 10), FeesApplied = 0m },
                new PaymentTransaction { TransactionId = 8,  LoanReference = "0505-20001", TransactionDate = new DateTime(2023, 3, 6),  TransactionCode = "PA", ConfirmationNumber = "20011003", TransactionAmount = 210.00m, PrincipalApplied = 156.00m, PaidThroughDate = new DateTime(2023, 3, 10), FeesApplied = 0m },
                new PaymentTransaction { TransactionId = 9,  LoanReference = "0505-20001", TransactionDate = new DateTime(2023, 1, 9),  TransactionCode = "LF", ConfirmationNumber = "20011004", TransactionAmount = 25.00m,  PrincipalApplied = 0m,      PaidThroughDate = null,                        FeesApplied = 25.00m },
            
                // Robert Davis payments
                new PaymentTransaction { TransactionId = 10, LoanReference = "0707-30001", TransactionDate = new DateTime(2024, 1, 15), TransactionCode = "PO", ConfirmationNumber = "30011001", TransactionAmount = 120.00m, PrincipalApplied = 119.00m, PaidThroughDate = new DateTime(2024, 1, 20), FeesApplied = 0m },
                new PaymentTransaction { TransactionId = 11, LoanReference = "0707-30001", TransactionDate = new DateTime(2023, 12, 18), TransactionCode = "PA", ConfirmationNumber = "30011002", TransactionAmount = 120.00m, PrincipalApplied = 116.00m, PaidThroughDate = new DateTime(2023, 12, 20), FeesApplied = 0m },
                new PaymentTransaction { TransactionId = 12, LoanReference = "0707-30001", TransactionDate = new DateTime(2023, 11, 13), TransactionCode = "PA", ConfirmationNumber = "30011003", TransactionAmount = 120.00m, PrincipalApplied = 113.00m, PaidThroughDate = new DateTime(2023, 11, 20), FeesApplied = 0m },
            
                // Maria Garcia loan 1 payments
                new PaymentTransaction { TransactionId = 13, LoanReference = "0202-40001", TransactionDate = new DateTime(2021, 5, 3),  TransactionCode = "PO", ConfirmationNumber = "40011001", TransactionAmount = 95.00m,  PrincipalApplied = 94.00m,  PaidThroughDate = new DateTime(2021, 5, 8),  FeesApplied = 0m },
                new PaymentTransaction { TransactionId = 14, LoanReference = "0202-40001", TransactionDate = new DateTime(2021, 4, 5),  TransactionCode = "PA", ConfirmationNumber = "40011002", TransactionAmount = 95.00m,  PrincipalApplied = 92.00m,  PaidThroughDate = new DateTime(2021, 4, 8),  FeesApplied = 0m },
            
                // Maria Garcia loan 2 payments
                new PaymentTransaction { TransactionId = 15, LoanReference = "0202-40002", TransactionDate = new DateTime(2025, 1, 28), TransactionCode = "PA", ConfirmationNumber = "40021001", TransactionAmount = 195.00m, PrincipalApplied = 155.00m, PaidThroughDate = new DateTime(2025, 2, 1),  FeesApplied = 0m },
                new PaymentTransaction { TransactionId = 16, LoanReference = "0202-40002", TransactionDate = new DateTime(2024, 12, 30), TransactionCode = "PA", ConfirmationNumber = "40021002", TransactionAmount = 195.00m, PrincipalApplied = 153.00m, PaidThroughDate = new DateTime(2025, 1, 1),  FeesApplied = 0m },
                new PaymentTransaction { TransactionId = 17, LoanReference = "0202-40002", TransactionDate = new DateTime(2024, 11, 27), TransactionCode = "PA", ConfirmationNumber = "40021003", TransactionAmount = 195.00m, PrincipalApplied = 151.00m, PaidThroughDate = new DateTime(2024, 12, 1), FeesApplied = 0m }
            );
        }
    }
}