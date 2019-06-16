import React from 'react';
import { Link } from 'react-router-dom';

const HeaderContainer = () => {

    return (
        <div className="header">
            <nav className="navbar navbar-expand-lg navbar-light">
                <button className="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarNavDropdown" aria-controls="navbarNavDropdown" aria-expanded="false" aria-label="Toggle navigation">
                    <span className="navbar-toggler-icon"></span>
                </button>
                <div className="collapse navbar-collapse" id="navbarNavDropdown">
                    <ul className="navbar-nav">
                        <li className="nav-item active">
                            <Link className="nav-link" to="/AdminLogIn">Admin</Link>
                        </li>

                        <li className="nav-item">
                            <Link className="nav-link" to="/">Home</Link>
                        </li>
                        <li className="nav-item">
                            <Link className="nav-link" to="/viewReport">View Reports</Link>
                        </li>
                    </ul>
                </div>
            </nav>
        </div>
    );
}

export default HeaderContainer;