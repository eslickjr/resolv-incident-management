import { ResponsiveContainer, BarChart, Bar, XAxis, YAxis, Tooltip, CartesianGrid } from "recharts";
import { IncidentsByPeriodDto } from "../interfaces/Types";

interface ChartProps {
    chartTitle: string;
    data: IncidentsByPeriodDto[];
}

const ChartIncPer = ({ chartTitle, data }: ChartProps) => {
    const chartData = data.map(d => ({ name: d.label, incidents: d.count }));

    return (
        <>
            <h2 className="chartTitle">Incidents — Last {chartTitle}</h2>
            <ResponsiveContainer width="100%" aspect={3}>
                <BarChart data={chartData} margin={{ top: 4, right: 20, left: 0, bottom: 0 }}>
                    <CartesianGrid strokeDasharray="3 3" stroke="#e0e0e0" />
                    <XAxis dataKey="name" tick={{ fontFamily: 'Work Sans', fontSize: 12 }} />
                    <YAxis tick={{ fontFamily: 'Work Sans', fontSize: 12 }} />
                    <Tooltip
                        contentStyle={{ fontFamily: 'Work Sans', fontSize: 13 }}
                        cursor={{ fill: '#e3f2fd' }}
                    />
                    <Bar dataKey="incidents" fill="#0d478e" radius={[3, 3, 0, 0]} />
                </BarChart>
            </ResponsiveContainer>
        </>
    );
};

export default ChartIncPer;