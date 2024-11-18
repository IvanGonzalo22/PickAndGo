import React, { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';

const EmployeeMain: React.FC = () => {
  const navigate = useNavigate();

  useEffect(() => {
      const authToken = localStorage.getItem('authToken');
      const role = localStorage.getItem('role');

      if (!authToken || role !== 'Employee') {
          navigate('/login');
      }
  }, [navigate]);

  return (
      <div>
          <h1>Welcome, Employee!</h1>
      </div>
  );
};

export default EmployeeMain;
