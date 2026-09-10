import { useNavigate } from 'react-router-dom';
import { useMsal } from '@azure/msal-react';
import { CallMatchResult, CallMatchItem } from '../interfaces/Types';
import { createIncident, getOpenIncidentBySSN } from '../api/incidents';
import '../styles/IncomingCallModal.css';

interface IncomingCallModalProps {
    result: CallMatchResult;
    onClose: () => void;
    onNewCustomer: (phone: string, firstName: string, lastName: string) => void;
}

const IncomingCallModal: React.FC<IncomingCallModalProps> = ({ result, onClose, onNewCustomer }) => {
    const navigate = useNavigate();
    const { instance } = useMsal();

    const handleIncidentSelect = (item: CallMatchItem) => {
        onClose();
        if (item.incidentId && item.branch && item.account) {
            navigate(`/customerAccount/${item.incidentId}/${item.branch}/${item.account}`);
        } else if (item.incidentId) {
            navigate(`/noAccount/${item.incidentId}`);
        }
    };

    const handleLoanSelect = async (item: CallMatchItem) => {
        if (!item.ssn) return;
        try {
            const rawSSN = item.ssn.replace(/\D/g, '');
            const openIncident = await getOpenIncidentBySSN(instance, rawSSN);
            onClose();
            if (openIncident) {
                if (item.branch && item.account) {
                    navigate(`/customerAccount/${openIncident.incidentId}/${item.branch}/${item.account}`);
                } else {
                    navigate(`/noAccount/${openIncident.incidentId}`);
                }
            } else {
                const incident = await createIncident(instance, {
                    firstName: item.fullName.split(' ')[0],
                    lastName: item.fullName.split(' ').slice(1).join(' '),
                    phone: result.phone,
                    ssn: item.ssn,
                    branch: item.branch,
                    account: item.account
                });
                if (incident) {
                    if (item.branch && item.account) {
                        navigate(`/customerAccount/${incident.incidentId}/${item.branch}/${item.account}`);
                    } else {
                        navigate(`/noAccount/${incident.incidentId}`);
                    }
                }
            }
        } catch (err) {
            console.error('Failed to handle loan select:', err);
        }
    };

    const incidentMatches = result.matches.filter(m => m.source === 'incident');
    const loanMatches = result.matches.filter(m => m.source === 'loan');

    return (
        <div id="incomingCallOverlay">
            <div id="incomingCallModal">
                <div id="incomingCallHeader">
                    <div id="incomingCallTitleRow">
                        <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
                            <path d="M22 16.92v3a2 2 0 0 1-2.18 2 19.79 19.79 0 0 1-8.63-3.07A19.5 19.5 0 0 1 4.69 12 19.79 19.79 0 0 1 1.63 3.4 2 2 0 0 1 3.62 1.22h3a2 2 0 0 1 2 1.72 12.84 12.84 0 0 0 .7 2.81 2 2 0 0 1-.45 2.11L7.91 8.84a16 16 0 0 0 6 6l.92-.92a2 2 0 0 1 2.11-.45 12.84 12.84 0 0 0 2.81.7A2 2 0 0 1 21.73 16z"/>
                        </svg>
                        <span id="incomingCallTitle">Incoming Call</span>
                    </div>
                    <span id="incomingCallClose" onClick={onClose}>✕</span>
                </div>
                <div id="incomingCallBody">
                    <div id="incomingCallInfo">
                        <span id="incomingCallName">{result.firstName} {result.lastName}</span>
                        <span id="incomingCallPhone">{result.phone}</span>
                    </div>

                    {incidentMatches.length > 0 && (
                        <div className="incomingCallSection">
                            <div className="incomingCallSectionTitle">Incidents</div>
                            {incidentMatches.map((item, i) => (
                                <div
                                    key={i}
                                    className={`incomingCallItem${item.closedAt ? ' incomingCallItemClosed' : ' incomingCallItemOpen'}`}
                                    onClick={() => handleIncidentSelect(item)}
                                >
                                    <div className="incomingCallItemName">{item.fullName}</div>
                                    <div className="incomingCallItemMeta">
                                        {item.issue && <span>{item.issue}</span>}
                                        {item.hasLoan && (
                                            <span className="incomingCallLoanBadge">+ Loan</span>
                                        )}
                                        <span className={`incomingCallBadge${item.closedAt ? ' incomingCallBadgeClosed' : ' incomingCallBadgeOpen'}`}>
                                            {item.closedAt ? 'Closed' : 'Open'}
                                        </span>
                                    </div>
                                </div>
                            ))}
                        </div>
                    )}

                    {loanMatches.length > 0 && (
                        <div className="incomingCallSection">
                            <div className="incomingCallSectionTitle">Loans</div>
                            {loanMatches.map((item, i) => (
                                <div
                                    key={i}
                                    className="incomingCallItem"
                                    onClick={() => handleLoanSelect(item)}
                                >
                                    <div className="incomingCallItemName">{item.fullName}</div>
                                    <div className="incomingCallItemMeta">
                                        <span>Br {item.branch} — Acc {item.account}</span>
                                    </div>
                                </div>
                            ))}
                        </div>
                    )}

                    <button
                        id="incomingCallNewBtn"
                        onClick={() => {
                            onClose();
                            onNewCustomer(result.phone ?? '', result.firstName ?? '', result.lastName ?? '');
                        }}
                    >
                        New Customer
                    </button>
                </div>
            </div>
        </div>
    );
};

export default IncomingCallModal;