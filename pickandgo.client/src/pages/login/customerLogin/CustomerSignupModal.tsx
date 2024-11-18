import React, { useState } from 'react';

interface CustomerSignupModalProps {
  onClose: () => void;
}

const CustomerSignupModal: React.FC<CustomerSignupModalProps> = ({ onClose }) => {
  const [name, setName] = useState('');
  const [surname, setSurname] = useState('');
  const [mobile, setMobile] = useState('');
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');

  const apiUrl = "http://localhost:5256/api";

  const handleSubmit = async (event: React.FormEvent) => {
    event.preventDefault();

    const userData = {
      firstName: name,
      lastName: surname,
      phone: mobile,
      email: email,
      password: password,
      role: 'Customer'
    };

    try {
      const response = await fetch(`${apiUrl}/auth/register`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(userData),
      });

      const result = await response.json();
      console.log('API Response:', result);

      if (response.ok) {
        console.log('User registered successfully');
        onClose();
      } else {
        setError(result.Message || 'Something went wrong.');
      }
    } catch (error) {
      console.error('Error registering user:', error);
      setError('An error occurred. Please try again.');
    }
  };

  return (
    <div className="fixed inset-0 flex items-center justify-center bg-black bg-opacity-50 z-50">
      <div className="bg-white p-6 rounded-lg shadow-lg w-80 max-w-full">
        <h2 className="text-xl font-semibold mb-4">Sign Up</h2>

        {error && <div className="text-red-500 mb-3">{error}</div>}

        <input
          type="text"
          placeholder="Name"
          value={name}
          onChange={(e) => setName(e.target.value)}
          className="w-full p-3 mb-3 border border-gray-300 rounded-md"
        />

        <input
          type="text"
          placeholder="Surname"
          value={surname}
          onChange={(e) => setSurname(e.target.value)}
          className="w-full p-3 mb-3 border border-gray-300 rounded-md"
        />

        <input
          type="text"
          placeholder="Mobile Phone"
          value={mobile}
          onChange={(e) => setMobile(e.target.value)}
          className="w-full p-3 mb-3 border border-gray-300 rounded-md"
        />

        <input
          type="email"
          placeholder="Email"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          className="w-full p-3 mb-3 border border-gray-300 rounded-md"
        />

        <input
          type="password"
          placeholder="Password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          className="w-full p-3 mb-4 border border-gray-300 rounded-md"
        />

        <div className="flex justify-end space-x-3">
          <button onClick={onClose} className="text-gray-600">Cancel</button>
          <button onClick={handleSubmit} className="text-blue-600 font-semibold">Create Account</button>
        </div>
      </div>
    </div>
  );
};

export default CustomerSignupModal;
