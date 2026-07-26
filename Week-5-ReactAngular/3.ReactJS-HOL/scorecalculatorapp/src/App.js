// src/App.js
import React from 'react';
import CalculateScore from './Components/CalculateScore';

function App() {
  return (
    <div className="App">
      <CalculateScore 
        Name="John Doe" 
        School="Greenwood High School" 
        Total={450} 
        goal={5} 
      />
    </div>
  );
}

export default App;