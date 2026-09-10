import { IPublicClientApplication } from '@azure/msal-browser';
import { getAccessToken } from '../auth/getAccessToken';
import {
    Incident,
    IncidentHistory,
    CreateIncidentRequest,
    UpdateIncidentRequest,
    IncidentNote
} from '../interfaces/Types';

export const createIncident = async (
    instance: IPublicClientApplication,
    request: CreateIncidentRequest
): Promise<Incident | null> => {
    const token = await getAccessToken(instance);
    const response = await fetch('/api/incidents', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        },
        body: JSON.stringify(request)
    });

    if (!response.ok) return null;
    return response.json();
};

export const updateIncident = async (
    instance: IPublicClientApplication,
    incidentId: number,
    request: UpdateIncidentRequest
): Promise<Incident | null> => {
    const token = await getAccessToken(instance);
    const response = await fetch(`/api/incidents/${incidentId}`, {
        method: 'PATCH',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        },
        body: JSON.stringify(request)
    });

    if (!response.ok) return null;
    return response.json();
};

export const getIncidentById = async (
    instance: IPublicClientApplication,
    incidentId: number
): Promise<Incident | null> => {
    const token = await getAccessToken(instance);
    const response = await fetch(`/api/incidents/${incidentId}`, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        }
    });

    if (!response.ok) return null;
    return response.json();
};

export const getIncidentsByCustomer = async (
    instance: IPublicClientApplication,
    ssn: string
): Promise<IncidentHistory[]> => {
    const token = await getAccessToken(instance);
    const response = await fetch(`/api/incidents/customer/${encodeURIComponent(ssn)}`, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        }
    });

    if (!response.ok) return [];
    return response.json();
};

export const getOpenIncidentBySSN = async (
    instance: IPublicClientApplication,
    ssn: string
): Promise<Incident | null> => {
    const token = await getAccessToken(instance);
    const response = await fetch(`/api/incidents/open/${encodeURIComponent(ssn)}`, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        }
    });

    if (response.status === 404) return null;
    if (!response.ok) return null;
    return response.json();
};

export const getRecentIncidents = async (
    instance: IPublicClientApplication
): Promise<Incident[]> => {
    const token = await getAccessToken(instance);
    const response = await fetch('/api/incidents/recent', {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        }
    });

    if (!response.ok) return [];
    return response.json();
};

// Incident Notes API

export const getNotes = async (
    instance: IPublicClientApplication,
    incidentId: number
): Promise<IncidentNote[]> => {
    const token = await getAccessToken(instance);
    const response = await fetch(`/api/incidents/${incidentId}/notes`, {
        headers: { 'Authorization': `Bearer ${token}` }
    });
    if (!response.ok) return [];
    return response.json();
};

export const addNote = async (
    instance: IPublicClientApplication,
    incidentId: number,
    note: string
): Promise<IncidentNote | null> => {
    const token = await getAccessToken(instance);
    const response = await fetch(`/api/incidents/${incidentId}/notes`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        },
        body: JSON.stringify({ note })
    });
    if (!response.ok) return null;
    return response.json();
};