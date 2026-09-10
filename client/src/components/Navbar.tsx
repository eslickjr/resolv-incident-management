import logo from '../assets/resolv-logo.svg';
import { Link, useNavigate, useLocation } from 'react-router-dom';
import { useState } from 'react';
import Confirm from './Confirm';
import '../styles/Navbar.css';

const Navbar = () => {
    const navigate = useNavigate();
    const location = useLocation();
    const [isModalOpen, setIsModalOpen] = useState<boolean>(false);
    const [pendingPath, setPendingPath] = useState<string>('/');

    const isOnIncidentPage = location.pathname.startsWith('/noAccount') ||
                             location.pathname.startsWith('/customerAccount');

    const isHome = location.pathname === '/' ||
                   location.pathname.startsWith('/noAccount') ||
                   location.pathname.startsWith('/customerAccount');

    const isStats = location.pathname === '/userStats';

    const handleNavClick = (e: React.MouseEvent<HTMLAnchorElement>, to: string) => {
        if (isOnIncidentPage) {
            e.preventDefault();
            setPendingPath(to);
            setIsModalOpen(true);
        }
    };

    const handleConfirm = async () => {
        setIsModalOpen(false);
        window.dispatchEvent(new Event('saveIncidentTime'));
        setTimeout(() => navigate(pendingPath), 100);
    };

    return (
        <>
            <nav id="navContainer">
                <div id="navLeft">
                    <Link to="/" id="navBrand" onClick={(e) => handleNavClick(e, '/')}>
                        <img src={logo} alt="Resolv" id="logo" />
                        <div id="navBrandTextGroup">
                            <span id="navBrandText">Resolv</span>
                            <span id="navBrandSubText">Incident Management</span>
                        </div>
                    </Link>
                </div>
                <div id="navRight">
                    <Link
                        to="/"
                        className={`navLink${isHome ? ' navLinkActive' : ''}`}
                        onClick={(e) => handleNavClick(e, '/')}
                    >
                        Home
                    </Link>
                    <Link
                        to="/userStats"
                        className={`navLink${isStats ? ' navLinkActive' : ''}`}
                        onClick={(e) => handleNavClick(e, '/userStats')}
                    >
                        Stats
                    </Link>
                </div>
            </nav>
            <Confirm
                isOpen={isModalOpen}
                onClose={() => setIsModalOpen(false)}
                onConfirm={handleConfirm}
                mode="update"
                skipNavigate={true}
            />
        </>
    );
};

export default Navbar;