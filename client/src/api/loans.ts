import { IPublicClientApplication } from '@azure/msal-browser';
import { getAccessToken } from '../auth/getAccessToken';
import { Loan, LoanPayment, LoanHistory } from '../interfaces/Types';

export const getLoanInformation = async (
    instance: IPublicClientApplication,
    branch: string,
    account: string
): Promise<Loan | null> => {
    const token = await getAccessToken(instance);
    const response = await fetch(
        `/api/loans/${encodeURIComponent(branch)}/${encodeURIComponent(account)}`,
        {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            }
        }
    );

    if (!response.ok) return null;
    return response.json();
};

export const getPaymentHistory = async (
    instance: IPublicClientApplication,
    branch: string,
    account: string
): Promise<LoanPayment[]> => {
    const token = await getAccessToken(instance);
    const response = await fetch(
        `/api/loans/${encodeURIComponent(branch)}/${encodeURIComponent(account)}/payments`,
        {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            }
        }
    );

    if (!response.ok) return [];
    return response.json();
};

export const getLoanHistory = async (
    instance: IPublicClientApplication,
    ssn: string
): Promise<LoanHistory[]> => {
    const token = await getAccessToken(instance);
    const response = await fetch(`/api/loans/history/${encodeURIComponent(ssn)}`, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        }
    });

    if (!response.ok) return [];
    return response.json();
};