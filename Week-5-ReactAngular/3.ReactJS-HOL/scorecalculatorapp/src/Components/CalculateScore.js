// src/Components/CalculateScore.js
import React from 'react';
import '../Stylesheets/mystyle.css';

export const CalculateScore = ({ Name, School, Total, goal }) => {
  // Calculate average score per goal/subject
  const average = (Total / goal).toFixed(2);

  return (
    <div className="formatstyle">
      <h1><span className="textstyle">Student Details</span></h1>
      <div className="Name">
        <b>Name: </b> {Name}
      </div>
      <div className="School">
        <b>School: </b> {School}
      </div>
      <div className="Total">
        <b>Total Score: </b> {Total}
      </div>
      <div className="Score">
        <b>Average Score: </b> {average}
      </div>
    </div>
  );
};

export default CalculateScore;