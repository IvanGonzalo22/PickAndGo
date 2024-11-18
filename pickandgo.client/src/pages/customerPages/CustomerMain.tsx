import React, { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';

const CustomerMain: React.FC = () => {
  const navigate = useNavigate();

  useEffect(() => {
    const role = localStorage.getItem('role');
    if (role !== 'Customer') {
      navigate('/login');
    }
  }, [navigate]);

  return (
    <div>
      <h1>Welcome, Customer!</h1>
    </div>
  );
};

export default CustomerMain;
