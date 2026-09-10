import { useEffect, useState } from "react";
import { useMsal } from "@azure/msal-react";
import { LoanPayment } from "../interfaces/Types";
import { getPaymentHistory } from "../api/loans";

import '../styles/PaymentHistory.css';

interface PaymentHistoryProps {
    branch?: string;
    account?: string;
}

const PaymentHistory: React.FC<PaymentHistoryProps> = ({ branch, account }) => {
    const { instance } = useMsal();
    const [payments, setPayments] = useState<LoanPayment[]>([]);
    const [loading, setLoading] = useState<boolean>(true);

    useEffect(() => {
        const fetchPayments = async () => {
            if (!branch || !account) { setLoading(false); return; }
            setLoading(true);
            try {
                const data = await getPaymentHistory(instance, branch, account);
                setPayments(data);
            } catch (err) {
                console.error('Failed to load payment history:', err);
            } finally {
                setLoading(false);
            }
        };

        fetchPayments();
    }, [branch, account]);

    useEffect(() => {
        const scrollContainer = document.getElementById('pmtHistoryScrollContainer');
        const headerContainer = document.getElementById('pmtHistoryHeaderContainer');
        const headerTable = document.getElementById('pmtHistoryHeaderTable');

        if (!scrollContainer || !headerContainer || !headerTable) return;

        const checkScrollbar = () => {
            const hasScrollbar = scrollContainer.scrollHeight > scrollContainer.clientHeight;
            const headerPadding = headerContainer.offsetWidth - headerTable.offsetWidth;
            headerContainer.style.paddingRight = hasScrollbar ? `${headerPadding}px` : '0px';
        };

        checkScrollbar();
        window.addEventListener('resize', checkScrollbar);

        return () => window.removeEventListener('resize', checkScrollbar);
    }, [payments]);

    const formatCurrency = (val: number): string =>
        `$${val.toLocaleString('en-US', { minimumFractionDigits: 2 })}`;

    const formatDate = (val?: string): string => {
        if (!val) return '—';
        return new Date(val).toLocaleDateString('en-US');
    };

    return (
        <div>
            <div id="pmtHistoryTableContainer">
                <div id="pmtHistoryTableWrapper">
                    <div id="pmtHistoryHeaderContainer">
                        <table id="pmtHistoryTable">
                            <thead id="pmtHistoryHeader">
                                <tr id="pmtHistoryHeaderRow" className="pmtHistoryHeaderRow pmtHistoryRow">
                                    <th className="pmtHistoryColHeader pmtHistoryDate"><span className="pmtHistorySpan">Date</span></th>
                                    <th className="pmtHistoryColHeader pmtHistoryCode"><span className="pmtHistorySpan">Code</span></th>
                                    <th className="pmtHistoryColHeader pmtHistoryRef"><span className="pmtHistorySpan">Reference</span></th>
                                    <th className="pmtHistoryColHeader pmtHistoryAmount"><span className="pmtHistorySpan">Amount</span></th>
                                    <th className="pmtHistoryColHeader pmtHistoryPrincipal"><span className="pmtHistorySpan">Principal</span></th>
                                    <th className="pmtHistoryColHeader pmtHistoryPaidThru"><span className="pmtHistorySpan">Paid Through</span></th>
                                    <th className="pmtHistoryColHeader pmtHistoryCharge"><span className="pmtHistorySpan">Interest/Late Charge</span></th>
                                </tr>
                            </thead>
                        </table>
                    </div>
                    <div id="pmtHistoryScrollContainer">
                        <table id="pmtHistoryScrollTable">
                            <tbody id="pmtHistoryBody">
                                {loading ? (
                                    <tr><td colSpan={7}>Loading...</td></tr>
                                ) : payments.length === 0 ? (
                                    <tr>
                                        <td colSpan={7}>
                                            <div className="emptyState">
                                                <svg xmlns="http://www.w3.org/2000/svg" width="64" height="64" fill="currentColor" className="bi bi-inbox" viewBox="0 0 16 16">
                                                    <path d="M4.98 4a.5.5 0 0 0-.39.188L1.54 8H6a.5.5 0 0 1 .5.5 1.5 1.5 0 1 0 3 0A.5.5 0 0 1 10 8h4.46l-3.05-3.812A.5.5 0 0 0 11.02 4zm9.954 5H10.45a2.5 2.5 0 0 1-4.9 0H1.066l.32 2.562a.5.5 0 0 0 .497.438h12.234a.5.5 0 0 0 .496-.438zM3.809 3.563A1.5 1.5 0 0 1 4.981 3h6.038a1.5 1.5 0 0 1 1.172.563l3.7 4.625a.5.5 0 0 1 .105.374l-.39 3.124A1.5 1.5 0 0 1 14.117 13H1.883a1.5 1.5 0 0 1-1.489-1.314l-.39-3.124a.5.5 0 0 1 .106-.374z"/>
                                                </svg>
                                                <span className="emptyStateText">No payment history</span>
                                            </div>
                                        </td>
                                    </tr>
                                ) : (
                                    payments.map((payment, index) => (
                                        <tr key={index} className="pmtHistoryRow pmtHistoryBodyRow">
                                            <td className="pmtHistoryDate pmtHistoryData">
                                                <span className="pmtHistorySpan pmtHistoryDateSpan">{formatDate(payment.transactionDate)}</span>
                                            </td>
                                            <td className="pmtHistoryCode pmtHistoryData">
                                                <span className="pmtHistorySpan pmtHistoryCodeSpan">{payment.transactionCode ?? '—'}</span>
                                            </td>
                                            <td className="pmtHistoryRef pmtHistoryData">
                                                <span className="pmtHistorySpan pmtHistoryRefSpan">{payment.confirmationNumber ?? '—'}</span>
                                            </td>
                                            <td className="pmtHistoryAmount pmtHistoryData">
                                                <span className="pmtHistorySpan pmtHistoryAmountSpan">{formatCurrency(payment.transactionAmount)}</span>
                                            </td>
                                            <td className="pmtHistoryPrincipal pmtHistoryData">
                                                <span className="pmtHistorySpan pmtHistoryPrincipalSpan">{formatCurrency(payment.principalApplied)}</span>
                                            </td>
                                            <td className="pmtHistoryPaidThru pmtHistoryData">
                                                <span className="pmtHistorySpan pmtHistoryPaidThruSpan">{formatDate(payment.paidThroughDate)}</span>
                                            </td>
                                            <td className="pmtHistoryCharge pmtHistoryData">
                                                <span className="pmtHistorySpan pmtHistoryChargeSpan">{formatCurrency(payment.feesApplied)}</span>
                                            </td>
                                        </tr>
                                    ))
                                )}
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default PaymentHistory;