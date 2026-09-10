-- ============================================================
-- Incident Management — SQL Server Schema
-- ============================================================

-- ── Incidents ─────────────────────────────────────────────────────────────────
CREATE TABLE Incidents (
    IncidentId       INT            IDENTITY(1,1) PRIMARY KEY,
    Branch           NVARCHAR(10)   NOT NULL,
    Phone            NVARCHAR(20)   NULL,
    SSN              NVARCHAR(11)   NULL,          -- stored as XXX-XX-XXXX
    FirstName        NVARCHAR(100)  NOT NULL,
    LastName         NVARCHAR(100)  NOT NULL,
    Issue            NVARCHAR(MAX)  NOT NULL,
    Solution         NVARCHAR(MAX)  NULL,
    AdditionalDetails NVARCHAR(MAX) NULL,
    CreatedBy        NVARCHAR(100)  NOT NULL,
    CreatedAt        DATETIME2      NOT NULL DEFAULT SYSUTCDATETIME(),
    ClosedBy         NVARCHAR(100)  NULL,
    ClosedAt         DATETIME2      NULL
);

CREATE INDEX IX_Incidents_Branch    ON Incidents (Branch);
CREATE INDEX IX_Incidents_SSN       ON Incidents (SSN);
CREATE INDEX IX_Incidents_CreatedAt ON Incidents (CreatedAt);
CREATE INDEX IX_Incidents_ClosedAt  ON Incidents (ClosedAt);

-- ── IncidentStatTracking ──────────────────────────────────────────────────────
CREATE TABLE IncidentStatTracking (
    StatId           INT            IDENTITY(1,1) PRIMARY KEY,
    IncidentId       INT            NOT NULL
                                    REFERENCES Incidents(IncidentId) ON DELETE CASCADE,
    Username         NVARCHAR(100)  NOT NULL,
    Opened           BIT            NOT NULL DEFAULT 0,
    TimeSpentSeconds INT            NOT NULL DEFAULT 0,
    Closed           BIT            NOT NULL DEFAULT 0
);

CREATE INDEX IX_StatTracking_IncidentId ON IncidentStatTracking (IncidentId);
CREATE INDEX IX_StatTracking_Username   ON IncidentStatTracking (Username);

-- ── Loans ─────────────────────────────────────────────────────────────────────
CREATE TABLE Loans (
    LoanId           INT            IDENTITY(1,1) PRIMARY KEY,

    -- General Loan Information
    Branch           NVARCHAR(10)   NOT NULL,
    Class            NVARCHAR(50)   NULL,
    Account          NVARCHAR(50)   NULL,
    Codes            NVARCHAR(200)  NULL,
    FirstName        NVARCHAR(100)  NOT NULL,
    LastName         NVARCHAR(100)  NOT NULL,
    SSN              NVARCHAR(11)   NULL,
    Address          NVARCHAR(300)  NULL,
    Cell             NVARCHAR(20)   NULL,

    -- Loan Information
    LoanDate         DATE           NULL,
    LoanAmount       DECIMAL(18,2)  NULL,
    Proceeds         DECIMAL(18,2)  NULL,
    Balance          DECIMAL(18,2)  NULL,
    Payoff           DECIMAL(18,2)  NULL,
    Delinquency      DECIMAL(18,2)  NULL,
    ChargeOffDate    DATE           NULL,

    -- Payment Information
    FirstPayDate     DATE           NULL,
    PaymentAmount    DECIMAL(18,2)  NULL,
    AmountDue        DECIMAL(18,2)  NULL,
    NextDueDate      DATE           NULL,
    LastDueDate      DATE           NULL
);

CREATE INDEX IX_Loans_Branch  ON Loans (Branch);
CREATE INDEX IX_Loans_SSN     ON Loans (SSN);
CREATE INDEX IX_Loans_Account ON Loans (Account);

-- ── LoanPaymentHistory ────────────────────────────────────────────────────────
CREATE TABLE LoanPaymentHistory (
    PaymentId            INT           IDENTITY(1,1) PRIMARY KEY,
    LoanId               INT           NOT NULL
                                       REFERENCES Loans(LoanId) ON DELETE CASCADE,
    PaymentDate          DATE          NOT NULL,
    Code                 NVARCHAR(50)  NULL,
    ReferenceNumber      NVARCHAR(100) NULL,
    Amount               DECIMAL(18,2) NOT NULL DEFAULT 0,
    Principal            DECIMAL(18,2) NOT NULL DEFAULT 0,
    PaidThrough          DATE          NULL,
    InterestOrLateCharge DECIMAL(18,2) NOT NULL DEFAULT 0
);

CREATE INDEX IX_LoanPaymentHistory_LoanId      ON LoanPaymentHistory (LoanId);
CREATE INDEX IX_LoanPaymentHistory_PaymentDate ON LoanPaymentHistory (PaymentDate);

-- ============================================================
-- Views (History — keyless read-only projections)
-- ============================================================

-- ── vw_IncidentHistory ────────────────────────────────────────────────────────
CREATE OR ALTER VIEW vw_IncidentHistory AS
SELECT
    i.IncidentId,
    i.CreatedAt,
    i.Branch,
    NULL                                        AS Account,   -- extend when Loan is linked
    CONCAT(i.FirstName, ' ', i.LastName)        AS FullName,
    i.SSN,
    i.Phone,
    i.Issue,
    i.Solution                                  AS Action,
    i.AdditionalDetails
FROM Incidents i;

-- ── vw_LoanHistory ────────────────────────────────────────────────────────────
CREATE OR ALTER VIEW vw_LoanHistory AS
SELECT
    l.LoanId,
    CAST(l.LoanDate AS DATETIME2)               AS LoanDateTime,
    l.Branch,
    l.Account                                   AS AccountNumber,
    CONCAT(l.FirstName, ' ', l.LastName)        AS FullName,
    l.LoanAmount,
    l.Proceeds,
    l.PaymentAmount,
    CASE
        WHEN l.ChargeOffDate IS NOT NULL
            THEN CONCAT('Charge-Off: ', FORMAT(l.ChargeOffDate, 'MM/dd/yyyy'))
        WHEN l.Balance = 0 AND l.LoanDate IS NOT NULL
            THEN CONCAT('Paidoff: ', FORMAT(l.LastDueDate, 'MM/dd/yyyy'))
        ELSE NULL
    END                                         AS ClosedDate
FROM Loans l;
