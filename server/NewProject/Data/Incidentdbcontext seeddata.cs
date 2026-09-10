using IncidentManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace IncidentManagement.Data
{
    public partial class IncidentDbContext
    {
        partial void SeedSolutionsAndTips(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Solution>().HasData(
                // 1: NB Loan Inquiry
                new Solution { SolutionId = 1, IssueId = 1, Name = "Transferred to Branch", IsActive = true },
                new Solution { SolutionId = 2, IssueId = 1, Name = "Application Started", IsActive = true },
                new Solution { SolutionId = 3, IssueId = 1, Name = "Information Provided - No Application", IsActive = true },
                // 2: FB Loan Inquiry
                new Solution { SolutionId = 4, IssueId = 2, Name = "Transferred to Branch", IsActive = true },
                new Solution { SolutionId = 5, IssueId = 2, Name = "Application Started", IsActive = true },
                new Solution { SolutionId = 6, IssueId = 2, Name = "Information Provided - No Application", IsActive = true },
                // 3: Refinance Inquiry
                new Solution { SolutionId = 7, IssueId = 3, Name = "Transferred to Branch", IsActive = true },
                new Solution { SolutionId = 8, IssueId = 3, Name = "Refinance Information Provided", IsActive = true },
                new Solution { SolutionId = 9, IssueId = 3, Name = "Application Started", IsActive = true },
                // 4: Credit Card
                new Solution { SolutionId = 10, IssueId = 4, Name = "Credit Card Information Provided", IsActive = true },
                new Solution { SolutionId = 11, IssueId = 4, Name = "Transferred to Credit Card Department", IsActive = true },
                new Solution { SolutionId = 12, IssueId = 4, Name = "Issue Resolved", IsActive = true },
                // 5: Taxes
                new Solution { SolutionId = 13, IssueId = 5, Name = "Tax Information Provided", IsActive = true },
                new Solution { SolutionId = 14, IssueId = 5, Name = "Tax Document Sent", IsActive = true },
                new Solution { SolutionId = 15, IssueId = 5, Name = "Transferred to Tax Department", IsActive = true },
                // 6: Payment Processed
                new Solution { SolutionId = 16, IssueId = 6, Name = "ACH Payment Processed", IsActive = true },
                new Solution { SolutionId = 17, IssueId = 6, Name = "Card Payment Processed", IsActive = true },
                new Solution { SolutionId = 18, IssueId = 6, Name = "Payment Scheduled", IsActive = true },
                // 7: Setup Auto Pay
                new Solution { SolutionId = 19, IssueId = 7, Name = "Auto Pay Set Up - ACH", IsActive = true },
                new Solution { SolutionId = 20, IssueId = 7, Name = "Auto Pay Set Up - Card", IsActive = true },
                // 8: Cancelled Future Payment
                new Solution { SolutionId = 21, IssueId = 8, Name = "Future Payment Cancelled", IsActive = true },
                new Solution { SolutionId = 22, IssueId = 8, Name = "Future Payment Rescheduled", IsActive = true },
                // 9: Cancelled Auto Pay
                new Solution { SolutionId = 23, IssueId = 9, Name = "Auto Pay Cancelled", IsActive = true },
                new Solution { SolutionId = 24, IssueId = 9, Name = "Auto Pay Updated", IsActive = true },
                // 10: Refund Payment
                new Solution { SolutionId = 25, IssueId = 10, Name = "Refund Processed", IsActive = true },
                new Solution { SolutionId = 26, IssueId = 10, Name = "Refund Pending - Escalated", IsActive = true },
                new Solution { SolutionId = 27, IssueId = 10, Name = "Payment Voided", IsActive = true },
                // 11: Promise to Pay
                new Solution { SolutionId = 28, IssueId = 11, Name = "Promise to Pay Recorded", IsActive = true },
                new Solution { SolutionId = 29, IssueId = 11, Name = "Payment Arrangement Set Up", IsActive = true },
                new Solution { SolutionId = 30, IssueId = 11, Name = "Hardship Extension Granted", IsActive = true },
                new Solution { SolutionId = 31, IssueId = 11, Name = "Transferred to Collections", IsActive = true },
                // 12: Negotiated Settlement
                new Solution { SolutionId = 32, IssueId = 12, Name = "Settlement Offer Accepted", IsActive = true },
                new Solution { SolutionId = 33, IssueId = 12, Name = "Settlement Offer Declined", IsActive = true },
                new Solution { SolutionId = 34, IssueId = 12, Name = "Settlement Escalated to Supervisor", IsActive = true },
                new Solution { SolutionId = 35, IssueId = 12, Name = "Settlement Payment Processed", IsActive = true },
                // 13: Cancelled Insurance
                new Solution { SolutionId = 36, IssueId = 13, Name = "Insurance Cancellation Processed", IsActive = true },
                new Solution { SolutionId = 37, IssueId = 13, Name = "Insurance Cancellation Pending", IsActive = true },
                new Solution { SolutionId = 38, IssueId = 13, Name = "Customer Retained Insurance", IsActive = true },
                // 14: Assigned Account
                new Solution { SolutionId = 39, IssueId = 14, Name = "Account Information Provided", IsActive = true },
                new Solution { SolutionId = 40, IssueId = 14, Name = "Transferred to Legal Servicer", IsActive = true },
                new Solution { SolutionId = 41, IssueId = 14, Name = "Payment Taken for Assigned Account", IsActive = true },
                // 15: Sold Account
                new Solution { SolutionId = 42, IssueId = 15, Name = "Third Party Information Provided", IsActive = true },
                new Solution { SolutionId = 43, IssueId = 15, Name = "Customer Directed to New Servicer", IsActive = true },
                // 16: Bankruptcy Account
                new Solution { SolutionId = 44, IssueId = 16, Name = "Bankruptcy Information Provided", IsActive = true },
                new Solution { SolutionId = 45, IssueId = 16, Name = "Transferred to Bankruptcy Department", IsActive = true },
                new Solution { SolutionId = 46, IssueId = 16, Name = "Account Noted - No Action Taken", IsActive = true },
                // 17: Repo Inquiry
                new Solution { SolutionId = 47, IssueId = 17, Name = "Repo Information Provided", IsActive = true },
                new Solution { SolutionId = 48, IssueId = 17, Name = "Transferred to Collections", IsActive = true },
                new Solution { SolutionId = 49, IssueId = 17, Name = "Redemption Information Provided", IsActive = true },
                // 18: Lien Inquiry
                new Solution { SolutionId = 50, IssueId = 18, Name = "Lien Information Provided", IsActive = true },
                new Solution { SolutionId = 51, IssueId = 18, Name = "Lien Release Requested", IsActive = true },
                new Solution { SolutionId = 52, IssueId = 18, Name = "Transferred to Branch", IsActive = true },
                // 19: Payment Question
                new Solution { SolutionId = 53, IssueId = 19, Name = "Payment Information Provided", IsActive = true },
                new Solution { SolutionId = 54, IssueId = 19, Name = "Payment History Reviewed with Customer", IsActive = true },
                new Solution { SolutionId = 55, IssueId = 19, Name = "Discrepancy Escalated", IsActive = true },
                // 20: Credit Question
                new Solution { SolutionId = 56, IssueId = 20, Name = "Credit Information Provided", IsActive = true },
                new Solution { SolutionId = 57, IssueId = 20, Name = "Credit Report Explanation Provided", IsActive = true },
                new Solution { SolutionId = 58, IssueId = 20, Name = "Directed to Credit Bureau", IsActive = true },
                // 21: Insurance Question
                new Solution { SolutionId = 59, IssueId = 21, Name = "Insurance Information Provided", IsActive = true },
                new Solution { SolutionId = 60, IssueId = 21, Name = "Insurance Details Reviewed with Customer", IsActive = true },
                new Solution { SolutionId = 61, IssueId = 21, Name = "Transferred to Insurance Department", IsActive = true },
                // 22: Application Question
                new Solution { SolutionId = 62, IssueId = 22, Name = "Application Process Explained", IsActive = true },
                new Solution { SolutionId = 63, IssueId = 22, Name = "Transferred to Branch", IsActive = true },
                new Solution { SolutionId = 64, IssueId = 22, Name = "Requirements Information Provided", IsActive = true },
                // 23: General Question
                new Solution { SolutionId = 65, IssueId = 23, Name = "Question Answered", IsActive = true },
                new Solution { SolutionId = 66, IssueId = 23, Name = "Transferred to Appropriate Department", IsActive = true },
                new Solution { SolutionId = 67, IssueId = 23, Name = "Branch Contact Information Provided", IsActive = true },
                // 24: Account Question
                new Solution { SolutionId = 68, IssueId = 24, Name = "Account Information Provided", IsActive = true },
                new Solution { SolutionId = 69, IssueId = 24, Name = "Account Details Reviewed with Customer", IsActive = true },
                new Solution { SolutionId = 70, IssueId = 24, Name = "Transferred to Branch", IsActive = true },
                // 25: App Status Question
                new Solution { SolutionId = 71, IssueId = 25, Name = "Application Status Provided", IsActive = true },
                new Solution { SolutionId = 72, IssueId = 25, Name = "Transferred to Branch for Status Update", IsActive = true },
                new Solution { SolutionId = 73, IssueId = 25, Name = "Customer Advised to Wait for Decision", IsActive = true },
                // 26: Credit Dispute
                new Solution { SolutionId = 74, IssueId = 26, Name = "Credit Dispute Submitted", IsActive = true },
                new Solution { SolutionId = 75, IssueId = 26, Name = "Credit Dispute Follow Up Completed", IsActive = true },
                new Solution { SolutionId = 76, IssueId = 26, Name = "Directed to Credit Bureau for Dispute", IsActive = true },
                // 27: ID Theft Dispute
                new Solution { SolutionId = 77, IssueId = 27, Name = "ID Theft Dispute Submitted", IsActive = true },
                new Solution { SolutionId = 78, IssueId = 27, Name = "ID Theft Follow Up Completed", IsActive = true },
                new Solution { SolutionId = 79, IssueId = 27, Name = "ServiceNow Ticket Created", IsActive = true },
                new Solution { SolutionId = 80, IssueId = 27, Name = "Escalated to Compliance", IsActive = true },
                // 28: Payment Dispute
                new Solution { SolutionId = 81, IssueId = 28, Name = "Payment Dispute Submitted", IsActive = true },
                new Solution { SolutionId = 82, IssueId = 28, Name = "Unauthorized Payment Escalated", IsActive = true },
                new Solution { SolutionId = 83, IssueId = 28, Name = "Payment Dispute Follow Up Completed", IsActive = true },
                // 29: Online Account Set Up
                new Solution { SolutionId = 84, IssueId = 29, Name = "Online Account Created", IsActive = true },
                new Solution { SolutionId = 85, IssueId = 29, Name = "Loan Linked to Online Account", IsActive = true },
                new Solution { SolutionId = 86, IssueId = 29, Name = "Customer Assisted with Registration", IsActive = true },
                // 30: Password Reset
                new Solution { SolutionId = 87, IssueId = 30, Name = "Password Reset Completed", IsActive = true },
                new Solution { SolutionId = 88, IssueId = 30, Name = "Password Reset Link Sent", IsActive = true },
                new Solution { SolutionId = 89, IssueId = 30, Name = "Account Unlocked", IsActive = true },
                // 31: Website Issues
                new Solution { SolutionId = 90, IssueId = 31, Name = "Issue Reported and Escalated", IsActive = true },
                new Solution { SolutionId = 91, IssueId = 31, Name = "Workaround Provided", IsActive = true },
                new Solution { SolutionId = 92, IssueId = 31, Name = "Customer Directed to Alternative Method", IsActive = true },
                // 32: IVR Transfer
                new Solution { SolutionId = 93, IssueId = 32, Name = "Call Transferred to IVR", IsActive = true },
                new Solution { SolutionId = 94, IssueId = 32, Name = "IVR Option Explained to Customer", IsActive = true },
                // 33: Complaint Submitted
                new Solution { SolutionId = 95, IssueId = 33, Name = "Complaint Documented and Submitted", IsActive = true },
                new Solution { SolutionId = 96, IssueId = 33, Name = "Complaint Escalated to Supervisor", IsActive = true },
                new Solution { SolutionId = 97, IssueId = 33, Name = "Resolution Provided - No Formal Complaint", IsActive = true },
                // 34: Escalated to MGR
                new Solution { SolutionId = 98,  IssueId = 34, Name = "Escalated to Senior Representative", IsActive = true },
                new Solution { SolutionId = 99,  IssueId = 34, Name = "Escalated to Manager", IsActive = true },
                new Solution { SolutionId = 100, IssueId = 34, Name = "Issue Resolved After Escalation", IsActive = true },
                // 35: Adverse Action Letter
                new Solution { SolutionId = 101, IssueId = 35, Name = "Adverse Action Letter Explained", IsActive = true },
                new Solution { SolutionId = 102, IssueId = 35, Name = "Denial Reasons Provided", IsActive = true },
                new Solution { SolutionId = 103, IssueId = 35, Name = "Transferred to Branch", IsActive = true },
                // 36: Restricted Contact
                new Solution { SolutionId = 104, IssueId = 36, Name = "Communication Restriction Added", IsActive = true },
                new Solution { SolutionId = 105, IssueId = 36, Name = "Do Not Call Code Applied", IsActive = true },
                new Solution { SolutionId = 106, IssueId = 36, Name = "Restriction Details Confirmed with Customer", IsActive = true },
                // 37: Letter Request
                new Solution { SolutionId = 107, IssueId = 37, Name = "Letter Requested and Submitted", IsActive = true },
                new Solution { SolutionId = 108, IssueId = 37, Name = "Payoff Letter Sent", IsActive = true },
                new Solution { SolutionId = 109, IssueId = 37, Name = "Account Statement Sent", IsActive = true },
                new Solution { SolutionId = 110, IssueId = 37, Name = "Customer Directed to Branch for Letter", IsActive = true },
                // 38: 3rd Party
                new Solution { SolutionId = 111, IssueId = 38, Name = "Third Party Verified and Assisted", IsActive = true },
                new Solution { SolutionId = 112, IssueId = 38, Name = "Third Party Not Authorized - No Information Given", IsActive = true },
                new Solution { SolutionId = 113, IssueId = 38, Name = "Third Party Directed to Account Holder", IsActive = true },
                // 39: Non-World Customer
                new Solution { SolutionId = 114, IssueId = 39, Name = "Wrong Number - Call Ended", IsActive = true },
                new Solution { SolutionId = 115, IssueId = 39, Name = "Solicitation - Call Ended", IsActive = true },
                new Solution { SolutionId = 116, IssueId = 39, Name = "Directed to Correct Company", IsActive = true },
                // 40: Spanish Call
                new Solution { SolutionId = 117, IssueId = 40, Name = "Spanish Representative Connected", IsActive = true },
                new Solution { SolutionId = 118, IssueId = 40, Name = "Spanish Line Unavailable - Alternative Provided", IsActive = true },
                new Solution { SolutionId = 119, IssueId = 40, Name = "Transferred to Spanish Speaking Branch", IsActive = true },
                // 41: Dead Air
                new Solution { SolutionId = 120, IssueId = 41, Name = "No Response - Call Ended", IsActive = true },
                new Solution { SolutionId = 121, IssueId = 41, Name = "Greeting Repeated - No Response - Disconnected", IsActive = true },
                // 42: Customer Hang Up
                new Solution { SolutionId = 122, IssueId = 42, Name = "Customer Disconnected Before Reason Given", IsActive = true },
                new Solution { SolutionId = 123, IssueId = 42, Name = "Call Back Attempted", IsActive = true },
                // 43: Reason Not Provided
                new Solution { SolutionId = 124, IssueId = 43, Name = "Customer Declined to Provide Reason", IsActive = true },
                new Solution { SolutionId = 125, IssueId = 43, Name = "Rollover Call - Branch Contact Provided", IsActive = true },
                // 44: Callback No Answer
                new Solution { SolutionId = 126, IssueId = 44, Name = "No Answer - Voicemail Left", IsActive = true },
                new Solution { SolutionId = 127, IssueId = 44, Name = "No Answer - No Voicemail", IsActive = true },
                new Solution { SolutionId = 128, IssueId = 44, Name = "Callback Rescheduled", IsActive = true },
                // 45: Callback 3rd Party
                new Solution { SolutionId = 129, IssueId = 45, Name = "Third Party Answered - Message Left", IsActive = true },
                new Solution { SolutionId = 130, IssueId = 45, Name = "Third Party Answered - No Message Left", IsActive = true },
                new Solution { SolutionId = 131, IssueId = 45, Name = "Callback Rescheduled", IsActive = true },
                // 46: Do Not Call
                new Solution { SolutionId = 132, IssueId = 46, Name = "Do Not Call Code Applied", IsActive = true },
                new Solution { SolutionId = 133, IssueId = 46, Name = "Full Cease and Desist Applied", IsActive = true },
                new Solution { SolutionId = 134, IssueId = 46, Name = "Do Not Call Confirmed and Documented", IsActive = true }
            );

            modelBuilder.Entity<CallTip>().HasData(
                // 1: NB Loan Inquiry
                new CallTip { TipId = 182, IssueId = 1, Tip = "Greet the caller warmly and ask how you can assist them today with starting a new loan application.", SortOrder = 1 },
                new CallTip { TipId = 183, IssueId = 1, Tip = "Collect the caller's basic information — name, address, and phone number — before transferring to a branch.", SortOrder = 2 },
                new CallTip { TipId = 184, IssueId = 1, Tip = "Inform the caller of the general requirements: valid ID, proof of income, and references may be needed.", SortOrder = 3 },
                new CallTip { TipId = 185, IssueId = 1, Tip = "If the caller wants to proceed, offer to transfer them to their nearest branch or provide branch contact information.", SortOrder = 4 },
                // 2: FB Loan Inquiry
                new CallTip { TipId = 186, IssueId = 2, Tip = "Welcome back former borrowers warmly — acknowledge their previous relationship with the company.", SortOrder = 1 },
                new CallTip { TipId = 187, IssueId = 2, Tip = "Ask for their previous branch location to route them to the correct team.", SortOrder = 2 },
                new CallTip { TipId = 188, IssueId = 2, Tip = "Let them know returning customers may qualify for expedited processing based on their prior history.", SortOrder = 3 },
                new CallTip { TipId = 189, IssueId = 2, Tip = "Transfer to the appropriate branch or collect information to start the application process.", SortOrder = 4 },
                // 3: Refinance Inquiry
                new CallTip { TipId = 190, IssueId = 3, Tip = "Verify the customer's current loan details before discussing refinance options.", SortOrder = 1 },
                new CallTip { TipId = 191, IssueId = 3, Tip = "Do not quote specific rates — inform the customer that rates are determined by the branch.", SortOrder = 2 },
                new CallTip { TipId = 192, IssueId = 3, Tip = "Explain that a refinance may increase the loan amount or adjust the payment terms.", SortOrder = 3 },
                new CallTip { TipId = 193, IssueId = 3, Tip = "Transfer to the branch for a formal refinance evaluation.", SortOrder = 4 },
                // 4: Credit Card
                new CallTip { TipId = 194, IssueId = 4, Tip = "Identify which credit card product the customer is calling about before proceeding.", SortOrder = 1 },
                new CallTip { TipId = 195, IssueId = 4, Tip = "If the issue is beyond your scope, transfer to the credit card department promptly.", SortOrder = 2 },
                new CallTip { TipId = 196, IssueId = 4, Tip = "Do not provide credit card numbers or sensitive card details over the phone — direct to secure channels.", SortOrder = 3 },
                // 5: Taxes
                new CallTip { TipId = 197, IssueId = 5, Tip = "Confirm the customer's identity before discussing any tax-related account information.", SortOrder = 1 },
                new CallTip { TipId = 198, IssueId = 5, Tip = "Tax documents are typically mailed by January 31st for the prior year — advise if the customer has not received theirs.", SortOrder = 2 },
                new CallTip { TipId = 199, IssueId = 5, Tip = "If the customer needs a duplicate tax document, note the request and advise of processing time.", SortOrder = 3 },
                new CallTip { TipId = 200, IssueId = 5, Tip = "Do not provide tax advice — direct tax-specific questions to a qualified tax professional.", SortOrder = 4 },
                // 6: Payment Processed
                new CallTip { TipId = 201, IssueId = 6, Tip = "Confirm the payment amount, date, and method before ending the call.", SortOrder = 1 },
                new CallTip { TipId = 202, IssueId = 6, Tip = "Provide the customer with a confirmation number if available.", SortOrder = 2 },
                new CallTip { TipId = 203, IssueId = 6, Tip = "Let the customer know when the payment will be reflected on their account — typically 1-2 business days for ACH.", SortOrder = 3 },
                // 7: Setup Auto Pay
                new CallTip { TipId = 204, IssueId = 7, Tip = "Collect the customer's bank routing number, account number, and account type (checking or savings) for ACH setup.", SortOrder = 1 },
                new CallTip { TipId = 205, IssueId = 7, Tip = "For card auto pay, collect the card number, expiration date, and billing zip code.", SortOrder = 2 },
                new CallTip { TipId = 206, IssueId = 7, Tip = "Confirm the payment date and amount with the customer before finalizing setup.", SortOrder = 3 },
                new CallTip { TipId = 207, IssueId = 7, Tip = "Advise the customer they can cancel auto pay at any time by calling in.", SortOrder = 4 },
                // 8: Cancelled Future Payment
                new CallTip { TipId = 208, IssueId = 8, Tip = "Verify the future payment date and amount with the customer before cancelling.", SortOrder = 1 },
                new CallTip { TipId = 209, IssueId = 8, Tip = "Confirm cancellation and advise the customer their payment will not be processed on that date.", SortOrder = 2 },
                new CallTip { TipId = 210, IssueId = 8, Tip = "Ask if the customer would like to reschedule the payment for a different date.", SortOrder = 3 },
                // 9: Cancelled Auto Pay
                new CallTip { TipId = 211, IssueId = 9, Tip = "Confirm which recurring payment the customer wants to cancel before proceeding.", SortOrder = 1 },
                new CallTip { TipId = 212, IssueId = 9, Tip = "Advise the customer that cancellation may not take effect if processed within 24 hours of the next payment date.", SortOrder = 2 },
                new CallTip { TipId = 213, IssueId = 9, Tip = "Ask if the customer would like to update their payment method rather than cancel entirely.", SortOrder = 3 },
                // 10: Refund Payment
                new CallTip { TipId = 214, IssueId = 10, Tip = "Verify the payment details — date, amount, and method — before initiating a refund.", SortOrder = 1 },
                new CallTip { TipId = 215, IssueId = 10, Tip = "Refunds typically take 3-5 business days to process back to the original payment method.", SortOrder = 2 },
                new CallTip { TipId = 216, IssueId = 10, Tip = "If the refund is for a duplicate payment, escalate to a supervisor for approval.", SortOrder = 3 },
                new CallTip { TipId = 217, IssueId = 10, Tip = "Document all refund details in the incident notes.", SortOrder = 4 },
                // 11: Promise to Pay
                new CallTip { TipId = 218, IssueId = 11, Tip = "Listen empathetically to the customer's hardship situation before proposing options.", SortOrder = 1 },
                new CallTip { TipId = 219, IssueId = 11, Tip = "Record the specific date and amount the customer commits to paying.", SortOrder = 2 },
                new CallTip { TipId = 220, IssueId = 11, Tip = "Do not promise outcomes beyond your authority — supervisor approval is required for extensions.", SortOrder = 3 },
                new CallTip { TipId = 221, IssueId = 11, Tip = "Advise the customer of any late fees or consequences of missing the promised payment date.", SortOrder = 4 },
                // 12: Negotiated Settlement
                new CallTip { TipId = 222, IssueId = 12, Tip = "Do not discuss settlement options without supervisor approval first.", SortOrder = 1 },
                new CallTip { TipId = 223, IssueId = 12, Tip = "Verify the current payoff amount before presenting any settlement figures.", SortOrder = 2 },
                new CallTip { TipId = 224, IssueId = 12, Tip = "Settlement offers are final — advise the customer that partial payments may not be accepted after an offer is made.", SortOrder = 3 },
                new CallTip { TipId = 225, IssueId = 12, Tip = "Document all settlement details thoroughly in the incident notes.", SortOrder = 4 },
                // 13: Cancelled Insurance
                new CallTip { TipId = 226, IssueId = 13, Tip = "Verify the customer's identity and loan details before processing any insurance cancellation.", SortOrder = 1 },
                new CallTip { TipId = 227, IssueId = 13, Tip = "Inform the customer that cancelling insurance may affect their loan terms — confirm they understand.", SortOrder = 2 },
                new CallTip { TipId = 228, IssueId = 13, Tip = "Note the cancellation request date and advise of the effective cancellation date.", SortOrder = 3 },
                // 14: Assigned Account
                new CallTip { TipId = 229, IssueId = 14, Tip = "Do not provide detailed account information to third parties — verify the caller is the account holder first.", SortOrder = 1 },
                new CallTip { TipId = 230, IssueId = 14, Tip = "Inform the customer that their account has been assigned to a legal servicer for collections.", SortOrder = 2 },
                new CallTip { TipId = 231, IssueId = 14, Tip = "Provide the legal servicer contact information if available.", SortOrder = 3 },
                new CallTip { TipId = 232, IssueId = 14, Tip = "Payments may still be accepted — check account status before processing.", SortOrder = 4 },
                // 15: Sold Account
                new CallTip { TipId = 233, IssueId = 15, Tip = "Verify the account has been sold before providing any information about the new servicer.", SortOrder = 1 },
                new CallTip { TipId = 234, IssueId = 15, Tip = "Direct the customer to the new servicer for all future account inquiries and payments.", SortOrder = 2 },
                new CallTip { TipId = 235, IssueId = 15, Tip = "Do not accept payments for sold accounts — direct to the purchasing company.", SortOrder = 3 },
                // 16: Bankruptcy Account
                new CallTip { TipId = 236, IssueId = 16, Tip = "Do not contact customers with active bankruptcy without legal department approval.", SortOrder = 1 },
                new CallTip { TipId = 237, IssueId = 16, Tip = "Do not discuss payment arrangements or collections for accounts in active bankruptcy.", SortOrder = 2 },
                new CallTip { TipId = 238, IssueId = 16, Tip = "Transfer all bankruptcy-related calls to the bankruptcy department immediately.", SortOrder = 3 },
                new CallTip { TipId = 239, IssueId = 16, Tip = "Document the call thoroughly and note that the account is in bankruptcy.", SortOrder = 4 },
                // 17: Repo Inquiry
                new CallTip { TipId = 240, IssueId = 17, Tip = "Do not confirm or deny repossession details without verifying the customer's identity first.", SortOrder = 1 },
                new CallTip { TipId = 241, IssueId = 17, Tip = "Provide the repossession company contact information if the customer is inquiring about their vehicle.", SortOrder = 2 },
                new CallTip { TipId = 242, IssueId = 17, Tip = "If the customer wants to redeem their vehicle, transfer to collections for redemption details.", SortOrder = 3 },
                new CallTip { TipId = 243, IssueId = 17, Tip = "Do not make promises about stopping or reversing a repossession without supervisor approval.", SortOrder = 4 },
                // 18: Lien Inquiry
                new CallTip { TipId = 244, IssueId = 18, Tip = "Verify the customer's identity and loan status before discussing lien details.", SortOrder = 1 },
                new CallTip { TipId = 245, IssueId = 18, Tip = "Lien releases are processed after the loan is paid in full — advise of processing time (typically 2-4 weeks).", SortOrder = 2 },
                new CallTip { TipId = 246, IssueId = 18, Tip = "If the customer needs a lien release letter, submit the request and advise of turnaround time.", SortOrder = 3 },
                // 19: Payment Question
                new CallTip { TipId = 247, IssueId = 19, Tip = "Pull up the customer's payment history before answering questions to ensure accuracy.", SortOrder = 1 },
                new CallTip { TipId = 248, IssueId = 19, Tip = "If a payment is not showing, advise that ACH payments may take 1-2 business days to post.", SortOrder = 2 },
                new CallTip { TipId = 249, IssueId = 19, Tip = "Do not confirm payment details that you cannot verify in the system.", SortOrder = 3 },
                new CallTip { TipId = 250, IssueId = 19, Tip = "If there is a discrepancy, escalate to a supervisor rather than making assumptions.", SortOrder = 4 },
                // 20: Credit Question
                new CallTip { TipId = 251, IssueId = 20, Tip = "Explain that credit reporting is done monthly and updates may take 30-60 days to reflect.", SortOrder = 1 },
                new CallTip { TipId = 252, IssueId = 20, Tip = "Do not make promises about credit score improvements.", SortOrder = 2 },
                new CallTip { TipId = 253, IssueId = 20, Tip = "If the customer disputes a credit entry, direct them to submit a formal credit dispute.", SortOrder = 3 },
                // 21: Insurance Question
                new CallTip { TipId = 254, IssueId = 21, Tip = "Review the customer's insurance details on the account before answering questions.", SortOrder = 1 },
                new CallTip { TipId = 255, IssueId = 21, Tip = "If the customer wants to cancel insurance, handle under the Cancelled Insurance issue type.", SortOrder = 2 },
                new CallTip { TipId = 256, IssueId = 21, Tip = "For complex insurance questions, transfer to the insurance department.", SortOrder = 3 },
                // 22: Application Question
                new CallTip { TipId = 257, IssueId = 22, Tip = "Explain the general loan application process — application, review, decision, funding.", SortOrder = 1 },
                new CallTip { TipId = 258, IssueId = 22, Tip = "Advise the customer of typical requirements: ID, proof of income, references.", SortOrder = 2 },
                new CallTip { TipId = 259, IssueId = 22, Tip = "Do not quote specific rates or approval odds — those are determined by the branch.", SortOrder = 3 },
                new CallTip { TipId = 260, IssueId = 22, Tip = "Offer to transfer to a branch for a more detailed conversation.", SortOrder = 4 },
                // 23: General Question
                new CallTip { TipId = 261, IssueId = 23, Tip = "Listen carefully to understand exactly what the customer is asking before responding.", SortOrder = 1 },
                new CallTip { TipId = 262, IssueId = 23, Tip = "If the question is outside your knowledge, transfer to the appropriate department rather than guessing.", SortOrder = 2 },
                new CallTip { TipId = 263, IssueId = 23, Tip = "Branch hours, locations, and contact numbers are available in the branch directory.", SortOrder = 3 },
                // 24: Account Question
                new CallTip { TipId = 264, IssueId = 24, Tip = "Verify the customer's identity before providing any account details.", SortOrder = 1 },
                new CallTip { TipId = 265, IssueId = 24, Tip = "Common account questions include balance, interest rate, number of payments remaining, and next due date.", SortOrder = 2 },
                new CallTip { TipId = 266, IssueId = 24, Tip = "For questions about loan terms or modifications, transfer to the originating branch.", SortOrder = 3 },
                // 25: App Status Question
                new CallTip { TipId = 267, IssueId = 25, Tip = "Application status is determined by the branch — transfer the customer to their branch for an accurate update.", SortOrder = 1 },
                new CallTip { TipId = 268, IssueId = 25, Tip = "Do not speculate on approval or denial — only the branch can provide official status.", SortOrder = 2 },
                new CallTip { TipId = 269, IssueId = 25, Tip = "Advise the customer of typical decision timeframes if available.", SortOrder = 3 },
                // 26: Credit Dispute
                new CallTip { TipId = 270, IssueId = 26, Tip = "Collect the specific credit entry the customer is disputing — creditor name, date, and amount.", SortOrder = 1 },
                new CallTip { TipId = 271, IssueId = 26, Tip = "Submit the dispute through the proper internal channel and provide the customer a reference number if available.", SortOrder = 2 },
                new CallTip { TipId = 272, IssueId = 26, Tip = "Advise the customer that disputes are investigated within 30 days per the Fair Credit Reporting Act.", SortOrder = 3 },
                new CallTip { TipId = 273, IssueId = 26, Tip = "The customer may also dispute directly with the credit bureau — provide bureau contact information.", SortOrder = 4 },
                // 28: Payment Dispute
                new CallTip { TipId = 274, IssueId = 28, Tip = "Verify the payment details the customer is disputing — date, amount, and method.", SortOrder = 1 },
                new CallTip { TipId = 275, IssueId = 28, Tip = "Do not immediately confirm or deny unauthorized payment — escalate to a supervisor for investigation.", SortOrder = 2 },
                new CallTip { TipId = 276, IssueId = 28, Tip = "Advise the customer that the investigation may take 3-5 business days.", SortOrder = 3 },
                new CallTip { TipId = 277, IssueId = 28, Tip = "Document all dispute details thoroughly in the incident notes.", SortOrder = 4 },
                // 29: Online Account Set Up
                new CallTip { TipId = 278, IssueId = 29, Tip = "Walk the customer through the online account registration process step by step.", SortOrder = 1 },
                new CallTip { TipId = 279, IssueId = 29, Tip = "The customer will need their loan account number and the email address on file to register.", SortOrder = 2 },
                new CallTip { TipId = 280, IssueId = 29, Tip = "If the customer is having trouble linking their loan, verify the account number and branch code are correct.", SortOrder = 3 },
                new CallTip { TipId = 281, IssueId = 29, Tip = "Advise the customer to check their spam folder for the verification email.", SortOrder = 4 },
                // 30: Password Reset
                new CallTip { TipId = 282, IssueId = 30, Tip = "Verify the customer's identity before initiating a password reset.", SortOrder = 1 },
                new CallTip { TipId = 283, IssueId = 30, Tip = "The password reset link is sent to the email address on file — confirm the customer has access to that email.", SortOrder = 2 },
                new CallTip { TipId = 284, IssueId = 30, Tip = "Advise the customer the reset link expires within 24 hours.", SortOrder = 3 },
                new CallTip { TipId = 285, IssueId = 30, Tip = "If the customer no longer has access to their email on file, they will need to contact the branch to update it.", SortOrder = 4 },
                // 31: Website Issues
                new CallTip { TipId = 286, IssueId = 31, Tip = "Ask the customer to describe the issue in detail — what page, what action, and what error message if any.", SortOrder = 1 },
                new CallTip { TipId = 287, IssueId = 31, Tip = "Suggest clearing browser cache and cookies or trying a different browser as a first step.", SortOrder = 2 },
                new CallTip { TipId = 288, IssueId = 31, Tip = "If the issue persists, document it thoroughly and escalate to the technical team.", SortOrder = 3 },
                new CallTip { TipId = 289, IssueId = 31, Tip = "Provide an alternative method for the customer to complete their task while the issue is investigated.", SortOrder = 4 },
                // 32: IVR Transfer
                new CallTip { TipId = 290, IssueId = 32, Tip = "Before transferring to the IVR, confirm the customer understands they will be in an automated system.", SortOrder = 1 },
                new CallTip { TipId = 291, IssueId = 32, Tip = "Provide the customer with the IVR option number they need so they can navigate quickly.", SortOrder = 2 },
                // 33: Complaint Submitted
                new CallTip { TipId = 292, IssueId = 33, Tip = "Listen to the customer's complaint fully before responding — do not interrupt.", SortOrder = 1 },
                new CallTip { TipId = 293, IssueId = 33, Tip = "Acknowledge the customer's frustration and apologize for their experience.", SortOrder = 2 },
                new CallTip { TipId = 294, IssueId = 33, Tip = "Document the complaint in detail — what happened, when, and what resolution the customer is seeking.", SortOrder = 3 },
                new CallTip { TipId = 295, IssueId = 33, Tip = "Advise the customer of the complaint review timeframe and how they will be contacted with a resolution.", SortOrder = 4 },
                // 34: Escalated to MGR
                new CallTip { TipId = 296, IssueId = 34, Tip = "Before escalating, attempt to resolve the issue yourself and document what was tried.", SortOrder = 1 },
                new CallTip { TipId = 297, IssueId = 34, Tip = "Provide the supervisor with a brief summary of the situation before transferring the customer.", SortOrder = 2 },
                new CallTip { TipId = 298, IssueId = 34, Tip = "Do not put the customer on hold for more than 2-3 minutes without checking back in.", SortOrder = 3 },
                // 35: Adverse Action Letter
                new CallTip { TipId = 299, IssueId = 35, Tip = "Verify the customer received the adverse action letter and note the date on the letter.", SortOrder = 1 },
                new CallTip { TipId = 300, IssueId = 35, Tip = "Adverse action letters contain the specific reasons for denial — review these with the customer.", SortOrder = 2 },
                new CallTip { TipId = 301, IssueId = 35, Tip = "Do not guarantee approval on a reapplication — transfer to the branch for reapplication guidance.", SortOrder = 3 },
                new CallTip { TipId = 302, IssueId = 35, Tip = "The customer has the right to request a free credit report within 60 days of receiving an adverse action letter.", SortOrder = 4 },
                // 36: Restricted Contact
                new CallTip { TipId = 303, IssueId = 36, Tip = "Apply the appropriate restriction code immediately when a customer requests no further contact.", SortOrder = 1 },
                new CallTip { TipId = 304, IssueId = 36, Tip = "A Do Not Call request must be honored immediately and documented thoroughly.", SortOrder = 2 },
                new CallTip { TipId = 305, IssueId = 36, Tip = "If the customer requests a full cease of all communication, apply the full cease code and notify a supervisor.", SortOrder = 3 },
                // 37: Letter Request
                new CallTip { TipId = 306, IssueId = 37, Tip = "Confirm the customer's mailing address before submitting any letter request.", SortOrder = 1 },
                new CallTip { TipId = 307, IssueId = 37, Tip = "Common letter requests include payoff letters, account verification letters, and payment history.", SortOrder = 2 },
                new CallTip { TipId = 308, IssueId = 37, Tip = "Advise the customer of the processing time — typically 5-7 business days.", SortOrder = 3 },
                new CallTip { TipId = 309, IssueId = 37, Tip = "Payoff letters are typically valid for 10 days — inform the customer of the expiration date.", SortOrder = 4 },
                // 38: 3rd Party
                new CallTip { TipId = 310, IssueId = 38, Tip = "Verify the caller's identity and their relationship to the account holder before sharing any information.", SortOrder = 1 },
                new CallTip { TipId = 311, IssueId = 38, Tip = "Do not share account details with a third party unless the account holder has authorized them.", SortOrder = 2 },
                new CallTip { TipId = 312, IssueId = 38, Tip = "If the third party is a legal representative, request documentation before proceeding.", SortOrder = 3 },
                // 39: Non-World Customer
                new CallTip { TipId = 313, IssueId = 39, Tip = "Politely inform the caller they have reached the wrong number or company.", SortOrder = 1 },
                new CallTip { TipId = 314, IssueId = 39, Tip = "Do not provide any account information or attempt to assist with unrelated inquiries.", SortOrder = 2 },
                new CallTip { TipId = 315, IssueId = 39, Tip = "End the call professionally after clarifying the situation.", SortOrder = 3 },
                // 40: Spanish Call
                new CallTip { TipId = 316, IssueId = 40, Tip = "Greet the caller and immediately attempt to connect them with a Spanish-speaking representative.", SortOrder = 1 },
                new CallTip { TipId = 317, IssueId = 40, Tip = "If a Spanish speaker is unavailable, advise the customer of when one will be available.", SortOrder = 2 },
                new CallTip { TipId = 318, IssueId = 40, Tip = "Document that the call was in Spanish and the reason could not be determined if unable to assist.", SortOrder = 3 },
                // 41: Dead Air
                new CallTip { TipId = 319, IssueId = 41, Tip = "Greet the caller clearly and repeat the greeting at least twice before ending the call.", SortOrder = 1 },
                new CallTip { TipId = 320, IssueId = 41, Tip = "State your name and company clearly in case the caller can hear but cannot respond.", SortOrder = 2 },
                new CallTip { TipId = 321, IssueId = 41, Tip = "Disconnect after two attempts with no response and document the call.", SortOrder = 3 },
                // 42: Customer Hang Up
                new CallTip { TipId = 322, IssueId = 42, Tip = "Document the call as a customer hang up and note the time.", SortOrder = 1 },
                new CallTip { TipId = 323, IssueId = 42, Tip = "If the caller ID is available, a call back attempt may be appropriate.", SortOrder = 2 },
                // 43: Reason Not Provided
                new CallTip { TipId = 324, IssueId = 43, Tip = "Attempt to determine the reason for the call by asking open-ended questions.", SortOrder = 1 },
                new CallTip { TipId = 325, IssueId = 43, Tip = "For rollover calls, provide the branch contact information and offer to transfer.", SortOrder = 2 },
                new CallTip { TipId = 326, IssueId = 43, Tip = "Document the call and note that the reason was not provided despite attempts.", SortOrder = 3 },
                // 44: Callback No Answer
                new CallTip { TipId = 327, IssueId = 44, Tip = "Attempt the callback at least twice before marking as no answer.", SortOrder = 1 },
                new CallTip { TipId = 328, IssueId = 44, Tip = "Leave a voicemail if available — identify yourself and provide a callback number.", SortOrder = 2 },
                new CallTip { TipId = 329, IssueId = 44, Tip = "Document the callback attempt with date, time, and outcome.", SortOrder = 3 },
                // 45: Callback 3rd Party
                new CallTip { TipId = 330, IssueId = 45, Tip = "Do not leave detailed account information with a third party.", SortOrder = 1 },
                new CallTip { TipId = 331, IssueId = 45, Tip = "You may leave a general message asking the account holder to return the call.", SortOrder = 2 },
                new CallTip { TipId = 332, IssueId = 45, Tip = "Document the callback attempt and note that a third party answered.", SortOrder = 3 },
                // 46: Do Not Call
                new CallTip { TipId = 333, IssueId = 46, Tip = "Apply the Do Not Call code immediately — do not delay or ask clarifying questions first.", SortOrder = 1 },
                new CallTip { TipId = 334, IssueId = 46, Tip = "If the customer states they have previously requested no contact and are still being called, escalate to a supervisor immediately.", SortOrder = 2 },
                new CallTip { TipId = 335, IssueId = 46, Tip = "Even if a full cease is placed on the account, this term code is still required when do not call verbiage is used.", SortOrder = 3 },
                new CallTip { TipId = 336, IssueId = 46, Tip = "Document the customer's exact words and the time of the request.", SortOrder = 4 }
            );
        }
    }
}