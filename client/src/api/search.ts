import { IPublicClientApplication } from '@azure/msal-browser';
import { getAccessToken } from '../auth/getAccessToken';
import { SearchResult } from '../interfaces/Types';

export const searchByName = async (
    instance: IPublicClientApplication,
    firstName: string,
    lastName: string
): Promise<SearchResult[]> => {
    const token = await getAccessToken(instance);
    const response = await fetch(`/api/search/name/${encodeURIComponent(firstName)}/${encodeURIComponent(lastName)}`, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        }
    });

    if (!response.ok) return [];
    return response.json();
};

export const searchBySSN = async (
    instance: IPublicClientApplication,
    ssn: string
): Promise<SearchResult[]> => {
    const token = await getAccessToken(instance);
    const response = await fetch(`/api/search/ssn/${encodeURIComponent(ssn)}`, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        }
    });

    if (!response.ok) return [];
    return response.json();
};