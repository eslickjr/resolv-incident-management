import { IPublicClientApplication } from '@azure/msal-browser';
import { getAccessToken } from '../auth/getAccessToken';

export interface BranchResult {
    locationId: number;
    locationCode: string;
    locationName: string;
    physicalAddress1?: string;
    physicalCity?: string;
    physicalState?: string;
    physicalZip?: string;
    phoneMain?: string;
    isActive: boolean;
}

export const searchBranches = async (
    instance: IPublicClientApplication,
    query: string
): Promise<BranchResult[]> => {
    const token = await getAccessToken(instance);
    const response = await fetch(`/api/branches/search?q=${encodeURIComponent(query)}`, {
        headers: { 'Authorization': `Bearer ${token}` }
    });
    if (!response.ok) return [];
    return response.json();
};

export const getBranchByCode = async (
    instance: IPublicClientApplication,
    locationCode: string
): Promise<BranchResult | null> => {
    const token = await getAccessToken(instance);
    const response = await fetch(`/api/branches/${encodeURIComponent(locationCode)}`, {
        headers: { 'Authorization': `Bearer ${token}` }
    });
    if (!response.ok) return null;
    return response.json();
};