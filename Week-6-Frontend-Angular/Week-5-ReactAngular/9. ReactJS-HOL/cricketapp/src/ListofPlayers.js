import React from 'react';

// Main list of 11 players with names and scores
export const players = [
  { name: 'Jack', score: 50 },
  { name: 'Michael', score: 70 },
  { name: 'John', score: 40 },
  { name: 'Ann', score: 61 },
  { name: 'Elisabeth', score: 61 },
  { name: 'Sachin', score: 95 },
  { name: 'Dhoni', score: 100 },
  { name: 'Virat', score: 84 },
  { name: 'Jadeja', score: 64 },
  { name: 'Raina', score: 75 },
  { name: 'Rohit', score: 80 }
];

// Component to render all players using Array.map()
export function ListofPlayers({ players }) {
  return (
    <div>
      {players.map((item, index) => (
        <div key={index}>
          <li>Mr. {item.name} <span>{item.score}</span></li>
        </div>
      ))}
    </div>
  );
}

// Component to render players with scores <= 70
export function Scorebelow70({ players }) {
  const players70 = [];
  
  players.map((item) => {
    if (item.score <= 70) {
      players70.push(item);
    }
    return item;
  });

  return (
    <div>
      {players70.map((item, index) => (
        <div key={index}>
          <li>Mr. {item.name} {item.score}</li>
        </div>
      ))}
    </div>
  );
}