import { useState } from 'react';
import { useMsal } from '@azure/msal-react';
import { addNote } from '../api/incidents';
import SettlementCalculator from './SettlementCalculator';
import { Loan } from '../interfaces/Types';
import '../styles/PaymentProcessor.css';

interface PaymentProcessorProps {
    loan: Loan | null;
    incidentId?: number;
}

const PaymentProcessor: React.FC<PaymentProcessorProps> = ({ loan, incidentId }) => {
    const { instance } = useMsal();
    const [paymentMethod, setPaymentMethod] = useState<'ach' | 'card'>('card');
    const [paymentAmount, setPaymentAmount] = useState<string>(
        loan?.amountDue != null ? loan.amountDue.toFixed(2) : ''
    );
    const [useAccountInfo, setUseAccountInfo] = useState<boolean>(true);

    // ACH fields
    const [routingNumber, setRoutingNumber] = useState<string>('');
    const [accountNumber, setAccountNumber] = useState<string>('');
    const [accountType, setAccountType]     = useState<'checking' | 'savings'>('checking');

    // Card fields
    const [firstName, setFirstName]   = useState<string>(loan?.firstName ?? '');
    const [lastName, setLastName]     = useState<string>(loan?.lastName ?? '');
    const allZips = (loan?.address ?? '').match(/\b\d{5}\b/g);
    const zip = allZips ? allZips[allZips.length - 1] : '';
    const [zipCode, setZipCode]       = useState<string>(zip);
    const [cardNumber, setCardNumber] = useState<string>('');
    const [cvv, setCvv]               = useState<string>('');

    // Settlement
    const [selectedSettlement, setSelectedSettlement] = useState<number | null>(null);

    if (!loan) return null;
    if ((loan.balance ?? 0) === 0) return null;

    const handlePaymentSubmit = async () => {
        if (!incidentId) return;

        const method = paymentMethod === 'card' ? 'Card' : 'ACH';
        const last4 = paymentMethod === 'card'
            ? cardNumber.replace(/\s/g, '').slice(-4)
            : accountNumber.slice(-4);
        const note = `Payment processed: ${method} ${last4 ? `ending in ${last4}` : ''} — Amount: $${displayAmount()}`;

        await addNote(instance, incidentId, note);
    };

    const handlePayoffSelect = () => {
        if (selectedSettlement !== null) setSelectedSettlement(null);
        const cents = Math.round((loan?.payoff ?? 0) * 100);
        setPaymentAmount(cents.toString().padStart(3, '0'));
    };

    const handleUseAccountInfo = (checked: boolean) => {
        setUseAccountInfo(checked);
        if (checked) {
            setFirstName(loan?.firstName ?? '');
            setLastName(loan?.lastName ?? '');
            // Extract zip from address — last 5 digits of address string
            const allZips = (loan?.address ?? '').match(/\b\d{5}\b/g);
            setZipCode(allZips ? allZips[allZips.length - 1] : '');
        } else {
            setFirstName('');
            setLastName('');
            setZipCode('');
        }
    };

    const handleAmountKey = (e: React.KeyboardEvent<HTMLInputElement>) => {
        e.preventDefault();
        const input = e.currentTarget;
        const digits = paymentAmount.replace(/\D/g, '');
        const display = displayAmount();
        const selStart = input.selectionStart ?? display.length;
        const selEnd = input.selectionEnd ?? display.length;

        if (e.key === 'a' && e.ctrlKey) {
            input.select();
            return;
        }

        // Count digits to the left of selection
        const leftDisplay = display.slice(0, selStart);
        const leftDigitCount = (leftDisplay.match(/\d/g) ?? []).length;

        // Count digits to the right of selection
        const rightDisplay = display.slice(selEnd);
        const rightDigitCount = (rightDisplay.match(/\d/g) ?? []).length;

        const selectedDigitCount = digits.length - leftDigitCount - rightDigitCount;

        const leftDigits  = digits.slice(0, leftDigitCount);
        const rightDigits = digits.slice(digits.length - rightDigitCount);

        if (e.key === 'Backspace' || e.key === 'Delete') {
            if (selectedDigitCount > 0) {
                const newDigits = (leftDigits + rightDigits).padStart(3, '0') || '000';
                setPaymentAmount(newDigits);
            } else {
                const newDigits = digits.slice(0, -1) || '0';
                setPaymentAmount(newDigits.padStart(3, '0'));
            }
            return;
        }

        if (!/^\d$/.test(e.key)) return;

        // Deselect settlement tier if user manually types an amount
        if (selectedSettlement !== null) setSelectedSettlement(null);

        if (selectedDigitCount > 0) {
            const newDigits = (leftDigits + e.key + rightDigits).replace(/^0+/, '') || '0';
            if (newDigits.length > 8) return;
            setPaymentAmount(newDigits.padStart(3, '0'));
        } else {
            if (digits.length >= 8) return;
            const newDigits = (digits + e.key).replace(/^0+/, '') || '0';
            setPaymentAmount(newDigits.padStart(3, '0'));
        }
    };

    const displayAmount = () => {
        const digits = paymentAmount.replace(/\D/g, '').padStart(3, '0');
        const cents = digits.slice(-2);
        const dollars = digits.slice(0, -2).replace(/^0+/, '') || '0';
        const formatted = parseInt(dollars).toLocaleString('en-US');
        return `${formatted}.${cents}`;
    };

    const isChargeOff = !!loan.chargeOffDate;

    const paymentValue = parseFloat(displayAmount());
    const exceedsPayoff = (loan?.payoff ?? 0) > 0 && paymentValue > (loan?.payoff ?? 0);

    const formatCurrency = (val: number) =>
        `$${val.toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 })}`;

    const formatCard = (val: string) => {
        const digits = val.replace(/\D/g, '').slice(0, 16);
        return digits.replace(/(.{4})/g, '$1 ').trim();
    };

    const paymentForm = (
        <div className="paymentSection">
            <div className="paymentSectionHeader">
                <span className="paymentSectionTitle">Process Payment</span>
            </div>
            <div id="paymentBody">
                
                <div id="paymentMethodToggle">
                    <button
                        className={`paymentMethodBtn${paymentMethod === 'card' ? ' paymentMethodSelected' : ''}`}
                        onClick={() => setPaymentMethod('card')}
                    >
                        Card
                    </button>
                    <button
                        className={`paymentMethodBtn${paymentMethod === 'ach' ? ' paymentMethodSelected' : ''}`}
                        onClick={() => setPaymentMethod('ach')}
                    >
                        ACH / Bank Account
                    </button>
                </div>

                <div id="paymentFields">
                    {paymentMethod === 'ach' ? (
                        <>
                            <div className="paymentField">
                                <label className="paymentLabel">Routing Number</label>
                                <input type="text" className="paymentInput" placeholder="9 digits"
                                    value={routingNumber} maxLength={9}
                                    onChange={e => setRoutingNumber(e.target.value.replace(/\D/g, ''))} />
                            </div>
                            <div className="paymentField">
                                <label className="paymentLabel">Account Number</label>
                                <input type="text" className="paymentInput" placeholder="Account number"
                                    value={accountNumber}
                                    onChange={e => setAccountNumber(e.target.value.replace(/\D/g, ''))} />
                            </div>
                            <div className="paymentField">
                                <label className="paymentLabel">Account Type</label>
                                <select className="paymentInput" value={accountType}
                                    onChange={e => setAccountType(e.target.value as 'checking' | 'savings')}>
                                    <option value="checking">Checking</option>
                                    <option value="savings">Savings</option>
                                </select>
                            </div>
                        </>
                    ) : (
                        <>
                            <div id="useAccountInfoRow">
                                <input
                                    type="checkbox"
                                    id="useAccountInfo"
                                    checked={useAccountInfo}
                                    onChange={e => handleUseAccountInfo(e.target.checked)}
                                />
                                <label htmlFor="useAccountInfo" id="useAccountInfoLabel">
                                    Use account information
                                </label>
                            </div>
                            <div className="paymentField">
                                <label className="paymentLabel">First Name</label>
                                <input type="text" className="paymentInput" placeholder="First name" readOnly={useAccountInfo}
                                    value={firstName} onChange={e => setFirstName(e.target.value.replace(/[^a-zA-Z]/g, ''))} />
                            </div>
                            <div className="paymentField">
                                <label className="paymentLabel">Last Name</label>
                                <input type="text" className="paymentInput" placeholder="Last name" readOnly={useAccountInfo}
                                    value={lastName} onChange={e => setLastName(e.target.value.replace(/[^a-zA-Z]/g, ''))} />
                            </div>
                            <div className="paymentField">
                                <label className="paymentLabel">Zip Code</label>
                                <input type="text" className="paymentInput" placeholder="5 digits" readOnly={useAccountInfo}
                                    value={zipCode} maxLength={5}
                                    onChange={e => setZipCode(e.target.value.replace(/\D/g, ''))} />
                            </div>
                            <div className="paymentField">
                                <label className="paymentLabel">Card Number</label>
                                <input type="text" className="paymentInput" placeholder="0000 0000 0000 0000"
                                    value={cardNumber} onChange={e => setCardNumber(formatCard(e.target.value))} />
                            </div>
                            <div className="paymentField">
                                <label className="paymentLabel">Security Code</label>
                                <input type="text" className="paymentInput" placeholder="3-4 digits"
                                    value={cvv} maxLength={4}
                                    onChange={e => setCvv(e.target.value.replace(/\D/g, ''))} />
                            </div>
                        </>
                    )}
                    <div className="paymentField">
                        <label className="paymentLabel">Payment Amount</label>
                        <div id="paymentAmountContainer">
                            <div id="paymentAmountRow">
                                <span id="paymentDollar">$</span>
                                <input
                                    type="text"
                                    className="paymentInput"
                                    value={displayAmount()}
                                    onKeyDown={handleAmountKey}
                                    onChange={() => {}}
                                    onFocus={e => { if (displayAmount() !== '0.00') e.currentTarget.select(); }}
                                    style={{ textAlign: 'right', fontVariantNumeric: 'tabular-nums' }}
                                    readOnly={false}
                                />
                            </div>
                            <button id="payoffBtn" onClick={handlePayoffSelect}>
                                Use Payoff ({formatCurrency(loan?.payoff ?? 0)})
                            </button>
                        </div>
                    </div>
                </div>
                <span className="paymentError" style={{ visibility: exceedsPayoff ? 'visible' : 'hidden' }}>
                    Payment amount exceeds payoff ({formatCurrency(loan?.payoff ?? 0)})
                </span>
                <button
                    id="paymentSubmit"
                    disabled={exceedsPayoff || paymentValue === 0}
                    onClick={handlePaymentSubmit}
                >
                    Process Payment (API not connected)
                </button>
            </div>
        </div>
    );

    return (
        <div id="paymentProcessorContainer" className={isChargeOff ? 'paymentProcessorSplit' : ''}>
            {paymentForm}
            {isChargeOff && (
                <>
                    <div className="paymentSection">
                        <div className="paymentSectionHeader">
                            <span className="paymentSectionTitle">Settlement Calculator</span>
                        </div>
                        <SettlementCalculator
                            loan={loan}
                            onSelectAmount={(amount) => {
                                const cents = Math.round(amount * 100);
                                setPaymentAmount(cents.toString().padStart(3, '0'));
                                setSelectedSettlement(amount);
                            }}
                        />
                    </div>
                </>
            )}
        </div>
    );
};

export default PaymentProcessor;