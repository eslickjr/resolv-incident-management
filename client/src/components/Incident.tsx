import { useState, useEffect, useRef } from 'react';
import { useMsal } from '@azure/msal-react';
import { useBlocker, useNavigate } from 'react-router-dom';
import Confirm from './Confirm';
import Notes, { NotesRef } from './Notes';
import BranchSearchModal from './BranchSearchModal';
import { createIncident, updateIncident, getOpenIncidentBySSN, addNote } from '../api/incidents';
import { getIssues, getSolutionsForIssue, getCallTipsForIssue } from '../api/issueCache';
import { BranchResult } from '../api/branches.ts';

import '../styles/Incident.css';

interface BarBooleanI {
    mouseIn: boolean;
    focused: boolean;
    textIn: boolean;
}

interface IncidentProps {
    incidentId?: number;
    initialBranch?: string;
    initialAccount?: string;
    initialPhone?: string;
    initialSSN?: string;
    initialFirstName?: string;
    initialLastName?: string;
    initialIssue?: string;
    initialSolution?: string;
    initialAdditionalDetails?: string;
    isReadOnly?: boolean;
}

const formatPhone = (val: string): string => {
    const digits = val.replace(/\D/g, '');
    if (!digits) return '';
    let formatted = '(' + digits.substring(0, 3);
    if (digits.length >= 4) formatted += ') ' + digits.substring(3, 6);
    if (digits.length >= 7) formatted += '-' + digits.substring(6, 10);
    return formatted;
};

const formatSSN = (val: string): string => {
    const digits = val.replace(/\D/g, '');
    if (!digits) return '';
    let formatted = digits.substring(0, 3);
    if (digits.length >= 4) formatted += '-' + digits.substring(3, 5);
    if (digits.length >= 6) formatted += '-' + digits.substring(5, 9);
    return formatted;
};

const maskSSN = (val: string): string => {
    const digits = val.replace(/\D/g, '');
    if (digits.length !== 9) return val;
    return `***-**-${digits.slice(5)}`;
};

