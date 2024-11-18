import React, { useState } from 'react';

const AdminEmployeeSignup: React.FC = () => {
  const [name, setName] = useState('');
  const [surname, setSurname] = useState('');
  const [mobile, setMobile] = useState('');
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');
  const [successMessage, setSuccessMessage] = useState('');

  const apiUrl = "http://localhost:5256/api";

  const handleSubmit = async (event: React.FormEvent) => {
    event.preventDefault();

    const employeeData = {
      firstName: name,
      lastName: surname,
      phone: mobile,
      email: email,
      password: password,
      role: 'Employee'
    };

    try {
      const response = await fetch(`${apiUrl}/auth/register`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(employeeData),
      });

      const result = await response.json();
      console.log('API Response:', result);

      if (response.ok) {
        setSuccessMessage('Employee registered successfully');
        setName('');
        setSurname('');
        setMobile('');
        setEmail('');
        setPassword('');
        setError('');
      } else {
        setError(result.Message || 'Something went wrong.');
      }
    } catch (error) {
      console.error('Error registering employee:', error);
      setError('An error occurred. Please try again.');
    }
  };

  return (
    <div className="min-h-screen flex flex-col items-center justify-center px-4 bg-gray-100">
      <h2 className="text-xl font-semibold mb-8">Employee Signing Up for Admin</h2>

      {error && <div className="text-red-500 mb-3">{error}</div>}

      {successMessage && <div className="text-green-500 mb-3">{successMessage}</div>}

      <form onSubmit={handleSubmit} className="space-y-4 w-full max-w-md">
        <input
          type="text"
          placeholder="Name"
          value={name}
          onChange={(e) => setName(e.target.value)}
          className="w-full p-3 border border-gray-300 rounded-md"
        />

        <input
          type="text"
          placeholder="Surname"
          value={surname}
          onChange={(e) => setSurname(e.target.value)}
          className="w-full p-3 border border-gray-300 rounded-md"
        />

        <input
          type="text"
          placeholder="Mobile Phone"
          value={mobile}
          onChange={(e) => setMobile(e.target.value)}
          className="w-full p-3 border border-gray-300 rounded-md"
        />

        <input
          type="email"
          placeholder="Email"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          className="w-full p-3 border border-gray-300 rounded-md"
        />

        <input
          type="password"
          placeholder="Password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          className="w-full p-3 border border-gray-300 rounded-md"
        />

        <button type="submit" className="text-blue-600 font-semibold w-full p-3 border border-gray-300 rounded-md">
          Create Employee
        </button>
      </form>
    </div>
  );
};

export default AdminEmployeeSignup;
