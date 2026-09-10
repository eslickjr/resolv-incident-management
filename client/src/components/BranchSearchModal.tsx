import { useState, useEffect, useRef } from 'react';
import { useMsal } from '@azure/msal-react';
import { searchBranches, getBranchByCode, BranchResult } from '../api/branches.ts';
import '../styles/BranchSearchModal.css';

interface BranchSearchModalProps {
    isOpen: boolean;
    onClose: () => void;
    currentBranch?: string;
    cachedBranch?: BranchResult | null;
    onSelect: (locationCode: string, branchInfo: BranchResult) => void;
}

const BranchSearchModal: React.FC<BranchSearchModalProps> = ({
    isOpen, onClose, currentBranch, cachedBranch, onSelect
}) => {
    const { instance } = useMsal();
    const [query, setQuery] = useState<string>('');
    const searchTimeout = useRef<ReturnType<typeof setTimeout> | null>(null);
    const [results, setResults] = useState<BranchResult[]>([]);
    const [currentBranchInfo, setCurrentBranchInfo] = useState<BranchResult | null>(null);
    const [loading, setLoading] = useState<boolean>(false);
    const [searching, setSearching] = useState<boolean>(false);

    useEffect(() => {
        if (!isOpen) return;
        if (!currentBranch) { setCurrentBranchInfo(null); return; }
        
        // Use cache if branch matches
        if (cachedBranch && cachedBranch.locationCode === currentBranch) {
            setCurrentBranchInfo(cachedBranch);
            return;
        }
        
        // Otherwise fetch from DB
        getBranchByCode(instance, currentBranch).then(b => {
            setCurrentBranchInfo(b);
        });
    }, [isOpen, currentBranch]);

    const handleSearch = async (val: string) => {
        setQuery(val);
        if (searchTimeout.current) clearTimeout(searchTimeout.current);
        if (!val.trim()) { setResults([]); return; }

        setSearching(true);

        searchTimeout.current = setTimeout(async () => {
            setLoading(true);
            setSearching(false);
            try {
                const data = await searchBranches(instance, val);
                setResults(data);
            } catch (err) {
                console.error('Branch search failed:', err);
            } finally {
                setLoading(false);
            }
        }, 400); // Debounce delay
    };

    const handleSelect = (branch: BranchResult) => {
        onSelect(branch.locationCode ?? '', branch);
        handleClose();
    };

    const handleClose = () => {
        setQuery('');
        setResults([]);
        setCurrentBranchInfo(null);
        setSearching(false);
        setLoading(false);
        onClose();
    };

    if (!isOpen) return null;

    return (
        <div id="branchSearchOverlay" onClick={handleClose}>
            <div id="branchSearchModal" onClick={e => e.stopPropagation()}>
                <div id="branchSearchHeader">
                    <span id="branchSearchTitle">Search for Branch</span>
                    <span id="branchSearchClose" onClick={handleClose}>✕</span>
                </div>
                <div id="branchSearchBody">
                    {currentBranchInfo && (
                        <div id="branchCurrentInfo">
                            <div id="branchCurrentLabel">Current Branch</div>
                            <div className="branchResultItem branchResultCurrent">
                                <div className="branchResultCode">{currentBranchInfo.locationCode}</div>
                                <div className="branchResultName">{currentBranchInfo.locationName}</div>
                                <div className="branchResultAddress">
                                    {currentBranchInfo.physicalAddress1}, {currentBranchInfo.physicalCity}, {currentBranchInfo.physicalState} {currentBranchInfo.physicalZip}
                                </div>
                                {currentBranchInfo.phoneMain && (
                                    <div className="branchResultPhone">{currentBranchInfo.phoneMain}</div>
                                )}
                            </div>
                        </div>
                    )}
                    <div id="branchSearchInputRow">
                        <input
                            id="branchSearchInput"
                            type="text"
                            placeholder="Search by name, city, zip, phone..."
                            value={query}
                            onChange={e => handleSearch(e.target.value)}
                            autoFocus
                        />
                    </div>
                    <div id="branchSearchResults">
                        {loading || searching ? (
                            <p className="branchSearchEmpty">Searching...</p>
                        ) : results.length === 0 && query ? (
                            <p className="branchSearchEmpty">No branches found.</p>
                        ) : (
                            results.map(b => (
                                <div
                                    key={b.locationId}
                                    className="branchResultItem"
                                    onClick={() => handleSelect(b)}
                                >
                                    <div className="branchResultCode">{b.locationCode}</div>
                                    <div className="branchResultName">{b.locationName}</div>
                                    <div className="branchResultAddress">
                                        {b.physicalAddress1}, {b.physicalCity}, {b.physicalState} {b.physicalZip}
                                    </div>
                                    {b.phoneMain && (
                                        <div className="branchResultPhone">{b.phoneMain}</div>
                                    )}
                                </div>
                            ))
                        )}
                    </div>
                </div>
            </div>
        </div>
    );
};

export default BranchSearchModal;