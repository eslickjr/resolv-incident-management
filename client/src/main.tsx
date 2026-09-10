import ReactDOM from 'react-dom/client'
import { createBrowserRouter, RouterProvider } from 'react-router-dom'
import { PublicClientApplication } from '@azure/msal-browser'
import { MsalProvider } from '@azure/msal-react'
import { msalConfig } from './auth/msalConfig'

import App from './App'
import CustomerCareIncidents from './pages/CustomerCareIncidents'
import NoAccount from './pages/NoAccount'
import CustomerAccount from './pages/CustomerAccount'
import UserStats from './pages/UserStats'

const msalInstance = new PublicClientApplication(msalConfig);


const router = createBrowserRouter([
    {
        path: '/',
        element: <App />,
        errorElement: <h1 className='display-2'>Wrong page!</h1>,
        children: [
            {
                index: true,
                element: <CustomerCareIncidents />
            }, {
                path: '/noAccount/:incidentId',
                element: <NoAccount />
            }, {
                path: '/customerAccount/:incidentId/:branch/:account',
                element: <CustomerAccount />
            }, {
                path: '/userStats',
                element: <UserStats />
            }
        ]
    }
])

ReactDOM.createRoot(document.getElementById('root')!).render(
    <MsalProvider instance={msalInstance}>
        <RouterProvider router={router} />
    </MsalProvider>
)