import { useParams } from "react-router-dom";
import { useEffect, useState } from "react";
import { useMsal } from "@azure/msal-react";

import Incident from "../components/Incident";
import IncidentHistory from "../components/IncidentHistory";

import { Incident as IncidentType } from "../interfaces/Types";
import { getIncidentById } from "../api/incidents";

import "../styles/NoAccount.css";

const NoAccount = () => {
    const { incidentId } = useParams<{ incidentId: string }>();
    const { instance } = useMsal();
    const [incident, setIncident] = useState<IncidentType | null>(null);
    const [loading, setLoading] = useState<boolean>(true);

    useEffect(() => {
        const fetchIncident = async () => {
            if (!incidentId) return;
            try {
                const found = await getIncidentById(instance, parseInt(incidentId));
                if (found) setIncident(found);
            } catch (err) {
                console.error('Failed to load incident:', err);
            } finally {
                setLoading(false);
            }
        };

        fetchIncident();
    }, [incidentId]);

    if (loading) return <div>Loading...</div>;

    return (
        <div id="noAccount">
            <h1 id="incTitle">Incident</h1>
            <Incident
                incidentId={incidentId ? parseInt(incidentId) : undefined}
                initialBranch={incident?.branch ?? ''}
                initialPhone={incident?.phone ?? ''}
                initialSSN={incident?.ssn ?? ''}
                initialFirstName={incident?.firstName ?? ''}
                initialLastName={incident?.lastName ?? ''}
                initialIssue={incident?.issue ?? ''}
                initialSolution={incident?.solution ?? ''}
                initialAdditionalDetails={incident?.additionalDetails ?? ''}
                isReadOnly={!!incident?.closedAt}
            />
            <h1 id="historyTitle">Incident History</h1>
            <IncidentHistory ssn={incident?.ssn} currentIncidentId={incident?.incidentId} />
        </div>
    );
}

export default NoAccount;