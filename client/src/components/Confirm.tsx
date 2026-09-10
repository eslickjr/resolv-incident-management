import { useNavigate } from "react-router-dom";
import '../styles/Confirm.css';

interface ConfirmProps {
    isOpen: boolean;
    onClose: () => void;
    onConfirm: () => Promise<void>;
    mode: 'update' | 'close';
    skipNavigate?: boolean;
}

const Confirm: React.FC<ConfirmProps> = ({ isOpen, onClose, onConfirm, mode, skipNavigate }) => {
    if (!isOpen) return null;

    const navigate = useNavigate();

    const handleConfirm = async (e: React.MouseEvent<HTMLInputElement>) => {
        e.preventDefault();
        await onConfirm();
        if (!skipNavigate) {
            navigate('/');
        }
    };

    return (
        <>
            <div id="confirmOverlay" onClick={onClose}>
                <div id="confirmModal" onClick={(e) => e.stopPropagation()}>
                    <div id="confirmHeader">
                        <h2 id="confirmTitle">
                            {mode === 'close' ? 'Confirm Ticket Close' : 'Confirm Update'}
                        </h2>
                        <span id="confirmClose" onClick={onClose}>✕</span>
                    </div>
                    <form id="confirmForm">
                        <p id="confirmMessage">
                            {mode === 'close'
                                ? 'Are you sure you want to close this ticket?'
                                : 'Save your changes and return to home?'}
                        </p>
                        <div id="confirmButtonContainer">
                            <input id="confirmYes" className="confirmButton" type="button" value="Yes" onClick={handleConfirm} />
                            <input id="confirmNo"  className="confirmButton" type="button" value="No"  onClick={onClose} />
                        </div>
                    </form>
                </div>
            </div>
        </>
    );
};

export default Confirm;