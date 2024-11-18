import React, { useState } from 'react';

interface CustomerRecoverPasswordModalProps {
  onClose: () => void;
}

const CustomerRecoverPasswordModal: React.FC<CustomerRecoverPasswordModalProps> = ({ onClose }) => {
  const [email, setEmail] = useState('');

  const handleSubmit = (event: React.FormEvent) => {
    event.preventDefault();
    console.log('Password recovery email sent to:', email);
    onClose();
  };

  return (
    <div className="fixed inset-0 flex items-center justify-center bg-black bg-opacity-50 z-50">
      <div className="bg-white p-6 rounded-lg shadow-lg w-80 max-w-full">
        <h2 className="text-xl font-semibold mb-4">Recover Password</h2>

        <input
          type="email"
          placeholder="Enter your email"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          className="w-full p-3 mb-4 border border-gray-300 rounded-md"
        />

        <div className="flex justify-end space-x-3">
          <button onClick={onClose} className="text-gray-600">Cancel</button>
          <button onClick={handleSubmit} className="text-blue-600 font-semibold">Submit</button>
        </div>
      </div>
    </div>
  );
};

export default CustomerRecoverPasswordModal;
