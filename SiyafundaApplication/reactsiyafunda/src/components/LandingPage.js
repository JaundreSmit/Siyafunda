import React from 'react';
import 'bootstrap/dist/css/bootstrap.min.css'; 
import { useNavigate } from 'react-router-dom'; 

const LandingPage = () => {
    const navigate = useNavigate(); 

    return (
        <div className="text-center">
            <h1>Welcome to Siyafunda!</h1>
            <button className="btn btn-primary m-2" onClick={() => navigate('/login')}>
                Log In
            </button>
            <button className="btn btn-secondary m-2" onClick={() => navigate('/signup')}>
                Sign Up
            </button>
        </div>
    );
};

export default LandingPage;

