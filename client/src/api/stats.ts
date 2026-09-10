import { IPublicClientApplication } from '@azure/msal-browser';
import { getAccessToken } from '../auth/getAccessToken';
import { UserStatsDto, StatsResponseDto } from '../interfaces/Types';

export type StatsPeriod = 'day' | 'week' | 'month';

export const getAggregateStats = async (
    instance: IPublicClientApplication,
    period: StatsPeriod
): Promise<StatsResponseDto> => {
    const token = await getAccessToken(instance);
    const response = await fetch(`/api/stats/aggregate/${period}`, {
        headers: { 'Authorization': `Bearer ${token}` }
    });
    if (!response.ok) throw new Error('Not authorized');
    return response.json();
};

export const getMyStats = async (
    instance: IPublicClientApplication,
    period: StatsPeriod,
    username?: string
): Promise<StatsResponseDto> => {
    const token = await getAccessToken(instance);
    const url = username 
        ? `/api/stats/${period}?filterUsername=${encodeURIComponent(username)}`
        : `/api/stats/${period}`;
    const response = await fetch(url, {
        headers: { 'Authorization': `Bearer ${token}` }
    });
    return response.json();
};

export const getAllStats = async (
    instance: IPublicClientApplication,
    period: StatsPeriod
): Promise<UserStatsDto[]> => {
    const token = await getAccessToken(instance);
    const response = await fetch(`/api/stats/all/${period}`, {
        headers: { 'Authorization': `Bearer ${token}` }
    });
    if (!response.ok) throw new Error('Not authorized');
    return response.json();
};