import './App.css';
import { useEffect } from 'react';
import { Outlet } from 'react-router-dom';
import { useMsal } from '@azure/msal-react';
import Navbar from './components/Navbar';
import { loadIssuesAndSolutions } from './api/issueCache';

function App() {
    const { instance } = useMsal();

    useEffect(() => {
        loadIssuesAndSolutions(instance);
    }, []);

    return (
        <>
            <Navbar />
            <Outlet />
        </>
    );
}

export default App;