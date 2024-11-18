import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import EmployeeRecoverPasswordModal from './EmployeeRecoverPasswordModal';

const EmployeeMainLogin: React.FC = () => {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');
  const [isRecoverPasswordOpen, setIsRecoverPasswordOpen] = useState(false);
  const navigate = useNavigate();

  const apiUrl = "http://localhost:5256/api";

  const handleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();

    const loginData = {
      email: email,
      password: password,
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

        localStorage.setItem('authToken', result.Token);
        localStorage.setItem('role', result.role);

        if (result.role === 'Employee') {
          navigate('/employee');
        } else {
          navigate('/');
        }
      } else {
        setError(result.Message || 'Invalid credentials');
      }
    } catch (error) {
      console.error('Error logging in:', error);
      setError('An error occurred. Please try again.');
    }
  };

  return (
    <div className="min-h-screen flex flex-col items-center justify-center px-4 bg-gray-100">
      <p className="text-xl font-semibold mb-8">Welcome, Employee!</p>

      <form onSubmit={handleSubmit} className="space-y-4 w-full max-w-md">
        <input
          type="email"
          placeholder="Enter your email"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          className="w-full max-w-md p-3 mb-4 border border-gray-300 rounded-md"
        />

        <input
          type="password"
          placeholder="Enter your password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          className="w-full max-w-md p-3 mb-4 border border-gray-300 rounded-md"
        />

        <button
          type="submit"
          className="text-blue-600 font-semibold w-full p-3 mb-6 border border-gray-300 rounded-md"
        >
          Log in
        </button>
      </form>

      {error && <div className="text-red-500 mb-6">{error}</div>}

      <p
        className="text-blue-600 cursor-pointer"
        onClick={() => setIsRecoverPasswordOpen(true)}
      >
        Recover password
      </p>

      {isRecoverPasswordOpen && (
        <EmployeeRecoverPasswordModal onClose={() => setIsRecoverPasswordOpen(false)} />
      )}
    </div>
  );
};

export default EmployeeMainLogin;
