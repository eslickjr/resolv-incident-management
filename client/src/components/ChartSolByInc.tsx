import { useState } from "react";
import { ResponsiveContainer, PieChart, Pie, Cell, Tooltip, Legend } from "recharts";
import { SolutionsByIssueDto } from "../interfaces/Types";

const COLORS = ["#0d478e", "#1976d2", "#42a5f5", "#90caf9", "#FF8042", "#AA336A"];

interface ChartProps {
    data: SolutionsByIssueDto[];
}

const ChartSolByInc = ({ data }: ChartProps) => {
    const [selectedIssue, setSelectedIssue] = useState<string>('');

    const issues = [...new Set(data.map(d => d.issue))];

    const chartData = (selectedIssue
        ? data.filter(d => d.issue === selectedIssue)
        : data
    ).map(d => ({ name: d.solution, value: d.count }));

    return (
        <>
            <div style={{ display: 'flex', alignItems: 'center', gap: '10px', marginBottom: '12px' }}>
                <span className="chartTitle" style={{ margin: 0 }}>Solutions by Issue</span>
                <select
                    id="issueFilter"
                    value={selectedIssue}
                    onChange={e => setSelectedIssue(e.target.value)}
                >
                    <option value="">All Issues</option>
                    {issues.map(i => (
                        <option key={i} value={i}>{i}</option>
                    ))}
                </select>
            </div>
            <ResponsiveContainer width="100%" aspect={1.2}>
                <PieChart>
                    <Pie
                        data={chartData}
                        cx="50%"
                        cy="50%"
                        outerRadius={110}
                        dataKey="value"
                    >
                        {chartData.map((_, index) => (
                            <Cell key={`cell-${index}`} fill={COLORS[index % COLORS.length]} />
                        ))}
                    </Pie>
                    <Tooltip contentStyle={{ fontFamily: 'Work Sans', fontSize: 12 }} />
                    <Legend wrapperStyle={{ fontFamily: 'Work Sans', fontSize: 12 }} />
                </PieChart>
            </ResponsiveContainer>
        </>
    );
};

export default ChartSolByInc;