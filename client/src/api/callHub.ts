import * as signalR from '@microsoft/signalr';
import { CallMatchResult } from '../interfaces/Types';

let connection: signalR.HubConnection | null = null;

export const startCallHub = (onIncomingCall: (result: CallMatchResult) => void) => {
    connection = new signalR.HubConnectionBuilder()
        .withUrl('/hubs/call')
        .withAutomaticReconnect()
        .build();

    connection.on('IncomingCall', (result: CallMatchResult) => {
        onIncomingCall(result);
    });

    connection.start().catch(err => console.error('SignalR connection error:', err));
};

export const stopCallHub = () => {
    connection?.stop();
    connection = null;
};