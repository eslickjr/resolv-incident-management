import { useState, useEffect } from "react";
import PaymentHistory from "./PaymentHistory";
import PaymentProcessor from "./PaymentProcessor";
import { Loan as LoanType } from "../interfaces/Types";

import '../styles/Loan.css';

interface LoanProps {
    loan: LoanType | null;
    incidentId?: number;
}

const Loan: React.FC<LoanProps> = ({ loan: loanProp, incidentId }) => {
    const [loan, setLoan] = useState<LoanType | null>(loanProp ?? null);

    useEffect(() => {
        setLoan(loanProp ?? null);
    }, [loanProp]);

    const formatCurrency = (val?: number | null): string =>
        val != null ? `$${val.toLocaleString('en-US', { minimumFractionDigits: 2 })}` : '—';

    const formatDate = (val?: string | null): string => {
        if (!val) return '—';
        const d = new Date(val);
        return d.toLocaleDateString('en-US');
    };

    const maskSSN = (ssn?: string | null): string => {
        if (!ssn) return '—';
        const digits = ssn.replace(/\D/g, '');
        if (digits.length !== 9) return ssn;
        return `***-**-${digits.slice(5)}`;
    };

    const formatPhone = (val?: string | null): string => {
        if (!val) return '—';
        const digits = val.replace(/\D/g, '');
        if (!digits) return '—';
        let formatted = '(' + digits.substring(0, 3);
        if (digits.length >= 4) formatted += ') ' + digits.substring(3, 6);
        if (digits.length >= 7) formatted += '-' + digits.substring(6, 10);
        return formatted;
    };

    return (
        <div id="loanComponent">
            <div id="outerLoanContainer">
                <div id="loanContainer">
                    <div id="loanInformationContainer">
                        {loan ? (
                            <>
                                <div id="generalCustInfo" className="broadInfoContainer">
                                    <div className="loanInfoTitleContainer">
                                        <h3 className="loanInfoTitle">General Information</h3>
                                    </div>
                                    <div id="generalInfoContainer" className="infoContainer">
                                        <p>Branch: <span className="infoSpan">{loan.branchCode}</span></p>
                                        <div id="accInfo" className="info">
                                            <p>Class: <span className="infoSpan">{loan.loanClass ?? '—'}</span></p>
                                            <p>Account: <span className="infoSpan">{loan.accountNumber ?? '—'}</span></p>
                                            <p>Codes: <span className="infoSpan">{loan.statusCodes ?? '—'}</span></p>
                                        </div>
                                        <div id="nameInfo" className="info">
                                            <p>First Name: <span className="infoSpan">{loan.firstName}</span></p>
                                            <p>Last Name: <span className="infoSpan">{loan.lastName}</span></p>
                                        </div>
                                        <p>SSN: <span className="infoSpan">{maskSSN(loan.taxId)}</span></p>
                                        <p>Address: <span className="infoSpan">{loan.streetAddress ?? '—'}</span></p>
                                        <p>Cell: <span className="infoSpan">{formatPhone(loan.mobilePhone ?? '—')}</span></p>
                                    </div>
                                </div>
                                <div id="loanInfo" className="broadInfoContainer">
                                    <div className="loanInfoTitleContainer">
                                        <h3 className="loanInfoTitle">Loan Information</h3>
                                    </div>
                                    <div id="loanInfoContainer" className="infoContainer">
                                        <p>Loan Date: <span className="infoSpan">{formatDate(loan.originationDate ?? '—')}</span></p>
                                        <div id="loanAmtInfo" className="info">
                                            <p>Loan Amount: <span className="infoSpan">{formatCurrency(loan.loanAmount)}</span></p>
                                            <p>Proceeds: <span className="infoSpan">{formatCurrency(loan.netProceeds)}</span></p>
                                        </div>
                                        <div id="balanceInfo" className="info">
                                            <p>Balance: <span className="infoSpan">{formatCurrency(loan.principalBalance)}</span></p>
                                            <p>Payoff: <span className="infoSpan">{formatCurrency(loan.payoffAmount)}</span></p>
                                        </div>
                                        <p>Delinquency: <span className="infoSpan">{formatCurrency(loan.delinquentAmount)}</span></p>
                                        <p>Charge-Off Date: <span className="infoSpan">{formatDate(loan.chargeOffDate)}</span></p>
                                    </div>
                                </div>
                                <div id="pmtInfo" className="broadInfoContainer">
                                    <div className="loanInfoTitleContainer">
                                        <h3 className="loanInfoTitle">Payment Information</h3>
                                    </div>
                                    <div id="pmtInfoContainer" className="infoContainer">
                                        <p>First Pay: <span className="infoSpan">{formatDate(loan.firstPaymentDate ?? '—')}</span></p>
                                        <p>Payment Amount: <span className="infoSpan">{formatCurrency(loan.scheduledPayment ?? 0)}</span></p>
                                        <p>Amount Due: <span className="infoSpan">{formatCurrency(loan.amountDue)}</span></p>
                                        <p>Next Due: <span className="infoSpan">{formatDate(loan.nextDueDate)}</span>{loan.nextDueDate ? <> <span className="infoSpan">{formatCurrency(loan.nextDueAmount)}</span></> : null}</p>
                                        <p>Last Due Date: <span className="infoSpan">{formatDate(loan.lastDueDate)}</span></p>
                                        <p>Paid Last: <span className="infoSpan">{formatDate(loan.lastPaymentDate ?? '—')}</span>{loan.lastPaymentDate ? <> <span className="infoSpan">{formatCurrency(loan.lastPaymentAmount ?? 0)}</span></> : null}</p>
                                    </div>
                                </div>
                            </>
                        ) : (
                            <p>No loan information available.</p>
                        )}
                    </div>
                    <PaymentProcessor loan={loan} incidentId={incidentId} />
                    <div id="paymentHistory">
                        <div id="pmtHistoryTitleContainer">
                            <h3 id="pmtHistoryTitle">Payment History</h3>
                        </div>
                        <PaymentHistory
                            branch={loan?.branchCode}
                            account={loan?.accountNumber}
                        />
                    </div>
                </div>
            </div>
        </div>
    );
};

export default Loan;