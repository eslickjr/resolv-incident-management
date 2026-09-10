import { useState, useEffect } from 'react';
import { useMsal } from '@azure/msal-react';

import ChartIncPer from '../components/ChartIncPer';
import ChartIncType from '../components/ChartIncType';
import ChartSolByInc from '../components/ChartSolByInc';

import { getMyStats, getAllStats, getAggregateStats, StatsPeriod, StatsResponseDto, UserStatsDto } from '../api/stats.ts';

import '../styles/UserStats.css';

const UserStats = () => {
    const { instance } = useMsal();
    const [period, setPeriod] = useState<StatsPeriod>('month');
    const [stats, setStats] = useState<StatsResponseDto | null>(null);
    const [allUsers, setAllUsers] = useState<UserStatsDto[]>([]);
    const [selectedUser, setSelectedUser] = useState<string | null>(null);
    const [isAdmin, setIsAdmin] = useState<boolean>(false);
    const [loading, setLoading] = useState<boolean>(true);

    useEffect(() => {
        const fetchStats = async () => {
            setLoading(true);
            try {
                try {
                    const adminData = await getAllStats(instance, period);
                    setAllUsers(adminData);
                    setIsAdmin(true);
                    const data = await getAggregateStats(instance, period);
                    setStats(data);
                    setSelectedUser('all');
                } catch {
                    setIsAdmin(false);
                    const data = await getMyStats(instance, period);
                    setStats(data);
                }
            } catch (err) {
                console.error('Failed to load stats:', err);
            } finally {
                setLoading(false);
            }
        };

        fetchStats();
    }, [period]);

    const handleUserSelect = async (username: string | null) => {
        setSelectedUser(username);
        try {
            if (username === 'all' || username === null) {
                const data = await getAggregateStats(instance, period);
                setStats(data);
                setSelectedUser('all');
            } else {
                const data = await getMyStats(instance, period, username);
                setStats(data);
            }
        } catch (err) {
            console.error('Failed to load stats:', err);
        }
    };

    const calculateTime = (seconds: number): string => {
        const h = Math.floor(seconds / 3600);
        const m = Math.floor((seconds % 3600) / 60);
        const s = seconds % 60;
        return `${h}h ${m}m ${s}s`;
    };

    const totalIncidents = allUsers.reduce((sum, u) => sum + u.totalIncidents, 0);
    const totalTime = allUsers.reduce((sum, u) => sum + u.totalTimeSeconds, 0);
    const totalClosed = allUsers.reduce((sum, u) => sum + u.totalClosed, 0);
    const avgResolution = totalClosed > 0 ? Math.floor(totalTime / totalClosed) : 0;

    const summary = stats?.summary;

    return (
        <div id="statsPage">
            <h1 id="statsTitle">Stats</h1>

            {/* Period Toggle */}
            <div id="statsPeriodContainer">
                {(['day', 'week', 'month'] as StatsPeriod[]).map(p => (
                    <button
                        key={p}
                        className={`statsPeriodBtn${period === p ? ' statsPeriodSelected' : ''}`}
                        onClick={() => setPeriod(p)}
                    >
                        {p.charAt(0).toUpperCase() + p.slice(1)}
                    </button>
                ))}
            </div>

            {/* Summary Cards */}
            <div id="statsSummaryContainer">
                <div className="statsSummaryCard">
                    <div className="statsSummaryLabel">Total Incidents</div>
                    <div className="statsSummaryValue">{loading ? '—' : summary?.totalIncidents ?? 0}</div>
                </div>
                <div className="statsSummaryCard">
                    <div className="statsSummaryLabel">Opened</div>
                    <div className="statsSummaryValue">{loading ? '—' : summary?.totalOpened ?? 0}</div>
                </div>
                <div className="statsSummaryCard">
                    <div className="statsSummaryLabel">Closed</div>
                    <div className="statsSummaryValue">{loading ? '—' : summary?.totalClosed ?? 0}</div>
                </div>
                <div className="statsSummaryCard">
                    <div className="statsSummaryLabel">Total Time</div>
                    <div className="statsSummaryValue">{loading ? '—' : calculateTime(summary?.totalTimeSeconds ?? 0)}</div>
                </div>
                <div className="statsSummaryCard">
                    <div className="statsSummaryLabel">Avg Per Incident</div>
                    <div className="statsSummaryValue">{loading ? '—' : calculateTime(summary?.avgTimePerIncidentSeconds ?? 0)}</div>
                </div>
                <div className="statsSummaryCard">
                    <div className="statsSummaryLabel">Incidents / Hr</div>
                    <div className="statsSummaryValue">{loading ? '—' : summary?.incidentsPerHour ?? 0}</div>
                </div>
                <div className="statsSummaryCard">
                    <div className="statsSummaryLabel">Incidents / Day</div>
                    <div className="statsSummaryValue">{loading ? '—' : summary?.incidentsPerDay ?? 0}</div>
                </div>
            </div>

            {/* Charts */}
            <div id="statsChartsContainer">
                <div id="statsIncPerContainer">
                    <ChartIncPer
                        chartTitle={period === 'day' ? '24 Hours' : period === 'week' ? 'Week' : 'Month'}
                        data={stats?.incidentsByPeriod ?? []}
                    />
                </div>
                <div id="statsBottomCharts">
                    <div id="statsIncTypeContainer">
                        <ChartIncType data={stats?.incidentsByType ?? []} />
                    </div>
                    <div id="statsSolContainer">
                        <ChartSolByInc data={stats?.solutionsByIssue ?? []} />
                    </div>
                </div>
            </div>

            {/* Admin — All Users Table */}
            {isAdmin && (
                <div id="statsAdminContainer">
                    <h2 id="statsAdminTitle">All Users</h2>
                    <table id="statsAdminTable">
                        <thead>
                            <tr>
                                <th className="statsAdminHeader">Username</th>
                                <th className="statsAdminHeader">Total</th>
                                <th className="statsAdminHeader">Opened</th>
                                <th className="statsAdminHeader">Closed</th>
                                <th className="statsAdminHeader">Total Time</th>
                                <th className="statsAdminHeader">Avg Resolution</th>
                                <th className="statsAdminHeader">Avg Per Inc</th>
                                <th className="statsAdminHeader">Inc / Hr</th>
                                <th className="statsAdminHeader">Inc / Day</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr
                                className={`statsAdminRow${selectedUser === 'all' ? ' statsAdminRowSelected' : ''}`}
                                onClick={() => selectedUser !== 'all' ? handleUserSelect('all') : undefined}
                                style={{ cursor: selectedUser === 'all' ? 'default' : 'pointer' }}
                            >
                                <td className="statsAdminData" style={{ fontWeight: 600 }}>All Users</td>
                                <td className="statsAdminData">{totalIncidents}</td>
                                <td className="statsAdminData">{allUsers.reduce((sum, u) => sum + u.totalOpened, 0)}</td>
                                <td className="statsAdminData">{allUsers.reduce((sum, u) => sum + u.totalClosed, 0)}</td>
                                <td className="statsAdminData">{calculateTime(totalTime)}</td>
                                <td className="statsAdminData">{calculateTime(avgResolution)}</td>
                                <td className="statsAdminData">{totalIncidents > 0 ? calculateTime(Math.floor(totalTime / totalIncidents)) : '—'}</td>
                                <td className="statsAdminData">{allUsers.length > 0 ? Math.round(allUsers.reduce((sum, u) => sum + u.incidentsPerHour, 0) / allUsers.length * 100) / 100 : 0}</td>
                                <td className="statsAdminData">{allUsers.length > 0 ? Math.round(allUsers.reduce((sum, u) => sum + u.incidentsPerDay, 0) / allUsers.length * 100) / 100 : 0}</td>
                            </tr>
                            {allUsers.map((user, index) => (
                                <tr
                                    key={user.username}
                                    className={`statsAdminRow${selectedUser === user.username ? ' statsAdminRowSelected' : ''}${index % 2 === 0 ? '' : ' statsAdminRowEven'}`}
                                    onClick={() => selectedUser !== user.username ? handleUserSelect(user.username) : undefined}
                                    style={{ cursor: selectedUser === user.username ? 'default' : 'pointer' }}
                                >
                                    <td className="statsAdminData">{user.username}</td>
                                    <td className="statsAdminData">{user.totalIncidents}</td>
                                    <td className="statsAdminData">{user.totalOpened}</td>
                                    <td className="statsAdminData">{user.totalClosed}</td>
                                    <td className="statsAdminData">{calculateTime(user.totalTimeSeconds)}</td>
                                    <td className="statsAdminData">—</td>
                                    <td className="statsAdminData">{calculateTime(user.avgTimePerIncidentSeconds)}</td>
                                    <td className="statsAdminData">{user.incidentsPerHour}</td>
                                    <td className="statsAdminData">{user.incidentsPerDay}</td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                </div>
            )}
        </div>
    );
};

export default UserStats;