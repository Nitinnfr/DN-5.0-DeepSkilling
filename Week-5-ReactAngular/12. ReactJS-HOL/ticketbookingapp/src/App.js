// src/App.js
import React, { useState } from 'react';
import { Greeting, LoginButton, LogoutButton } from './Components';

function App() {
  const [isLoggedIn, setIsLoggedIn] = useState(false);

  // Event handlers to manage login state
  const handleLoginClick = () => {
    setIsLoggedIn(true);
  };

  const handleLogoutClick = () => {
    setIsLoggedIn(false);
  };

  // Element variable for conditional button rendering
  let button;
  if (isLoggedIn) {
    button = <LogoutButton onClick={handleLogoutClick} />;
  } else {
    button = <LoginButton onClick={handleLoginClick} />;
  }

  return (
    <div style={{ padding: '40px', fontFamily: 'sans-serif' }}>
      {/* Dynamic Greeting Component */}
      <Greeting isLoggedIn={isLoggedIn} />
      
      {/* Rendered Element Variable Button */}
      {button}
    </div>
  );
}

export default App;