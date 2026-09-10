// ── Search ────────────────────────────────────────────────────────────────────
export interface SearchResult {
    incidentId?: number;
    loanId?: number;
    fullName: string;
    firstName: string;
    lastName: string;
    ssn?: string;
    phone?: string;
    branch: string;
    account?: string;
    priority: number;
    closedAt?: string;
}

// ── Incident ──────────────────────────────────────────────────────────────────
export interface Incident {
    incidentId: number;
    branch: string;
    account?: string;
    phone?: string;
    ssn?: string;
    firstName: string;
    lastName: string;
    issue: string;
    solution?: string;
    additionalDetails?: string;
    createdBy: string;
    createdAt: string;
    closedBy?: string;
    closedAt?: string;
}

// ── Incident History ──────────────────────────────────────────────────────────
export interface IncidentHistory {
    incidentId: number;
    createdAt: string;
    branch: string;
    account?: string;
    fullName: string;
    phone?: string;
    issue: string;
    solution?: string;
    additionalDetails?: string;
}

// ── Incident Stat Tracking ────────────────────────────────────────────────────
export interface IncidentStatTracking {
    statId: number;
    incidentId: number;
    username: string;
    opened: boolean;
    timeSpentSeconds: number;
    closed: boolean;
}

// ── Create / Update Requests ──────────────────────────────────────────────────
export interface CreateIncidentRequest {
    firstName: string;
    lastName: string;
    ssn?: string;
    phone?: string;
    branch?: string;
    account?: string;
}

export interface UpdateIncidentRequest {
    branch?: string;
    phone?: string;
    ssn?: string;
    firstName?: string;
    lastName?: string;
    issue?: string;
    solution?: string;
    additionalDetails?: string;
    timeSpentSeconds: number;
    isClosing: boolean;
}

// ── Loan ──────────────────────────────────────────────────────────────────────
export interface Loan {
    loanId: number;
    branchCode: string;
    loanClass?: string;
    accountNumber?: string;
    statusCodes?: string;
    firstName: string;
    lastName: string;
    taxId?: string;
    streetAddress?: string;
    mobilePhone?: string;
    homePhone?: string;
    originationDate?: string;
    loanAmount?: number;
    netProceeds?: number;
    principalBalance?: number;
    payoffAmount?: number;
    delinquentAmount?: number;
    chargeOffDate?: string;
    firstPaymentDate?: string;
    scheduledPayment?: number;
    amountDue?: number;
    nextDueDate?: string;
    nextDueAmount?: number;
    lastDueDate?: string;
    lastPaymentDate?: string;
    lastPaymentAmount?: number;
    loanReference?: string;
}

// ── Loan Payment ──────────────────────────────────────────────────────────────
export interface LoanPayment {
    transactionId: number;
    loanReference: string;
    transactionDate: string;
    transactionCode?: string;
    confirmationNumber?: string;
    transactionAmount: number;
    principalApplied: number;
    paidThroughDate?: string;
    feesApplied: number;
}

// ── Loan History ──────────────────────────────────────────────────────────────
export interface LoanHistory {
    loanId: number;
    originationDate?: string;
    branchCode: string;
    accountNumber?: string;
    firstName: string;
    lastName: string;
    loanAmount?: number;
    netProceeds?: number;
    scheduledPayment?: number;
    chargeOffDate?: string;
    lastPaymentDate?: string;
    loanReference?: string;
}

// ── Stats ─────────────────────────────────────────────────────────────────────
export interface SolutionsByIssueDto {
    issue: string;
    solution: string;
    count: number;
}

export interface UserStatsDto {
    username: string;
    totalIncidents: number;
    totalOpened: number;
    totalClosed: number;
    incidentsPerHour: number;
    incidentsPerDay: number;
    avgTimePerIncidentSeconds: number;
    totalTimeSeconds: number;
    averageResolutionSeconds: number;
}

export interface StatsResponseDto {
    summary: UserStatsDto;
    incidentsByPeriod: IncidentsByPeriodDto[];
    incidentsByType: IncidentsByTypeDto[];
    solutionsByIssue: SolutionsByIssueDto[];
}

export interface IncidentsByPeriodDto {
    label: string;
    count: number;
}

export interface IncidentsByTypeDto {
    issue: string;
    count: number;
}

export interface IncidentNote {
    noteId: number;
    incidentId: number;
    username: string;
    note: string;
    createdAt: string;
}

// ── Call Webhook ──────────────────────────────────────────────────────────────
export interface CallMatchItem {
    source: string;
    incidentId?: number;
    branch?: string;
    account?: string;
    fullName: string;
    phone?: string;
    issue?: string;
    closedAt?: string;
    ssn?: string;
    hasLoan: boolean;
}

export interface CallMatchResult {
    matchType: string;
    incidentId?: number;
    branch?: string;
    account?: string;
    phone?: string;
    firstName?: string;
    lastName?: string;
    matches: CallMatchItem[];
}