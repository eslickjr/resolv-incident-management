import { useEffect, useState } from "react";
import { useMsal } from "@azure/msal-react";
import { LoanHistory as LoanHistoryType } from "../interfaces/Types";
import { getLoanHistory } from "../api/loans";
import Legend, { LegendItem } from "./Legend";

import '../styles/LoanHistory.css';

interface LoanHistoryProps {
    ssn?: string;
    currentBranch?: string;
    currentAccount?: string;
    onLoanSelect?: (branch: string, account: string) => void;
}

const LoanHistory: React.FC<LoanHistoryProps> = ({
    ssn,
    currentBranch,
    currentAccount,
    onLoanSelect
}) => {
    const { instance } = useMsal();
    const [history, setHistory] = useState<LoanHistoryType[]>([]);
    const [loading, setLoading] = useState<boolean>(true);

    useEffect(() => {
        const fetchHistory = async () => {
            if (!ssn) { setLoading(false); return; }
            setLoading(true);
            try {
                const data = await getLoanHistory(instance, ssn);
                setHistory(data);
            } catch (err) {
                console.error('Failed to load loan history:', err);
            } finally {
                setLoading(false);
            }
        };

        fetchHistory();
    }, [ssn]);

    useEffect(() => {
        const scrollContainer = document.getElementById('loanHistoryScrollContainer');
        const headerContainer = document.getElementById('loanHistoryHeaderContainer');
        const headerTable = document.getElementById('loanHistoryHeaderTable');

        if (!scrollContainer || !headerContainer || !headerTable) return;

        const checkScrollbar = () => {
            const hasScrollbar = scrollContainer.scrollHeight > scrollContainer.clientHeight;
            const headerPadding = headerContainer.offsetWidth - headerTable.offsetWidth;
            headerContainer.style.paddingRight = hasScrollbar ? `${headerPadding}px` : '0px';
        };

        checkScrollbar();
        window.addEventListener('resize', checkScrollbar);

        return () => window.removeEventListener('resize', checkScrollbar);
    }, [history]);

    const loanLegendItems: LegendItem[] = [
        { color: '#ffffff', label: 'Loan' },
        { color: '#ebf8ff', label: 'Currently viewing loan' },
        { color: '#fffff0', label: 'Hovered row' },
    ];

    const formatCurrency = (val?: number | null): string =>
        val != null ? `$${val.toLocaleString('en-US', { minimumFractionDigits: 2 })}` : '—';

    const formatDate = (isoString?: string): string => {
        if (!isoString) return '—';

        const d = new Date(isoString);
        return d.toLocaleDateString('en-US', {
            month: '2-digit',
            day: '2-digit',
            year: 'numeric'
        });
    };

    const isActiveLoan = (row: LoanHistoryType): boolean => {
        const normalizedRowBranch = row.branchCode?.replace(/^0+/, '');
        const normalizedCurrentBranch = currentBranch?.replace(/^0+/, '');
        return normalizedRowBranch === normalizedCurrentBranch && row.accountNumber === currentAccount;
    };

    const handleRowClick = (row: LoanHistoryType) => {
        if (isActiveLoan(row)) return;
        if (onLoanSelect && row.branchCode && row.accountNumber) {
            onLoanSelect(row.branchCode, row.accountNumber);
        }
    };

    return (
        <div>
            <div id="loanHistoryTableContainer">
                <div id="loanHistoryTableWrapper">
                    <div id="loanHistoryHeaderContainer">
                        <Legend items={loanLegendItems} />
                        <table id="loanHistoryHeaderTable">
                            <thead id="loanHistoryHeader">
                                <tr id="loanHistoryHeaderRow" className="loanHistoryHeaderRow loanHistoryRow">
                                    <th className="loanHistoryColHeader loanHistoryDate"><span className="loanHistorySpan">Date &<br />Time</span></th>
                                    <th className="loanHistoryColHeader loanHistoryBr"><span className="loanHistorySpan">Branch</span></th>
                                    <th className="loanHistoryColHeader loanHistoryAcc"><span className="loanHistorySpan">Account</span></th>
                                    <th className="loanHistoryColHeader loanHistoryName"><span className="loanHistorySpan">Name</span></th>
                                    <th className="loanHistoryColHeader loanHistoryAmount"><span className="loanHistorySpan">Loan Amount</span></th>
                                    <th className="loanHistoryColHeader loanHistoryProceeds"><span className="loanHistorySpan">Proceeds</span></th>
                                    <th className="loanHistoryColHeader loanHistoryPmtAmount"><span className="loanHistorySpan">Payment Amount</span></th>
                                    <th className="loanHistoryColHeader loanHistoryOff"><span className="loanHistorySpan">Closed Date</span></th>
                                </tr>
                            </thead>
                        </table>
                    </div>
                    <div id="loanHistoryScrollContainer">
                        <table id="loanHistoryScrollTable">
                            <tbody id="loanHistoryBody">
                                {loading ? (
                                    <tr><td colSpan={8}>Loading...</td></tr>
                                ) : history.length === 0 ? (
                                    <tr><td colSpan={8}>No loan history found.</td></tr>
                                ) : (
                                    history.map((row, index) => {
                                        const active = isActiveLoan(row);
                                        return (
                                            <tr
                                                key={index}
                                                className={`loanHistoryRow loanHistoryBodyRow${active ? ' loanHistoryActiveRow' : ''}`}
                                                onClick={() => handleRowClick(row)}
                                                style={{ cursor: active ? 'default' : 'pointer' }}
                                            >
                                                <td className="loanHistoryDate loanHistoryData"><span className="loanHistorySpan loanHistoryDateSpan">{formatDate(row.originationDate)}</span></td>
                                                <td className="loanHistoryBr loanHistoryData"><span className="loanHistorySpan loanHistoryBrSpan">{row.branchCode}</span></td>
                                                <td className="loanHistoryAcc loanHistoryData"><span className="loanHistorySpan loanHistoryAccSpan">{row.accountNumber ?? '—'}</span></td>
                                                <td className="loanHistoryName loanHistoryData"><span className="loanHistorySpan loanHistoryNameSpan">{row.firstName} {row.lastName}</span></td>
                                                <td className="loanHistoryAmount loanHistoryData"><span className="loanHistorySpan loanHistoryAmountSpan">{formatCurrency(row.loanAmount)}</span></td>
                                                <td className="loanHistoryProceeds loanHistoryData"><span className="loanHistorySpan loanHistoryProceedsSpan">{formatCurrency(row.netProceeds)}</span></td>
                                                <td className="loanHistoryPmtAmount loanHistoryData"><span className="loanHistorySpan loanHistoryPmtAmountSpan">{formatCurrency(row.scheduledPayment)}</span></td>
                                                <td className="loanHistoryOff loanHistoryData"><span className="loanHistorySpan loanHistoryOffSpan">{row.lastPaymentDate ?? ''}</span></td>
                                            </tr>
                                        );
                                    })
                                )}
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default LoanHistory;