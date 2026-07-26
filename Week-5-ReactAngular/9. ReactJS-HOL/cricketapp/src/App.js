import React from 'react';
import { players, ListofPlayers, Scorebelow70 } from './ListofPlayers';
import { IndianTeam, OddPlayers, EvenPlayers, IndianPlayers, ListofIndianPlayers } from './IndianPlayers';

function App() {
  // Set flag to true or false to change the rendered output
  var flag = true;

  if (flag === true) {
    return (
      <div style={{ padding: '20px' }}>
        <h1>List of Players</h1>
        <ListofPlayers players={players} />
        <hr />
        <h1>List of Players having Scores Less than 70</h1>
        <Scorebelow70 players={players} />
      </div>
    );
  } else {
    return (
      <div style={{ padding: '20px' }}>
        <div>
          <div>
            <h1>Indian Team</h1>
            <h1>Odd Players</h1>
            {OddPlayers(IndianTeam)}
            <hr />
            <h1>Even Players</h1>
            {EvenPlayers(IndianTeam)}
          </div>
          <hr />
          <div>
            <h1>List of Indian Players Merged:</h1>
            <ListofIndianPlayers IndianPlayers={IndianPlayers} />
          </div>
        </div>
      </div>
    );
  }
}

export default App;