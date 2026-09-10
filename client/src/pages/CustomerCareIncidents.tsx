import { useLocation, useNavigate } from 'react-router-dom';
import { useEffect, useState } from 'react';
import { useMsal } from '@azure/msal-react';
import IncidentHistory from "../components/IncidentHistory";
import CustomerSearch from "../components/CustomerSearch";
import IncomingCallModal from "../components/IncomingCallModal";
import NewCustomer from "../components/NewCustomer";
import { getMyStats } from '../api/stats';
import { startCallHub } from '../api/callHub';
import { createIncident } from '../api/incidents';
import { CallMatchResult, StatsResponseDto } from '../interfaces/Types';
import '../styles/CustomerCareIncidents.css';

const CustomerCareIncidents = () => {
    const location = useLocation();
    const { instance } = useMsal();
    const navigate = useNavigate();
    const [callResult, setCallResult] = useState<CallMatchResult | null>(null);
    const [stats, setStats] = useState<StatsResponseDto | null>(null);
    const [newCustomerOpen, setNewCustomerOpen] = useState(false);
    const [prefilledPhone, setPrefilledPhone] = useState('');
    const [prefilledFirst, setPrefilledFirst] = useState('');
    const [prefilledLast, setPrefilledLast] = useState('');

    useEffect(() => {
        const fetchStats = async () => {
            try {
                const data = await getMyStats(instance, 'day');
                setStats(data);
            } catch (err) {
                console.error('Failed to load stats:', err);
            }
        };
        fetchStats();
    }, []);

    useEffect(() => {
        startCallHub(async (result: CallMatchResult) => {
            if (result.matchType === 'incident_exact') {
                if (result.incidentId) {
                    if (result.branch && result.account) {
                        navigate(`/customerAccount/${result.incidentId}/${result.branch}/${result.account}`);
                    } else {
                        navigate(`/noAccount/${result.incidentId}`);
                    }
                } else {
                    // All closed — create new
                    const incident = await createIncident(instance, {
                        firstName: result.firstName ?? '',
                        lastName: result.lastName ?? '',
                        phone: result.phone,
                        branch: result.branch ?? undefined,
                        account: result.account ?? undefined
                    });
                    if (incident) {
                        if (result.branch && result.account) {
                            navigate(`/customerAccount/${incident.incidentId}/${result.branch}/${result.account}`);
                        } else {
                            navigate(`/noAccount/${incident.incidentId}`);
                        }
                    }
                }
            } else if (result.matchType === 'loan_exact' || result.matchType === 'loan_exact_new') {
                if (result.matchType === 'loan_exact' && result.incidentId) {
                    if (result.branch && result.account) {
                        navigate(`/customerAccount/${result.incidentId}/${result.branch}/${result.account}`);
                    } else {
                        navigate(`/noAccount/${result.incidentId}`);
                    }
                } else {
                    const incident = await createIncident(instance, {
                        firstName: result.firstName ?? '',
                        lastName: result.lastName ?? '',
                        phone: result.phone,
                        branch: result.branch,
                        account: result.account
                    });
                    if (incident && result.branch && result.account) {
                        navigate(`/customerAccount/${incident.incidentId}/${result.branch}/${result.account}`);
                    } else if (incident) {
                        navigate(`/noAccount/${incident.incidentId}`);
                    }
                }
            } else if (result.matchType === 'no_match') {
                // Skip incoming call modal — go straight to new customer
                setPrefilledPhone(result.phone ?? '');
                setPrefilledFirst(result.firstName ?? '');
                setPrefilledLast(result.lastName ?? '');
                setNewCustomerOpen(true);
            } else {
                // phone_only or no_match — show modal
                setCallResult(result);
            }
        });
    }, []);

    const calculateTime = (seconds: number): string => {
        const h = Math.floor(seconds / 3600);
        const m = Math.floor((seconds % 3600) / 60);
        const s = seconds % 60;
        return `${h}h ${m}m ${s}s`;
    };

    return (
        <div id="CustomerCareIncidents">
            <div id="homeStatsStrip">
                <div className="homeStatCard">
                    <div className="homeStatLabel">Incidents Today</div>
                    <div className="homeStatValue">{stats?.summary.totalIncidents ?? '—'}</div>
                </div>
                <div className="homeStatCard">
                    <div className="homeStatLabel">Avg Handle Time</div>
                    <div className="homeStatValue">{stats ? calculateTime(stats.summary.averageResolutionSeconds) : '—'}</div>
                </div>
                <div className="homeStatCard">
                    <div className="homeStatLabel">Total Time</div>
                    <div className="homeStatValue">{stats ? calculateTime(stats.summary.totalTimeSeconds) : '—'}</div>
                </div>
            </div>
            <h1 id="searchTitle">Customer Search</h1>
            <CustomerSearch />
            <h1 id="incHistoryTitle">Incident History</h1>
            <IncidentHistory key={location.key} />
            {callResult && (
                <IncomingCallModal
                    result={callResult}
                    onClose={() => setCallResult(null)}
                    onNewCustomer={(phone, firstName, lastName) => {
                        setPrefilledPhone(phone);
                        setPrefilledFirst(firstName);
                        setPrefilledLast(lastName);
                        setNewCustomerOpen(true);
                    }}
                />
            )}
            <NewCustomer
                key={`${prefilledPhone}-${prefilledFirst}-${prefilledLast}`}
                isOpen={newCustomerOpen}
                onClose={() => setNewCustomerOpen(false)}
                prefilledPhone={prefilledPhone}
                prefilledFirstName={prefilledFirst}
                prefilledLastName={prefilledLast}
            />
        </div>
    );
}

export default CustomerCareIncidents;