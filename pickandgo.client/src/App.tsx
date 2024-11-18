import React from 'react';
import { BrowserRouter as Router, Route, Routes, Navigate } from 'react-router-dom';
import CustomerMainLogin from './pages/login/customerLogin/CustomerMainLogin';
import EmployeeMainLogin from './pages/login/employeeLogin/EmployeeMainLogin';
import AdminEmployeeSignup from './pages/login/AdminEmployeeSignup';

const App: React.FC = () => {
  return (
    <Router>
      <Routes>

        <Route path="/" element={<Navigate to="/customer-login" />} />
        
        <Route path="/customer-login" element={<CustomerMainLogin />} />
        
        <Route path="/employee-login" element={<EmployeeMainLogin />} />
        
        <Route path="/admin-employee-signup" element={<AdminEmployeeSignup />} />
        
      </Routes>
    </Router>
  );
};

export default App;
