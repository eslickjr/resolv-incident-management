import { useState } from 'react';
import '../styles/Legend.css';

export interface LegendItem {
    color: string;
    label: string;
}

interface LegendProps {
    items: LegendItem[];
}

const Legend: React.FC<LegendProps> = ({ items }) => {
    const [visible, setVisible] = useState(false);

    return (
        <div
            id="legendContainer"
            onMouseEnter={() => setVisible(true)}
            onMouseLeave={() => setVisible(false)}
        >
            <div id="legendIcon">
                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
                    <circle cx="12" cy="12" r="10" />
                    <line x1="12" y1="8" x2="12" y2="8" strokeWidth="3" />
                    <line x1="12" y1="12" x2="12" y2="16" />
                </svg>
            </div>
            {visible && (
                <div id="legendTooltip">
                    {items.map((item, index) => (
                        <div key={index} className="legendItem">
                            <div className="legendSwatch" style={{ backgroundColor: item.color }} />
                            <span className="legendLabel">{item.label}</span>
                        </div>
                    ))}
                </div>
            )}
        </div>
    );
};

export default Legend;