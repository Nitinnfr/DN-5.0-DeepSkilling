import React from 'react';
import './App.css';

function App() {
  // Heading element definition
  const element = "Office Space";

  // Image source path (using a placeholder or direct image link)
  const sr = "https://images.unsplash.com/photo-1497366216548-37526070297c?auto=format&fit=crop&w=500&q=80";

  // Image JSX attribute
  const jsxatt = <img src={sr} width="25%" height="25%" alt="Office Space" />;

  // List of Office Objects
  const offices = [
    { Name: "DBS", Rent: 50000, Address: "Chennai" },
    { Name: "Regus", Rent: 70000, Address: "Bangalore" },
    { Name: "WeWork", Rent: 55000, Address: "Mumbai" }
  ];

  return (
    <div style={{ padding: '20px', fontFamily: 'Arial, sans-serif' }}>
      {/* Page Heading */}
      <h1>{element} , at Affordable Range</h1>

      {/* Office Space Image[cite: 2] */}
      {jsxatt}

      {/* Looping through list of office objects[cite: 2] */}
      {offices.map((ItemName, index) => {
        // Logic to push dynamic CSS class based on rent value[cite: 2]
        let colors = [];
        if (ItemName.Rent <= 60000) {
          colors.push('textRed');
        } else {
          colors.push('textGreen');
        }

        return (
          <div key={index} style={{ marginBottom: '30px' }}>
            <h1>Name: {ItemName.Name}</h1>
            <h3 className={colors.join(' ')}>
              Rent: Rs. {ItemName.Rent}
            </h3>
            <h3>Address: {ItemName.Address}</h3>
          </div>
        );
      })}
    </div>
  );
}

export default App;