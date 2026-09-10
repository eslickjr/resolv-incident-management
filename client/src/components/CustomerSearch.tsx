import NewCustomer from "./NewCustomer";
import { useEffect, useState, useRef } from "react";
import { useNavigate } from "react-router-dom";
import { useMsal } from "@azure/msal-react";
import { searchByName, searchBySSN } from "../api/search";
import { SearchResult } from "../interfaces/Types";
import { createIncident, getOpenIncidentBySSN } from "../api/incidents";

import '../styles/CustomerSearch.css';

const CustomerSearch = () => {
    const [searchResults, setSearchResults] = useState<SearchResult[]>([]);
    const [isModalOpen, setIsModalOpen] = useState<boolean>(false);
    const debounceRef = useRef<ReturnType<typeof setTimeout> | null>(null);
    const latestSearchValue = useRef<string>('');

    const navigate = useNavigate();
    const { instance } = useMsal();

    // Determine if input is SSN (numeric) or name (alpha)
    const isSSN = (value: string): boolean => /^\d/.test(value);

    const handleSearchChange = async (e: React.ChangeEvent<HTMLInputElement>): Promise<void> => {
        if (e.target.value.length > 0) {
            if (e.target.value.charAt(0) === ' ') {
                e.target.value = e.target.value.slice(1);
            } else if (isNaN(Number(e.target.value.charAt(0)))) {
                // Name input — allow letters and spaces only
                e.target.value = e.target.value.replace(/[^a-zA-Z\s]/g, '');
            } else {
                // SSN input — numbers only with formatting
                e.target.value = e.target.value.replace(/[^0-9]/g, '');
                let formatted = e.target.value.slice(0, 3);
                if (e.target.value.length > 3) formatted += '-' + e.target.value.slice(3, 5);
                if (e.target.value.length > 5) formatted += '-' + e.target.value.slice(5, 9);
                e.target.value = formatted;
            }
        }

        const currentValue = e.target.value;
        latestSearchValue.current = currentValue;

        setSearchResults([]);

        if (currentValue.length > 2) {
            if (debounceRef.current) clearTimeout(debounceRef.current);

            const shouldSearch = isSSN(currentValue)
                ? currentValue.length === 11
                : true;

            if (shouldSearch) {
                debounceRef.current = setTimeout(async () => {
                    try {
                        let results;
                        if (isSSN(currentValue)) {
                            results = await searchBySSN(instance, currentValue);
                        } else {
                            const parts = currentValue.trim().split(/\s+/);
                            const firstName = parts[0] ?? '';
                            const lastName = parts.length > 1 ? parts[1] : '';
                            results = await searchByName(instance, firstName, lastName);
                        }

                        if (latestSearchValue.current !== currentValue) return;
                        setSearchResults(results);
                    } catch (err) {
                        console.error('Search failed:', err);
                        setSearchResults([]);
                    }
                }, 1000);
            }
        } else {
            if (debounceRef.current) clearTimeout(debounceRef.current);
            setSearchResults([]);
        }
    };

    // Priority 1-3 = exact, 4-6 = loose
    // 1/4 = Incident+Loan (CL), 2/5 = Incident only (CX), 3/6 = Loan only (LN)
    const getTypeLabel = (priority: number): string => {
        const base = ((priority - 1) % 3) + 1;
        if (base === 1) return 'CL';
        if (base === 2) return 'CX';
        return 'LN';
    };

    const getTypeCssClass = (priority: number): string => {
        const base = ((priority - 1) % 3) + 1;
        if (base === 1) return 'clSearch typeSearch';
        if (base === 2) return 'cxSearch typeSearch';
        return 'lnSearch typeSearch';
    };

    const handleSearchClick = async (result: SearchResult): Promise<void> => {
        const typeLabel = getTypeLabel(result.priority);

        if (typeLabel === 'CL' || typeLabel === 'CX') {
            // Check if the existing incident is open
            if (result.incidentId && !result.closedAt) {
                // Incident is open — navigate to it
                if (typeLabel === 'CL' && result.branch && result.account) {
                    navigate(`/customerAccount/${result.incidentId}/${result.branch}/${result.account}`);
                } else {
                    navigate(`/noAccount/${result.incidentId}`);
                }
            } else {
                // Incident is closed or none — check for any open incident by SSN
                if (result.ssn) {
                    const openIncident = await getOpenIncidentBySSN(instance, result.ssn);
                    if (openIncident) {
                        if (result.account && result.branch) {
                            navigate(`/customerAccount/${openIncident.incidentId}/${result.branch}/${result.account}`);
                        } else {
                            navigate(`/noAccount/${openIncident.incidentId}`);
                        }
                        return;
                    }
                }
                // No open incident — create new
                const newIncident = await createIncident(instance, {
                    firstName: result.firstName,
                    lastName: result.lastName,
                    ssn: result.ssn,
                    phone: result.phone,
                    branch: result.branch,
                    account: result.account
                });
                if (newIncident) {
                    if (result.account && result.branch) {
                        navigate(`/customerAccount/${newIncident.incidentId}/${result.branch}/${result.account}`);
                    } else {
                        navigate(`/noAccount/${newIncident.incidentId}`);
                    }
                }
            }
        } else if (typeLabel === 'LN' && result.loanId) {
            // Loan only — check for open incident by SSN first
            if (result.ssn) {
                const openIncident = await getOpenIncidentBySSN(instance, result.ssn);
                if (openIncident) {
                    navigate(`/customerAccount/${openIncident.incidentId}/${result.branch}/${result.account}`);
                    return;
                }
            }
            // No open incident — create new
            const newIncident = await createIncident(instance, {
                firstName: result.firstName,
                lastName: result.lastName,
                ssn: result.ssn,
                phone: result.phone,
                branch: result.branch,
                account: result.account
            });
            if (newIncident) {
                navigate(`/customerAccount/${newIncident.incidentId}/${result.branch}/${result.account}`);
            }
        }
    };

    return (
        <div>
            <div id="outerContainer">
                <div id="searchContainer">
                    <div id="searchBarContainer">
                        <div id="searchBar">
                            <input
                                type="text"
                                id="searchInput"
                                onChange={handleSearchChange}
                                placeholder="Search for customer..."
                            />
                            <div id="searchResults" className={searchResults.length > 0 ? 'searchResultsVisible' : ''} tabIndex={0}>
                                <ul id="searchResultsList">
                                    {searchResults.map((result) => (
                                        <li
                                            key={`${result.incidentId ?? 'l'}-${result.account ?? result.fullName}`}
                                            className="searchResult"
                                            onClick={() => handleSearchClick(result)}
                                        >
                                            <div className="branchContainer searchResultContainer">
                                                <p className="branchSearch">{result.branch}</p>
                                            </div>
                                            <div className="nameContainer searchResultContainer">
                                                <p className="nameSearch">{result.fullName}</p>
                                            </div>
                                            <div className="typeContainer searchResultContainer">
                                                <p className={getTypeCssClass(result.priority)}>
                                                    {getTypeLabel(result.priority)}
                                                </p>
                                            </div>
                                        </li>
                                    ))}
                                </ul>
                            </div>
                        </div>
                    </div>
                    <input
                        type="button"
                        id="newCustomer"
                        value="New Customer"
                        onClick={() => setIsModalOpen(true)}
                    />
                    <NewCustomer isOpen={isModalOpen} onClose={() => setIsModalOpen(false)} />
                </div>
            </div>
        </div>
    );
}

export default CustomerSearch;