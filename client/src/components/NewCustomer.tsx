import { useState, useRef } from 'react';
import { useNavigate } from "react-router-dom";
import { useMsal } from '@azure/msal-react';
import { createIncident, getOpenIncidentBySSN } from '../api/incidents';

import '../styles/NewCustomer.css';

interface NewCustomerProps {
    isOpen: boolean;
    onClose: () => void;
    prefilledPhone?: string;
    prefilledFirstName?: string;
    prefilledLastName?: string;
}

const formatPhone = (val: string): string => {
    const digits = val.replace(/\D/g, '');
    if (!digits) return '';
    let formatted = '(' + digits.substring(0, 3);
    if (digits.length >= 4) formatted += ') ' + digits.substring(3, 6);
    if (digits.length >= 7) formatted += '-' + digits.substring(6, 10);
    return formatted;
};

const NewCustomer: React.FC<NewCustomerProps> = ({ isOpen, onClose, prefilledPhone, prefilledFirstName, prefilledLastName }) => {
    const [firstName, setFirstName]         = useState<string>(prefilledFirstName || '');
    const [lastName, setLastName]           = useState<string>(prefilledLastName || '');
    const [phone, setPhone]                 = useState<string>(prefilledPhone ? formatPhone(prefilledPhone) : '');
    const [ssn, setSsn]                     = useState<string>('');
    const [firstNameSpan, setFirstNameSpan] = useState<string>('');
    const [lastNameSpan, setLastNameSpan]   = useState<string>('');
    const [phoneSpan, setPhoneSpan]         = useState<string>('');
    const [ssnSpan, setSsnSpan]             = useState<string>('');
    const [isSubmitting, setIsSubmitting]   = useState<boolean>(false);

    const error = useRef<boolean>(false);
    const navigate = useNavigate();
    const { instance } = useMsal();

    if (!isOpen) return null;

    const handleAlphaInput = (e: React.ChangeEvent<HTMLInputElement>): void => {
        e.target.value = e.target.value.replace(/[^a-zA-Z]/g, '');

        if (e.target.id === 'newCustomerFirstName') {
            const val = e.target.value;
            setFirstName(val.charAt(0).toUpperCase() + val.slice(1));
            if (val && firstNameSpan) setFirstNameSpan('');
        }

        if (e.target.id === 'newCustomerLastName') {
            const val = e.target.value;
            setLastName(val.charAt(0).toUpperCase() + val.slice(1));
            if (val && lastNameSpan) setLastNameSpan('');
        }
    };

    const handlePhone = (e: React.ChangeEvent<HTMLInputElement>): void => {
        const input = e.target.value.replace(/\D/g, '');
        let formatted = '';
        if (input.length > 0) formatted += '(' + input.substring(0, 3);
        if (input.length >= 4) formatted += ') ' + input.substring(3, 6);
        if (input.length >= 7) formatted += '-' + input.substring(6, 10);
        setPhone(formatted);

        if (phoneSpan === 'Phone number is required' && formatted) setPhoneSpan('');
        else if (phoneSpan === 'Phone number must be 10 digits' && formatted.length === 14) setPhoneSpan('');
    };

    const handleSsn = (e: React.ChangeEvent<HTMLInputElement>): void => {
        const input = e.target.value.replace(/\D/g, '');
        let formatted = '';
        if (input.length > 0) formatted += input.substring(0, 3);
        if (input.length >= 4) formatted += '-' + input.substring(3, 5);
        if (input.length >= 6) formatted += '-' + input.substring(5, 9);
        setSsn(formatted);

        if (ssnSpan === 'SSN is required' && formatted) setSsnSpan('');
        else if (ssnSpan === 'SSN must be 9 digits' && formatted.length === 11) setSsnSpan('');
    };

    const handleNewCustomer = async (e: React.MouseEvent<HTMLInputElement>) => {
        e.preventDefault();
        error.current = false;

        if (!firstName) { setFirstNameSpan('First name is required'); error.current = true; }
        if (!lastName)  { setLastNameSpan('Last name is required');   error.current = true; }

        if (!phone) {
            setPhoneSpan('Phone number is required'); error.current = true;
        } else if (phone.length < 14) {
            setPhoneSpan('Phone number must be 10 digits'); error.current = true;
        }

        if (!ssn) {
            setSsnSpan('SSN is required'); error.current = true;
        } else if (ssn.length < 11) {
            setSsnSpan('SSN must be 9 digits'); error.current = true;
        }

        if (error.current) return;

        setIsSubmitting(true);
        try {
            if (ssn) {
                const rawSSN = ssn.replace(/\D/g, '');
                const openIncident = await getOpenIncidentBySSN(instance, rawSSN);
                if (openIncident) {
                    onClose();
                    if (openIncident.branch && openIncident.account) {
                        navigate(`/customerAccount/${openIncident.incidentId}/${openIncident.branch}/${openIncident.account}`);
                    } else {
                        navigate(`/noAccount/${openIncident.incidentId}`);
                    }
                    return;
                }
            }

            const incident = await createIncident(instance, {
                firstName,
                lastName,
                phone,
                ssn
            });

            if (!incident) {
                console.error('Failed to create incident');
                return;
            }

            onClose();
            if (incident.branch && incident.account) {
                navigate(`/customerAccount/${incident.incidentId}/${incident.branch}/${incident.account}`);
            } else {
                navigate(`/noAccount/${incident.incidentId}`);
            }
        } catch (err) {
            console.error('Error creating incident:', err);
        } finally {
            setIsSubmitting(false);
        }
    };

    return (
        <>
            <div id="newCustomerOverlay" onClick={onClose}>
                <div id="newCustomerModal" onClick={(e) => e.stopPropagation()}>
                    <div id="newCustomerModalHeader">
                        <h2 id="newCustomerModalTitle">New Customer</h2>
                        <span id="newCustomerModalClose" onClick={onClose}>✕</span>
                    </div>
                    <form id="newCustomerModalForm">
                        <div id="newCustomerModalFormContainer">
                            <label className="newCustomerModalLabel" htmlFor="newCustomerFirstName">First Name:</label>
                            <input type="text" id="newCustomerFirstName" value={firstName} className="newCustomerModalInput" onChange={handleAlphaInput} />
                            <span className="newCustomerModalSpan">{firstNameSpan}</span>

                            <label className="newCustomerModalLabel" htmlFor="newCustomerLastName">Last Name:</label>
                            <input type="text" id="newCustomerLastName" value={lastName} className="newCustomerModalInput" onChange={handleAlphaInput} />
                            <span className="newCustomerModalSpan">{lastNameSpan}</span>

                            <label className="newCustomerModalLabel" htmlFor="newCustomerSsn">SSN:</label>
                            <input type="text" inputMode="numeric" id="newCustomerSsn" value={ssn} className="newCustomerModalInput" onChange={handleSsn} />
                            <span className="newCustomerModalSpan">{ssnSpan}</span>

                            <label className="newCustomerModalLabel" htmlFor="newCustomerPhone">Phone:</label>
                            <input type="tel" id="newCustomerPhone" value={phone} className="newCustomerModalInput" onChange={handlePhone} />
                            <span className="newCustomerModalSpan">{phoneSpan}</span>
                        </div>
                        <input
                            id="newCustomerModalSubmit"
                            type="submit"
                            value={isSubmitting ? 'Submitting...' : 'Submit'}
                            onClick={handleNewCustomer}
                            disabled={isSubmitting}
                        />
                    </form>
                </div>
            </div>
        </>
    );
};

export default NewCustomer;