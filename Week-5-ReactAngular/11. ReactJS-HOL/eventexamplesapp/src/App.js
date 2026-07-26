// src/App.js
import React, { useState } from 'react';
import CurrencyConvertor from './CurrencyConvertor';

function App() {
  const [counter, setCounter] = useState(5);

  // Method 1: Increment Counter
  const handleIncrement = () => {
    setCounter((prev) => prev + 1);
  };

  // Method 2: Alert static message
  const sayHello = () => {
    alert('Hello! Member1');
  };

  // Multiple method invocation for Increment button
  const handleMultipleIncrements = () => {
    handleIncrement();
    sayHello();
  };

  // Decrement counter method
  const handleDecrement = () => {
    setCounter((prev) => prev - 1);
  };

  // Method accepting an argument
  const sayWelcome = (message) => {
    alert(message);
  };

  // Synthetic event handler
  const handleCustomClick = (e) => {
    // Demonstrating Synthetic Event usage
    alert('I was clicked');
  };

  return (
    <div style={{ padding: '20px', fontFamily: 'sans-serif' }}>
      {/* Counter Display */}
      <p style={{ fontSize: '18px' }}>{counter}</p>

      {/* Control Buttons */}
      <div style={{ display: 'flex', flexDirection: 'column', width: '120px', gap: '5px' }}>
        {/* Invokes multiple methods (Increment + Say Hello) */}
        <button onClick={handleMultipleIncrements}>Increment</button>
        
        {/* Decrement Counter */}
        <button onClick={handleDecrement}>Decrement</button>
        
        {/* Passes "welcome" parameter to function */}
        <button onClick={() => sayWelcome('welcome')}>Say welcome</button>
        
        {/* Invokes synthetic event handling */}
        <button onClick={handleCustomClick}>Click on me</button>
      </div>

      <br />

      {/* Currency Convertor Component */}
      <CurrencyConvertor />
    </div>
  );
}

export default App;