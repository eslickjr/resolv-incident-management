import { IPublicClientApplication } from '@azure/msal-browser';
import { loginRequest } from './msalConfig';

// Set to true to bypass Azure AD auth during development
const BYPASS_AUTH = true;
const DEV_TOKEN = 'dev-bypass-token';

/**
 * Silently acquires a token for API calls.
 * Falls back to a popup if silent acquisition fails.
 * In dev bypass mode, returns a placeholder token.
 */
export const getAccessToken = async (
    instance: IPublicClientApplication
): Promise<string | null> => {
    if (BYPASS_AUTH) return DEV_TOKEN;

    const accounts = instance.getAllAccounts();
    if (accounts.length === 0) return null;

    try {
        const result = await instance.acquireTokenSilent({
            ...loginRequest,
            account: accounts[0]
        });
        return result.accessToken;
    } catch {
        try {
            const result = await instance.acquireTokenPopup(loginRequest);
            return result.accessToken;
        } catch (err) {
            console.error('Token acquisition failed:', err);
            return null;
        }
    }
};