const Incident: React.FC<IncidentProps> = ({
    incidentId,
    initialBranch = '',
    initialAccount = '',
    initialPhone = '',
    initialSSN = '',
    initialFirstName = '',
    initialLastName = '',
    initialIssue = '',
    initialSolution = '',
    initialAdditionalDetails = '',
    isReadOnly = false
}) => {
    const { instance } = useMsal();
    const navigate = useNavigate();

    const issueRef = useRef<string>('');
    const solutionRef = useRef<string>('');
    const barBoolean = useRef<BarBooleanI[]>([]);
    const notesRef = useRef<NotesRef>(null);
    const [isModalOpen, setIsModalOpen] = useState<boolean>(false);
    const [modalMode, setModalMode] = useState<'update' | 'close'>('close');
    const errors = useRef<number>(0);
    const [branchModalOpen, setBranchModalOpen] = useState<boolean>(false);
    const [_cachedBranch, setCachedBranch] = useState<BranchResult | null>(null);

    const [branch, setBranch] = useState<string>(initialBranch);
    const [phone, setPhone] = useState<string>(formatPhone(initialPhone));
    const [ssn, setSSN] = useState<string>(formatSSN(initialSSN));
    const [showSSN, setShowSSN] = useState<boolean>(false);
    const [firstName, setFirstName] = useState<string>(initialFirstName.trim());
    const [lastName, setLastName] = useState<string>(initialLastName.trim());
    const [issue, setIssue] = useState<string>(initialIssue);
    const [solution, setSolution] = useState<string>(initialSolution);
    const [additionalDetails, setAdditionalDetails] = useState<string>(initialAdditionalDetails);
    const [noteText, setNoteText] = useState<string>('');
    const [showTips, setShowTips] = useState<boolean>(false);
    const [tipsExpanded, setTipsExpanded] = useState<boolean>(true);

    const [branchSpan, setBranchSpan] = useState<string>('');
    const [phoneSpan, setPhoneSpan] = useState<string>('');
    const [ssnSpan, setSSNSpan] = useState<string>('');
    const [firstNameSpan, setFirstNameSpan] = useState<string>('');
    const [lastNameSpan, setLastNameSpan] = useState<string>('');
    const [issueSpan, setIssueSpan] = useState<string>('');
    const [solutionSpan, setSolutionSpan] = useState<string>('');
    const [closeSpan, setCloseSpan] = useState<string>('');

    // ── Timer ─────────────────────────────────────────────────────────────────
    const timerStart = useRef<number>(Date.now());
    const getElapsedSeconds = () => Math.floor((Date.now() - timerStart.current) / 1000);

    // ── Navigation guard ──────────────────────────────────────────────────────
    useEffect(() => {
        const handleBeforeUnload = (e: BeforeUnloadEvent) => {
            e.preventDefault();
            e.returnValue = '';
            if (incidentId) {
                const payload = JSON.stringify({ timeSpentSeconds: getElapsedSeconds(), isClosing: false });
                const blob = new Blob([payload], { type: 'application/json' });
                navigator.sendBeacon(`/api/incidents/${incidentId}/beacon`, blob);
            }
        };
        window.addEventListener('beforeunload', handleBeforeUnload);
        return () => window.removeEventListener('beforeunload', handleBeforeUnload);
    }, [incidentId]);

    // ── Pre-populate from props ───────────────────────────────────────────────
    useEffect(() => {
        setBranch(initialBranch);
        setPhone(formatPhone(initialPhone));
        setSSN(formatSSN(initialSSN));
        setFirstName(initialFirstName.trim());
        setLastName(initialLastName.trim());
        setIssue(initialIssue);
        setSolution(initialSolution);
        setAdditionalDetails(initialAdditionalDetails);
        issueRef.current = initialIssue;
        solutionRef.current = initialSolution;
        setShowTips(initialIssue !== '');
    }, [initialBranch, initialPhone, initialSSN, initialFirstName, initialLastName, initialIssue, initialSolution, initialAdditionalDetails]);

    // ── Beacon for time tracking ─────────────────────────────────────────────
    useEffect(() => {
        const handleSaveTime = async () => {
            if (!incidentId) return;
            try {
                const blob = new Blob([JSON.stringify({ timeSpentSeconds: getElapsedSeconds(), isClosing: false })], { type: 'application/json' });
                navigator.sendBeacon(`/api/incidents/${incidentId}/beacon`, blob);
            } catch (err) {
                console.error('Failed to save time:', err);
            }
        };
        window.addEventListener('saveIncidentTime', handleSaveTime);
        return () => window.removeEventListener('saveIncidentTime', handleSaveTime);
    }, [incidentId]);

    const blocker = useBlocker(({ historyAction }) =>
        historyAction === 'POP' && !isReadOnly
    );

    const saveTime = async () => {
        if (!incidentId) return;
        try {
            await updateIncident(instance, incidentId, {
                timeSpentSeconds: getElapsedSeconds(),
                isClosing: false
            });
        } catch (err) {
            console.error('Failed to save time:', err);
        }
    };

    const handleNewIncident = async () => {
        setIssue('');
        setSolution('');
        setAdditionalDetails('');
        issueRef.current = '';
        solutionRef.current = '';

        if (ssn) {
            const rawSSN = ssn.replace(/\D/g, '');
            const openIncident = await getOpenIncidentBySSN(instance, rawSSN);
            if (openIncident) {
                if (initialAccount && branch) {
                    navigate(`/customerAccount/${openIncident.incidentId}/${branch}/${initialAccount}`);
                } else {
                    navigate(`/noAccount/${openIncident.incidentId}`);
                }
                return;
            }
        }

        const newIncident = await createIncident(instance, {
            firstName, lastName, ssn, phone, branch, account: initialAccount
        });

        if (newIncident) {
            if (initialAccount && branch) {
                navigate(`/customerAccount/${newIncident.incidentId}/${branch}/${initialAccount}`);
            } else {
                navigate(`/noAccount/${newIncident.incidentId}`);
            }
        }
    };

    const handleBranch = (val: string) => {
        val = val.replace(/[^0-9]/g, '');
        if (branchSpan === 'Branch is required' && val !== '') { setBranchSpan(''); errors.current -= 1; if (errors.current === 0) setCloseSpan(''); }
        if (branchSpan === 'Branch must be at least 2 digits' && val.length >= 2) { setBranchSpan(''); errors.current -= 1; if (errors.current === 0) setCloseSpan(''); }
        return val.slice(0, 4);
    };

    const handlePhone = (val: string) => {
        val = val.replace(/[^0-9]/g, '');
        if (val.length > 0) val = '(' + val;
        if (val.length > 4) val = val.slice(0, 4) + ') ' + val.slice(4);
        if (val.length > 9) val = val.slice(0, 9) + '-' + val.slice(9, 13);
        if (phoneSpan === 'Phone is required' && val !== '') { setPhoneSpan(''); errors.current -= 1; if (errors.current === 0) setCloseSpan(''); }
        if (phoneSpan === 'Phone must be 10 digits' && val.length === 14) { setPhoneSpan(''); errors.current -= 1; if (errors.current === 0) setCloseSpan(''); }
        return val;
    };

    const handleSSN = (val: string) => {
        val = val.replace(/[^0-9]/g, '');
        let formatted = val.slice(0, 3);
        if (val.length > 3) formatted += '-' + val.slice(3, 5);
        if (val.length > 5) formatted += '-' + val.slice(5, 9);
        if (ssnSpan === 'SSN is required' && formatted !== '') { setSSNSpan(''); errors.current -= 1; if (errors.current === 0) setCloseSpan(''); }
        if (ssnSpan === 'SSN must be 9 digits' && formatted.length === 11) { setSSNSpan(''); errors.current -= 1; if (errors.current === 0) setCloseSpan(''); }
        return formatted;
    };

    const handleIncChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
        const bar = e.target.parentElement as HTMLElement;
        if (e.target.id === 'incBranch') setBranch(handleBranch(e.target.value));
        if (e.target.id === 'incPhone') setPhone(handlePhone(e.target.value));
        if (e.target.id === 'incSSN') setSSN(handleSSN(e.target.value));
        if (e.target.id === 'incFN' || e.target.id === 'incLN') {
            e.target.value = e.target.value.replace(/[^a-zA-Z]/g, '');
            if (e.target.id === 'incFN') {
                setFirstName(e.target.value);
                if (firstNameSpan !== '' && e.target.value !== '') { setFirstNameSpan(''); errors.current -= 1; if (errors.current === 0) setCloseSpan(''); }
            } else {
                setLastName(e.target.value);
                if (lastNameSpan !== '' && e.target.value !== '') { setLastNameSpan(''); errors.current -= 1; if (errors.current === 0) setCloseSpan(''); }
            }
        }
        if (e.target.id === 'incAD') setAdditionalDetails(e.target.value);
        const key = bar.getAttribute('key');
        if (key !== null) barBoolean.current[parseInt(key)].textIn = e.target.value !== '';
    };

    const handleIssueChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
        if (issueSpan !== '' && e.target.value !== '') { setIssueSpan(''); errors.current -= 1; if (errors.current === 0) setCloseSpan(''); }
        issueRef.current = e.target.value;
        setIssue(e.target.value);
        setShowTips(e.target.value !== '');
    };

    const handleSolutionChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
        if (solutionSpan !== '' && e.target.value !== '') { setSolutionSpan(''); errors.current -= 1; if (errors.current === 0) setCloseSpan(''); }
        solutionRef.current = e.target.value;
        setSolution(e.target.value);
        setTipsExpanded(true);
    };

    const validate = (requireSolution: boolean): boolean => {
        errors.current = 0;
        if (!branch) { setBranchSpan('Branch is required'); errors.current += 1; }
        else if (branch.length < 2) { setBranchSpan('Branch must be at least 2 digits'); errors.current += 1; }
        else setBranchSpan('');
        if (!phone) { setPhoneSpan('Phone is required'); errors.current += 1; }
        else if (phone.length < 14) { setPhoneSpan('Phone must be 10 digits'); errors.current += 1; }
        else setPhoneSpan('');
        if (!ssn) { setSSNSpan('SSN is required'); errors.current += 1; }
        else if (ssn.length < 11) { setSSNSpan('SSN must be 9 digits'); errors.current += 1; }
        else setSSNSpan('');
        if (!firstName) { setFirstNameSpan('First Name is required'); errors.current += 1; } else setFirstNameSpan('');
        if (!lastName) { setLastNameSpan('Last Name is required'); errors.current += 1; } else setLastNameSpan('');
        if (!issue) { setIssueSpan('Issue is required'); errors.current += 1; } else setIssueSpan('');
        if (requireSolution && !solution) { setSolutionSpan('Solution is required'); errors.current += 1; } else setSolutionSpan('');

        if (errors.current > 0) { setCloseSpan('There is a problem with your form.'); return false; }
        setCloseSpan('');
        return true;
    };

    const openUpdateModal = () => { if (!validate(false)) return; setModalMode('update'); setIsModalOpen(true); };
    const openCloseModal  = () => { if (!validate(true))  return; setModalMode('close');  setIsModalOpen(true); };

    const handleConfirm = async () => {
        if (!incidentId) return;
        try {
            await updateIncident(instance, incidentId, {
                branch, phone, ssn, firstName, lastName, issue,
                solution: modalMode === 'close' ? solution : undefined,
                additionalDetails,
                timeSpentSeconds: getElapsedSeconds(),
                isClosing: modalMode === 'close'
            });

            // Save note if there's text
            if (noteText.trim()) {
                await addNote(instance, incidentId, noteText.trim());
                setNoteText('');
                notesRef.current?.refreshNotes();
            }
        } catch (err) {
            console.error('Failed to save incident:', err);
        }
    };

    return (
        <div id="incComponent">
            <div id="outerIncContainer">
                <div id="incContainer">
                    <form id="incForm" className={isReadOnly ? 'incFormReadOnly' : ''}>
                        <div id="incBranchCell" className="incFormCell incFormCol1 incFormRow1">
                            <span id="incBranchBarSpan" className="errorSpan">{branchSpan}</span>
                            <div id="incBranchBarContainer">
                                <div id="incBranchBar" className="incBar">
                                    <input type="text" inputMode="numeric" placeholder=" " value={branch} id="incBranch" className="incInput" name="incBranch" onChange={handleIncChange} disabled={isReadOnly} />
                                </div>
                                <input type="button" id="incBranchSearch" className="incButton" value="Search for Branch" onClick={() => setBranchModalOpen(true)} />
                            </div>
                        </div>
                        <div className="incFormCell incFormCol2 incFormRow1 incFormEmpty"></div>
                        <div id="incPhoneCell" className="incFormCell incFormCol1 incFormRow2">
                            <span id="incPhoneBarSpan" className="errorSpan">{phoneSpan}</span>
                            <div id="incPhoneBar" className="incBar">
                                <input type="tel" id="incPhone" placeholder=" " value={phone} className="incInput" name="incPhone" onChange={handleIncChange} disabled={isReadOnly} />
                            </div>
                        </div>
                        <div id="incSSNCell" className="incFormCell incFormCol2 incFormRow2">
                            <span id="incSSNBarSpan" className="errorSpan">{ssnSpan}</span>
                            <div id="incSSNBar" className="incBar">
                                <input type="text" inputMode="numeric" placeholder=" " value={showSSN ? ssn : maskSSN(ssn)} id="incSSN" className="incInput" name="incSSN" onChange={handleIncChange} disabled={isReadOnly} />
                                <span id="incSSNToggle" onClick={() => setShowSSN(!showSSN)}>
                                    {showSSN ? (
                                        <svg xmlns="http://www.w3.org/2000/svg" width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
                                            <path d="M17.94 17.94A10.07 10.07 0 0 1 12 20c-7 0-11-8-11-8a18.45 18.45 0 0 1 5.06-5.94"/>
                                            <path d="M9.9 4.24A9.12 9.12 0 0 1 12 4c7 0 11 8 11 8a18.5 18.5 0 0 1-2.16 3.19"/>
                                            <line x1="1" y1="1" x2="23" y2="23"/>
                                        </svg>
                                    ) : (
                                        <svg xmlns="http://www.w3.org/2000/svg" width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
                                            <path d="M1 12s4-8 11-8 11 8 11 8-4 8-11 8-11-8-11-8z"/>
                                            <circle cx="12" cy="12" r="3"/>
                                        </svg>
                                    )}
                                </span>
                            </div>
                        </div>
                        <div id="incFNCell" className="incFormCell incFormCol1 incFormRow3">
                            <span id="incFNBarSpan" className="errorSpan">{firstNameSpan}</span>
                            <div id="incFNBar" className="incBar">
                                <input type="text" id="incFN" placeholder=" " value={firstName} className="incInput" name="incFN" onChange={handleIncChange} disabled={isReadOnly} />
                            </div>
                        </div>
                        <div id="incLNCell" className="incFormCell incFormCol2 incFormRow3">
                            <span id="incLNBarSpan" className="errorSpan">{lastNameSpan}</span>
                            <div id="incLNBar" className="incBar">
                                <input type="text" id="incLN" placeholder=" " value={lastName} className="incInput" name="incLN" onChange={handleIncChange} disabled={isReadOnly} />
                            </div>
                        </div>
                        <div id="incIssueCell" className="incFormCell incFormCol1 incFormRow4">
                            <span id="incIssueBarSpan" className="errorSpan">{issueSpan}</span>
                            <div id="incIssueBar" className="incBar">
                                <select id="incIssue" value={issue} className="incInput" onChange={handleIssueChange} disabled={isReadOnly}>
                                    <option value="" disabled>Select an Issue</option>
                                    {getIssues().map(i => (
                                        <option key={i.issueId} value={i.name}>{i.name}</option>
                                    ))}
                                </select>
                            </div>
                        </div>
                        <div id="incSolCell" className="incFormCell incFormCol2 incFormRow4">
                            <span id="incSolBarSpan" className="errorSpan">{solutionSpan}</span>
                            <div id="incSolBar" className="incBar">
                                {issue === "" ?
                                    <select id="incSol" value={solution} className="incInput" disabled>
                                        <option value="" disabled>Select a Solution</option>
                                    </select>
                                :
                                    <select id="incSol" value={solution} className="incInput" onChange={handleSolutionChange} disabled={isReadOnly}>
                                        <option value="" disabled>Select a Solution</option>
                                        {getSolutionsForIssue(getIssues().find(i => i.name === issue)?.issueId ?? 0).map(s => (
                                            <option key={s.solutionId} value={s.name}>{s.name}</option>
                                        ))}
                                    </select>
                                }
                            </div>
                        </div>
                        {showTips && !isReadOnly && (
                            <div id="incCallTipsContainer">
                                <div id="incCallTipsHeader" onClick={() => setTipsExpanded(!tipsExpanded)}>
                                    <span id="incCallTipsTitle">Call Tips</span>
                                    <span id="incCallTipsToggle">{tipsExpanded ? '▲' : '▼'}</span>
                                </div>
                                {tipsExpanded && (
                                    <ul id="incCallTipsList">
                                        {getCallTipsForIssue(getIssues().find(i => i.name === issue)?.issueId ?? 0).map(tip => (
                                            <li key={tip.tipId} className="incCallTip">{tip.tip}</li>
                                        ))}
                                    </ul>
                                )}
                            </div>
                        )}
                        <div id="incADContainer">
                            <div id="incADBar" className="incBar">
                                <textarea id="incAD" className="incInput" placeholder=" " value={additionalDetails} name="incAD" onChange={handleIncChange} disabled={isReadOnly}/>
                            </div>
                            <div id="incCloseContainer">
                                {isReadOnly ? (
                                    <input type="button" id="incNewIncident" className="incButton" value="New Incident" onClick={handleNewIncident} />
                                ) : (
                                    <>
                                        <input type="button" id="incUpdate" className="incButton" value="Update" onClick={openUpdateModal} />
                                        <input type="button" id="incClose" className="incButton" value="Close Ticket" onClick={openCloseModal} />
                                    </>
                                )}
                                <span id="incCloseSpan" className="errorSpan">{closeSpan}</span>
                            </div>
                        </div>
                    </form>
                </div>
            </div>
            <Notes
                ref={notesRef}
                incidentId={incidentId}
                isReadOnly={isReadOnly}
                noteText={noteText}
                onNoteChange={setNoteText}
            />
            <Confirm
                isOpen={isModalOpen}
                onClose={() => setIsModalOpen(false)}
                onConfirm={handleConfirm}
                mode={modalMode}
            />
            <Confirm
                isOpen={blocker.state === 'blocked'}
                onClose={() => blocker.reset?.()}
                onConfirm={async () => { await saveTime(); blocker.proceed?.(); }}
                mode="update"
                skipNavigate={true}
            />
            <BranchSearchModal
                isOpen={branchModalOpen}
                onClose={() => setBranchModalOpen(false)}
                currentBranch={branch || undefined}
                cachedBranch={_cachedBranch}
                onSelect={(code, branchInfo) => {
                    setBranch(code);
                    setCachedBranch(branchInfo);
                }}
            />
        </div>
    );
};

export default Incident;