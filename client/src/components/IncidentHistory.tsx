import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useMsal } from '@azure/msal-react';

import { IncidentHistory as IncidentHistoryType } from '../interfaces/Types';
import { getIncidentsByCustomer, getRecentIncidents } from '../api/incidents';
import Legend, { LegendItem } from './Legend';

import '../styles/IncidentHistory.css';

interface IncidentHistoryProps {
    ssn?: string; // If provided, loads history for that customer. If not, loads current user's recent incidents.
    currentIncidentId?: number;
    onIncidentSelect?: (incidentId: number) => void; // Optional callback for when an incident is selected
    scrollable?: boolean; // Optional prop to enable/disable scrollable container
}

const IncidentHistory: React.FC<IncidentHistoryProps> = ({ ssn, currentIncidentId, onIncidentSelect, scrollable = false }) => {
    const [history, setHistory] = useState<IncidentHistoryType[]>([]);
    const [loading, setLoading] = useState<boolean>(true);
    const navigate = useNavigate();
    const { instance } = useMsal();

    useEffect(() => {
        const fetchHistory = async () => {
            setLoading(true);
            try {
                if (ssn) {
                    const data = await getIncidentsByCustomer(instance, ssn);
                    setHistory(data);
                } else {
                    // Home page — load current user's recent incidents and map to history shape
                    const data = await getRecentIncidents(instance);
                    const mapped: IncidentHistoryType[] = data.map(i => ({
                        incidentId: i.incidentId,
                        createdAt: i.createdAt,
                        branch: i.branch,
                        account: i.account,
                        fullName: `${i.firstName} ${i.lastName}`,
                        phone: i.phone,
                        issue: i.issue,
                        solution: i.solution,
                        additionalDetails: i.additionalDetails
                    }));
                    setHistory(mapped);
                }
            } catch (err) {
                console.error('Failed to load incident history:', err);
            } finally {
                setLoading(false);
            }
        };

        fetchHistory();
    }, [ssn]);

    useEffect(() => {
        if (!scrollable) return;

        const scrollContainer = document.getElementById('incHistoryScrollContainer');
        const headerContainer = document.getElementById('incHistoryHeaderContainer');
        const headerTable = document.getElementById('incHistoryTable');

        if (!scrollContainer || !headerContainer || !headerTable) return;

        const checkScrollbar = () => {
            const hasScrollbar = scrollContainer.scrollHeight > scrollContainer.clientHeight;
            const headerPadding = headerContainer.offsetWidth - headerTable.offsetWidth;
            headerContainer.style.paddingRight = hasScrollbar ? `${headerPadding}px` : '0px';
        };

        checkScrollbar();
        window.addEventListener('resize', checkScrollbar);

        return () => window.removeEventListener('resize', checkScrollbar);
    }, [history, scrollable]);

    const handleRowClick = (incident: IncidentHistoryType) => {
        if (incident.incidentId === currentIncidentId) return;
        
        if (onIncidentSelect) {
            onIncidentSelect(incident.incidentId);
        } else {
            // Home page — navigate
            if (incident.account && incident.branch) {
                navigate(`/customerAccount/${incident.incidentId}/${incident.branch}/${incident.account}`);
            } else {
                navigate(`/noAccount/${incident.incidentId}`);
            }
        }
    };

    const formatDate = (isoString: string): string => {
        const date = new Date(isoString);
        return date.toLocaleDateString('en-US', {
            month: '2-digit',
            day: '2-digit',
            year: 'numeric',
            hour: '2-digit',
            minute: '2-digit'
        });
    };

    const isActiveIncident = (row: IncidentHistoryType): boolean =>
        row.incidentId === currentIncidentId;

    if (!scrollable) {
        const incidentLegendItems: LegendItem[] = [
            { color: '#ffffff', label: 'Open incident' },
            { color: '#f0faf2', label: 'Closed incident' },
            { color: '#fffff0', label: 'Hovered row' },
        ];

        return (
            <div id="incComponent">
                <div id="incHistoryTableContainer">
                    <div id="incHistoryTableWrapper">
                        <div id="incHistoryHeaderContainer">
                            <Legend items={incidentLegendItems} />
                            <table id="incHistoryTable">
                                <thead id="incHistoryHeader">
                                    <tr id="incHistoryHeaderRow" className="incHistoryHeaderRow incHistoryRow">
                                        <th className="incHistoryColHeader incHistoryDate"><span className="incHistorySpan">Date &<br />Time</span></th>
                                        <th className="incHistoryColHeader incHistoryBr"><span className="incHistorySpan">Br</span></th>
                                        <th className="incHistoryColHeader incHistoryAcc"><span className="incHistorySpan">Acc</span></th>
                                        <th className="incHistoryColHeader incHistoryName"><span className="incHistorySpan">Name</span></th>
                                        <th className="incHistoryColHeader incHistoryPhone"><span className="incHistorySpan">Phone</span></th>
                                        <th className="incHistoryColHeader incHistoryIssue"><span className="incHistorySpan">Issue</span></th>
                                        <th className="incHistoryColHeader incHistorySolution"><span className="incHistorySpan">Solution</span></th>
                                        <th className="incHistoryColHeader incHistoryAD"><span className="incHistorySpan">Additional Details</span></th>
                                    </tr>
                                </thead>
                                <tbody id="incHistoryBody">
                                    {loading ? (
                                        <tr><td colSpan={8}>Loading...</td></tr>
                                    ) : history.length === 0 ? (
                                        <tr>
                                            <td colSpan={8}>
                                                <div className="emptyState">
                                                    <svg xmlns="http://www.w3.org/2000/svg" width="64" height="64" fill="currentColor" className="bi bi-inbox" viewBox="0 0 16 16">
                                                        <path d="M4.98 4a.5.5 0 0 0-.39.188L1.54 8H6a.5.5 0 0 1 .5.5 1.5 1.5 0 1 0 3 0A.5.5 0 0 1 10 8h4.46l-3.05-3.812A.5.5 0 0 0 11.02 4zm9.954 5H10.45a2.5 2.5 0 0 1-4.9 0H1.066l.32 2.562a.5.5 0 0 0 .497.438h12.234a.5.5 0 0 0 .496-.438zM3.809 3.563A1.5 1.5 0 0 1 4.981 3h6.038a1.5 1.5 0 0 1 1.172.563l3.7 4.625a.5.5 0 0 1 .105.374l-.39 3.124A1.5 1.5 0 0 1 14.117 13H1.883a1.5 1.5 0 0 1-1.489-1.314l-.39-3.124a.5.5 0 0 1 .106-.374z"/>
                                                    </svg>
                                                    <span className="emptyStateText">No incident history</span>
                                                </div>
                                            </td>
                                        </tr>
                                    ) : (
                                        history.map((row) => (
                                            <tr
                                                key={row.incidentId}
                                                className={`incHistoryRow incHistoryBodyRow ${row.solution ? 'incHistoryClosedRow' : ''} ${isActiveIncident(row) ? 'incHistoryActiveRow' : ''}`}
                                                onClick={() => !isActiveIncident(row) ? handleRowClick(row) : undefined}
                                                style={{ cursor: isActiveIncident(row) ? 'default' : 'pointer' }}
                                            >
                                                <td className="incHistoryDate incHistoryData">
                                                    <span className="incHistorySpan incHistoryDateSpan">{formatDate(row.createdAt)}</span>
                                                </td>
                                                <td className="incHistoryBr incHistoryData">
                                                    <span className="incHistorySpan incHistoryBrSpan">{row.branch}</span>
                                                </td>
                                                <td className="incHistoryAcc incHistoryData">
                                                    <span className="incHistorySpan incHistoryAccSpan">{row.account ?? ''}</span>
                                                </td>
                                                <td className="incHistoryName incHistoryData">
                                                    <span className="incHistorySpan incHistoryNameSpan">{row.fullName}</span>
                                                </td>
                                                <td className="incHistoryPhone incHistoryData">
                                                    <span className="incHistorySpan incHistoryPhoneSpan">{row.phone ?? ''}</span>
                                                </td>
                                                <td className="incHistoryIssue incHistoryData">
                                                    <span className="incHistorySpan incHistoryIssueSpan">{row.issue}</span>
                                                </td>
                                                <td className="incHistorySolution incHistoryData">
                                                    <span className="incHistorySpan incHistorySolutionSpan">{row.solution ?? ''}</span>
                                                </td>
                                                <td className="incHistoryAD incHistoryData">
                                                    <span className="incHistorySpan incHistoryADSpan">{row.additionalDetails ?? ''}</span>
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
    }

    const incidentLegendItems: LegendItem[] = [
        { color: '#ffffff', label: 'Open incident' },
        { color: '#f0faf2', label: 'Closed incident' },
        { color: '#ebf8ff', label: 'Currently viewing' },
        { color: '#fffff0', label: 'Hovered row' },
    ];

    return (
        <div id="incComponent">
            <div id="incHistoryTableContainer">
                <div id="incHistoryTableWrapper">
                    <div id="incHistoryHeaderContainer">
                        <Legend items={incidentLegendItems} />
                        <table id="incHistoryTable">
                            <thead id="incHistoryHeader">
                                <tr id="incHistoryHeaderRow" className="incHistoryHeaderRow incHistoryRow">
                                    <th className="incHistoryColHeader incHistoryDate"><span className="incHistorySpan">Date &<br />Time</span></th>
                                    <th className="incHistoryColHeader incHistoryBr"><span className="incHistorySpan">Br</span></th>
                                    <th className="incHistoryColHeader incHistoryAcc"><span className="incHistorySpan">Acc</span></th>
                                    <th className="incHistoryColHeader incHistoryName"><span className="incHistorySpan">Name</span></th>
                                    <th className="incHistoryColHeader incHistoryPhone"><span className="incHistorySpan">Phone</span></th>
                                    <th className="incHistoryColHeader incHistoryIssue"><span className="incHistorySpan">Issue</span></th>
                                    <th className="incHistoryColHeader incHistorySolution"><span className="incHistorySpan">Solution</span></th>
                                    <th className="incHistoryColHeader incHistoryAD"><span className="incHistorySpan">Additional Details</span></th>
                                </tr>
                            </thead>
                        </table>
                    </div>
                    <div id="incHistoryScrollContainer">
                        <table id="incHistoryScrollTable">
                            <tbody id="incHistoryBody">
                                {loading ? (
                                    <tr><td colSpan={8}>Loading...</td></tr>
                                ) : history.length === 0 ? (
                                    <tr><td colSpan={8}>No incidents found.</td></tr>
                                ) : (
                                    history.map((row) => (
                                        <tr
                                            key={row.incidentId}
                                            className={`incHistoryRow incHistoryBodyRow ${row.solution ? 'incHistoryClosedRow' : ''} ${isActiveIncident(row) ? 'incHistoryActiveRow' : ''}`}
                                            onClick={() => !isActiveIncident(row) ? handleRowClick(row) : undefined}
                                            style={{ cursor: isActiveIncident(row) ? 'default' : 'pointer' }}
                                        >
                                            <td className="incHistoryDate incHistoryData">
                                                <span className="incHistorySpan incHistoryDateSpan">{formatDate(row.createdAt)}</span>
                                            </td>
                                            <td className="incHistoryBr incHistoryData">
                                                <span className="incHistorySpan incHistoryBrSpan">{row.branch}</span>
                                            </td>
                                            <td className="incHistoryAcc incHistoryData">
                                                <span className="incHistorySpan incHistoryAccSpan">{row.account ?? ''}</span>
                                            </td>
                                            <td className="incHistoryName incHistoryData">
                                                <span className="incHistorySpan incHistoryNameSpan">{row.fullName}</span>
                                            </td>
                                            <td className="incHistoryPhone incHistoryData">
                                                <span className="incHistorySpan incHistoryPhoneSpan">{row.phone ?? ''}</span>
                                            </td>
                                            <td className="incHistoryIssue incHistoryData">
                                                <span className="incHistorySpan incHistoryIssueSpan">{row.issue}</span>
                                            </td>
                                            <td className="incHistorySolution incHistoryData">
                                                <span className="incHistorySpan incHistorySolutionSpan">{row.solution ?? ''}</span>
                                            </td>
                                            <td className="incHistoryAD incHistoryData">
                                                <span className="incHistorySpan incHistoryADSpan">{row.additionalDetails ?? ''}</span>
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

export default IncidentHistory;