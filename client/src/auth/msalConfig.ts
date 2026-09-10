import { Configuration, PopupRequest } from '@azure/msal-browser';

// Replace these with your actual Azure AD values from appsettings.json
export const msalConfig: Configuration = {
    auth: {
        clientId: 'YOUR_CLIENT_ID',
        authority: 'https://login.microsoftonline.com/YOUR_TENANT_ID',
        redirectUri: window.location.origin,
    },
    cache: {
        cacheLocation: 'sessionStorage', // Safer than localStorage for tokens
    }
};

// Scopes your API requires
export const loginRequest: PopupRequest = {
    scopes: ['api://YOUR_CLIENT_ID/access_as_user']
};