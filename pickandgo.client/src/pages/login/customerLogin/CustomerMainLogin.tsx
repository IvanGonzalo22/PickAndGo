import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import CustomerSignupModal from './CustomerSignupModal';
import CustomerRecoverPasswordModal from './CustomerRecoverPasswordModal';

const CustomerMainLogin: React.FC = () => {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');
  const [isSignupOpen, setIsSignupOpen] = useState(false);
  const [isRecoverPasswordOpen, setIsRecoverPasswordOpen] = useState(false);
  const navigate = useNavigate();
  const apiUrl = "http://localhost:5256/api";

  const handleSubmit = async (event: React.FormEvent) => {
    event.preventDefault();

    const loginData = {
      email,
      password,
    };

    try {
      const response = await fetch(`${apiUrl}/auth/register`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(loginData),
      });

      const result = await response.json();

      if (response.ok) {

        localStorage.setItem('token', result.Token);
        localStorage.setItem('role', result.Role);

        const decodedToken = JSON.parse(atob(result.Token.split('.')[1]));
        const userRole = decodedToken.role;

        if (userRole === 'Customer') {
          navigate('/customerPages/CustomerMain');
        } else if (userRole === 'Admin') {
          navigate('/adminPages/AdminDashboard');
        } else {
          setError('Incorrect role.');
        }
      } else {
        setError(result.Message || 'Login failed');
      }
    } catch (error) {
      console.error('Error logging in:', error);
      setError('An error occurred. Please try again.');
    }
  };

  return (
    <div className="min-h-screen flex flex-col items-center justify-center px-4 bg-gray-100">
      <p className="text-xl font-semibold mb-8">Welcome, Customer!</p>

      <h1 className="text-lg font-medium mb-6">
        Create a new account{' '}
        <span
          className="text-blue-600 cursor-pointer"
          onClick={() => setIsSignupOpen(true)}
        >
          here
        </span>
      </h1>

      <form onSubmit={handleSubmit} className="w-full max-w-md">
        <input
          type="email"
          placeholder="Enter your email"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          className="w-full p-3 mb-4 border border-gray-300 rounded-md"
        />

        <input
          type="password"
          placeholder="Enter your password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          className="w-full p-3 mb-4 border border-gray-300 rounded-md"
        />

        <button
          type="submit"
          className="text-blue-600 font-semibold w-full p-3 mb-6 border border-gray-300 rounded-md"
        >
          Log in
        </button>
      </form>

      {error && <div className="text-red-600">{error}</div>}

      <p
        className="text-blue-600 cursor-pointer"
        onClick={() => setIsRecoverPasswordOpen(true)}
      >
        Recover password
      </p>

      {isSignupOpen && <CustomerSignupModal onClose={() => setIsSignupOpen(false)} />}
      {isRecoverPasswordOpen && <CustomerRecoverPasswordModal onClose={() => setIsRecoverPasswordOpen(false)} />}
    </div>
  );
};

export default CustomerMainLogin;
