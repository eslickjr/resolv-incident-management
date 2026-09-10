import { useParams } from "react-router-dom";
import { useEffect, useState, useRef } from "react";
import { useMsal } from "@azure/msal-react";

import Incident from "../components/Incident";
import IncidentHistory from "../components/IncidentHistory";
import Loan from "../components/Loan";
import LoanHistory from "../components/LoanHistory";

import { Incident as IncidentType, Loan as LoanType } from "../interfaces/Types";
import { getIncidentById } from "../api/incidents";
import { getLoanInformation } from "../api/loans";

import '../styles/CustomerAccount.css';

const CustomerAccount = () => {
    const { incidentId, branch, account } = useParams<{ incidentId: string; branch: string; account: string }>();
    const { instance } = useMsal();

    const [incident, setIncident] = useState<IncidentType | null>(null);
    const [loan, setLoan] = useState<LoanType | null>(null);
    const [loading, setLoading] = useState<boolean>(true);

    const [activeBranch, setActiveBranch] = useState<string>(
        branch?.replace(/^0+/, '') ?? ''
    );
    const [activeAccount, setActiveAccount] = useState<string>(account ?? '');   

    const [incidentTab, setIncidentTab] = useState<boolean>(true);
    const incidentTabRef = useRef<boolean>(true);
    const [incTabClass, setIncTabClass] = useState<string>("selected");
    const [incHistoryTabClass, setIncHistoryTabClass] = useState<string>("deselected");
    const [loanTab, setLoanTab] = useState<boolean>(true);
    const loanTabRef = useRef<boolean>(true);
    const [loanTabClass, setLoanTabClass] = useState<string>("tab selected");
    const [loanHistoryTabClass, setLoanHistoryTabClass] = useState<string>("deselected");

    useEffect(() => {
        const fetchData = async () => {
            try {
                if (incidentId && incidentId !== 'new') {
                    const found = await getIncidentById(instance, parseInt(incidentId));
                    if (found) setIncident(found);
                }

                if (branch && account) {
                    const loanData = await getLoanInformation(instance, branch, account);
                    if (loanData) setLoan(loanData);
                }
            } catch (err) {
                console.error('Failed to load customer account data:', err);
            } finally {
                setLoading(false);
            }
        };

        fetchData();
    }, [incidentId, branch, account]);

    const handleLoanSelect = async (selectedBranch: string, selectedAccount: string) => {
        try {
            const loanData = await getLoanInformation(instance, selectedBranch, selectedAccount);
            if (loanData) {
                setLoan(loanData);
                setActiveBranch(selectedBranch);
                setActiveAccount(selectedAccount);
                setLoanTab(true);
                loanTabRef.current = true;
                setLoanTabClass("selected");
                setLoanHistoryTabClass("deselected");
            }
        } catch (err) {
            console.error('Failed to load selected loan:', err);
        }
    };

    const handleIncidentTab = (e: React.MouseEvent<HTMLDivElement>): void => {
        if ((e.target as HTMLDivElement).id === "incTab") {
            incidentTabRef.current = true;
            setIncidentTab(true);
            setIncTabClass("selected");
            setIncHistoryTabClass("deselected");
        } else {
            incidentTabRef.current = false;
            setIncidentTab(false);
            setIncTabClass("deselected");
            setIncHistoryTabClass("selected");
        }
    };

    const handleLoanTab = (e: React.MouseEvent<HTMLDivElement>): void => {
        if ((e.target as HTMLDivElement).id === "loanTab") {
            loanTabRef.current = true;
            setLoanTab(true);
            setLoanTabClass("selected");
            setLoanHistoryTabClass("deselected");
        } else {
            loanTabRef.current = false;
            setLoanTab(false);
            setLoanTabClass("deselected");
            setLoanHistoryTabClass("selected");
        }
    };

    const handleIncidentSelect = async (incidentId: number) => {
        try {
            const found = await getIncidentById(instance, incidentId);
            if (found) {
                setIncident(found);
                setIncidentTab(true);
                incidentTabRef.current = true;
                setIncTabClass("selected");
                setIncHistoryTabClass("deselected");
            }
        } catch (err) {
            console.error('Failed to load selected incident:', err);
        }
    };

    if (loading) return <div>Loading...</div>;

    return (
        <div id="customerAccount">
            <div id="incTitleContainer">
                <h1 id={incidentTab ? "incTitle" : "historyTitle"}>
                    {incidentTab ? "Incident" : "Incident History"}
                </h1>
            </div>
            <div id="incTabsContainer">
                <div id="incTab" className={`tab ${incTabClass}`} onClick={handleIncidentTab}>Incident</div>
                <div id="incHistoryTab" className={`tab ${incHistoryTabClass}`} onClick={handleIncidentTab}>Incident History</div>
            </div>
            {incidentTab ?
                <Incident
                    key={incident?.incidentId}
                    incidentId={incident?.incidentId}
                    initialBranch={incident?.branch ?? loan?.branch ?? ''}
                    initialAccount={incident?.account ?? loan?.account ?? ''}
                    initialPhone={incident?.phone ?? loan?.cell ?? ''}
                    initialSSN={incident?.ssn ?? loan?.ssn ?? ''}
                    initialFirstName={incident?.firstName ?? loan?.firstName ?? ''}
                    initialLastName={incident?.lastName ?? loan?.lastName ?? ''}
                    initialIssue={incident?.issue ?? ''}
                    initialSolution={incident?.solution ?? ''}
                    initialAdditionalDetails={incident?.additionalDetails ?? ''}
                    isReadOnly={!!incident?.closedAt}
                />
            :
                <IncidentHistory 
                    ssn={incident?.ssn ?? loan?.ssn} 
                    currentIncidentId={incident?.incidentId} 
                    onIncidentSelect={handleIncidentSelect}
                    scrollable={true}
                />
            }
            <div id="loanTitleContainer">
                <h1 id={loanTab ? "loanTitle" : "loanHistoryTitle"}>
                    {loanTab ? "Loan" : "Loan History"}
                </h1>
            </div>
            <div id="loanTabsContainer">
                <div id="loanTab" className={`tab ${loanTabClass}`} onClick={handleLoanTab}>Loan</div>
                <div id="loanHistoryTab" className={`tab ${loanHistoryTabClass}`} onClick={handleLoanTab}>Loan History</div>
            </div>
            {loanTab ?
                <Loan loan={loan} incidentId={incident?.incidentId} />
            :
                <LoanHistory
                    ssn={incident?.ssn ?? loan?.ssn}
                    currentBranch={activeBranch}
                    currentAccount={activeAccount}
                    onLoanSelect={handleLoanSelect}
                />
            }
        </div>
    );
}

export default CustomerAccount;