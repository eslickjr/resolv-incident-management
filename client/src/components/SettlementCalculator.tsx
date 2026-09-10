import { useState } from 'react';
import { Loan } from '../interfaces/Types';
import '../styles/SettlementCalculator.css';

interface SettlementCalculatorProps {
    loan: Loan | null;
    onSelectAmount: (amount: number) => void;
}

const SettlementCalculator: React.FC<SettlementCalculatorProps> = ({ loan, onSelectAmount }) => {
    // Payoff tier percentages
    const [payoffPct1, setPayoffPct1] = useState<string>('60');
    const [payoffPct2, setPayoffPct2] = useState<string>('70');
    const [payoffPct3, setPayoffPct3] = useState<string>('80');

    // Balance tier percentages
    const [balPct1, setBalPct1] = useState<string>('60');
    const [balPct2, setBalPct2] = useState<string>('75');
    const [balPct3, setBalPct3] = useState<string>('80');

    // Shared contractual payment amount for balance tiers 2 & 3
    const [contractual, setContractual] = useState<string>('000');

    const [selectedTier, setSelectedTier] = useState<string | null>(null);

    if (!loan) return null;

    const payoff  = loan.payoff  ?? 0;
    const balance = loan.balance ?? 0;

    const displayContractual = () => {
        const digits = contractual.padStart(3, '0');
        const cents = digits.slice(-2);
        const dollars = digits.slice(0, -2).replace(/^0+/, '') || '0';
        return `${parseInt(dollars).toLocaleString('en-US')}.${cents}`;
    };

    const contractualAmt = parseFloat(displayContractual()) || 0;

    const formatCurrency = (val: number) =>
        `$${val.toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 })}`;

    // ── Payoff Tiers ─────────────────────────────────────────────────────────
    const p1Total = payoff * (parseFloat(payoffPct1) / 100);
    const p2Total = payoff * (parseFloat(payoffPct2) / 100);
    const p3Total = payoff * (parseFloat(payoffPct3) / 100);

    const p1Payment = p1Total;
    const p2Payment = p2Total / 3;
    const p3Payment = p3Total / 6;

    // ── Balance Tiers ─────────────────────────────────────────────────────────
    const b1Total   = balance * (parseFloat(balPct1) / 100);
    const b2Total   = balance * (parseFloat(balPct2) / 100);
    const b3Total   = balance * (parseFloat(balPct3) / 100);

    const b2Remaining = Math.max(0, b2Total - contractualAmt);
    const b3Remaining = Math.max(0, b3Total - contractualAmt);

    const b2Payment = contractualAmt > 0 ? b2Remaining / 2 : 0;
    const b3Payment = contractualAmt > 0 ? b3Remaining / 5 : 0;

    const handleSelect = (tierId: string, amount: number) => {
        setSelectedTier(tierId);
        onSelectAmount(amount);
    };

    const handleContractualKey = (e: React.KeyboardEvent<HTMLInputElement>) => {
        e.preventDefault();
        const input = e.currentTarget;
        const digits = contractual.replace(/\D/g, '');
        const display = displayContractual();
        const selStart = input.selectionStart ?? display.length;
        const selEnd = input.selectionEnd ?? display.length;

        if (e.key === 'a' && e.ctrlKey) { input.select(); return; }

        const leftDisplay = display.slice(0, selStart);
        const rightDisplay = display.slice(selEnd);
        const leftDigitCount = (leftDisplay.match(/\d/g) ?? []).length;
        const rightDigitCount = (rightDisplay.match(/\d/g) ?? []).length;
        const selectedDigitCount = digits.length - leftDigitCount - rightDigitCount;
        const leftDigits = digits.slice(0, leftDigitCount);
        const rightDigits = digits.slice(digits.length - rightDigitCount);

        if (e.key === 'Backspace' || e.key === 'Delete') {
            if (selectedDigitCount > 0) {
                setContractual((leftDigits + rightDigits).padStart(3, '0') || '000');
            } else {
                setContractual(digits.slice(0, -1).padStart(3, '0') || '000');
            }
            return;
        }

        if (!/^\d$/.test(e.key)) return;

        if (selectedDigitCount > 0) {
            const newDigits = (leftDigits + e.key + rightDigits).replace(/^0+/, '') || '0';
            if (newDigits.length > 8) return;
            setContractual(newDigits.padStart(3, '0'));
        } else {
            if (digits.length >= 8) return;
            const newDigits = (digits + e.key).replace(/^0+/, '') || '0';
            setContractual(newDigits.padStart(3, '0'));
        }
    };

    const pctInput = (val: string, setter: (v: string) => void) => (
        <input
            type="number"
            className="settlementPctInput"
            value={val}
            min={1}
            max={100}
            onChange={e => setter(e.target.value)}
            onBlur={e => {
                const num = Math.min(100, Math.max(1, parseFloat(e.target.value) || 1));
                setter(num.toString());
            }}
            onFocus={e => e.target.select()}
        />
    );

    return (
        <div id="settlementCalc">
            {/* ── Payoff-Based ── */}
            <div className="settlementSection">
                <div className="settlementSectionHeader">
                    <span className="settlementSectionTitle">Payoff-Based Settlement</span>
                    <div className="settlementMeta">
                        <span className="settlementMetaLabel">Payoff</span>
                        <span className="settlementMetaValue">{formatCurrency(payoff)}</span>
                    </div>
                </div>
                <div className="settlementTiersGrid">
                    {/* Tier 1 */}
                    <div className={`settlementTierCard${selectedTier === 'p1' ? ' settlementTierCardSelected' : ''}`}>
                        <div className="settlementTierTop">
                            {pctInput(payoffPct1, setPayoffPct1)}
                            <span className="settlementTierPctLabel">% — 1 Payment</span>
                        </div>
                        <div className="settlementTierTotal">{formatCurrency(p1Total)}</div>
                        <div className="settlementTierPayment">
                            <span className="settlementTierPaymentLabel">Payment</span>
                            <span className="settlementTierPaymentValue">{formatCurrency(p1Payment)}</span>
                        </div>
                        <button
                            className={`settlementSelectBtn${selectedTier === 'p1' ? ' settlementSelectBtnSelected' : ''}`}
                            onClick={() => handleSelect('p1', p1Payment)}
                            disabled={p1Total <= 0}
                        >
                            Select
                        </button>
                    </div>

                    {/* Tier 2 */}
                    <div className={`settlementTierCard${selectedTier === 'p2' ? ' settlementTierCardSelected' : ''}`}>
                        <div className="settlementTierTop">
                            {pctInput(payoffPct2, setPayoffPct2)}
                            <span className="settlementTierPctLabel">% — 3 Payments</span>
                        </div>
                        <div className="settlementTierTotal">{formatCurrency(p2Total)}</div>
                        <div className="settlementTierPayment">
                            <span className="settlementTierPaymentLabel">Per Payment</span>
                            <span className="settlementTierPaymentValue">{formatCurrency(p2Payment)}</span>
                        </div>
                        <button
                            className={`settlementSelectBtn${selectedTier === 'p2' ? ' settlementSelectBtnSelected' : ''}`}
                            onClick={() => handleSelect('p2', p2Payment)}
                            disabled={p2Total <= 0}
                        >
                            Select
                        </button>
                    </div>

                    {/* Tier 3 */}
                    <div className={`settlementTierCard${selectedTier === 'p3' ? ' settlementTierCardSelected' : ''}`}>
                        <div className="settlementTierTop">
                            {pctInput(payoffPct3, setPayoffPct3)}
                            <span className="settlementTierPctLabel">% — 6 Payments</span>
                        </div>
                        <div className="settlementTierTotal">{formatCurrency(p3Total)}</div>
                        <div className="settlementTierPayment">
                            <span className="settlementTierPaymentLabel">Per Payment</span>
                            <span className="settlementTierPaymentValue">{formatCurrency(p3Payment)}</span>
                        </div>
                        <button
                            className={`settlementSelectBtn${selectedTier === 'p3' ? ' settlementSelectBtnSelected' : ''}`}
                            onClick={() => handleSelect('p3', p3Payment)}
                            disabled={p3Total <= 0}
                        >
                            Select
                        </button>
                    </div>
                </div>
            </div>

            {/* ── Balance-Based ── */}
            <div className="settlementSection">
                <div className="settlementSectionHeader">
                    <span className="settlementSectionTitle">Balance-Based Settlement</span>
                    <div className="settlementMeta">
                        <span className="settlementMetaLabel">Balance</span>
                        <span className="settlementMetaValue">{formatCurrency(balance)}</span>
                    </div>
                </div>

                <div className="settlementContractualRow">
                    <label className="settlementContractualLabel">Contractual Payment</label>
                    <div className="settlementContractualInput">
                        <span className="settlementDollarSign">$</span>
                        <input
                            type="text"
                            value={displayContractual()}
                            onKeyDown={handleContractualKey}
                            onChange={() => {}}
                            onFocus={e => { if (displayContractual() !== '0.00') e.currentTarget.select(); }}
                            style={{ textAlign: 'right', fontVariantNumeric: 'tabular-nums' }}
                        />
                    </div>
                </div>

                <div className="settlementTiersGrid">
                    {/* Balance Tier 1 */}
                    <div className={`settlementTierCard${selectedTier === 'b1' ? ' settlementTierCardSelected' : ''}`}>
                        <div className="settlementTierTop">
                            {pctInput(balPct1, setBalPct1)}
                            <span className="settlementTierPctLabel">% — 1 Payment</span>
                        </div>
                        <div className="settlementTierTotal">{formatCurrency(b1Total)}</div>
                        <div className="settlementTierPayment">
                            <span className="settlementTierPaymentLabel">Payment</span>
                            <span className="settlementTierPaymentValue">{formatCurrency(b1Total)}</span>
                        </div>
                        <button
                            className={`settlementSelectBtn${selectedTier === 'b1' ? ' settlementSelectBtnSelected' : ''}`}
                            onClick={() => handleSelect('b1', b1Total)}
                            disabled={b1Total <= 0}
                        >
                            Select
                        </button>
                    </div>

                    {/* Balance Tier 2 */}
                    <div className={`settlementTierCard${selectedTier === 'b2' ? ' settlementTierCardSelected' : ''}`}>
                        <div className="settlementTierTop">
                            {pctInput(balPct2, setBalPct2)}
                            <span className="settlementTierPctLabel">% — 3 Payments</span>
                        </div>
                        <div className="settlementTierTotal">{formatCurrency(b2Total)}</div>
                        <div className="settlementTierBreakdown">
                            <div className="settlementTierPayment">
                                <span className="settlementTierPaymentLabel">1st Payment</span>
                                <span className="settlementTierPaymentValue">{contractualAmt > 0 ? formatCurrency(contractualAmt) : '—'}</span>
                            </div>
                            <div className="settlementTierPayment">
                                <span className="settlementTierPaymentLabel">2 × Payments</span>
                                <span className="settlementTierPaymentValue">{contractualAmt > 0 ? formatCurrency(b2Payment) : '—'}</span>
                            </div>
                        </div>
                        <button
                            className={`settlementSelectBtn${selectedTier === 'b2' ? ' settlementSelectBtnSelected' : ''}`}
                            onClick={() => handleSelect('b2', contractualAmt)}
                            disabled={b2Total <= 0 || contractualAmt <= 0}
                        >
                            Select
                        </button>
                    </div>

                    {/* Balance Tier 3 */}
                    <div className={`settlementTierCard${selectedTier === 'b3' ? ' settlementTierCardSelected' : ''}`}>
                        <div className="settlementTierTop">
                            {pctInput(balPct3, setBalPct3)}
                            <span className="settlementTierPctLabel">% — 6 Payments</span>
                        </div>
                        <div className="settlementTierTotal">{formatCurrency(b3Total)}</div>
                        <div className="settlementTierBreakdown">
                            <div className="settlementTierPayment">
                                <span className="settlementTierPaymentLabel">1st Payment</span>
                                <span className="settlementTierPaymentValue">{contractualAmt > 0 ? formatCurrency(contractualAmt) : '—'}</span>
                            </div>
                            <div className="settlementTierPayment">
                                <span className="settlementTierPaymentLabel">5 × Payments</span>
                                <span className="settlementTierPaymentValue">{contractualAmt > 0 ? formatCurrency(b3Payment) : '—'}</span>
                            </div>
                        </div>
                        <button
                            className={`settlementSelectBtn${selectedTier === 'b3' ? ' settlementSelectBtnSelected' : ''}`}
                            onClick={() => handleSelect('b3', contractualAmt)}
                            disabled={b3Total <= 0 || contractualAmt <= 0}
                        >
                            Select
                        </button>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default SettlementCalculator;