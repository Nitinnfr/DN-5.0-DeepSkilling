// src/CourseDetails.js
import React from 'react';

function CourseDetails(props) {
  return (
    <div className="mystyle1">
      <h1>Course Details</h1>
      {/* Conditional rendering using Ternary Operator */}
      {props.courses && props.courses.length > 0 ? (
        props.courses.map((course) => (
          <div key={course.id} style={{ marginBottom: '20px' }}>
            <h2>{course.cname}</h2>
            <p style={{ fontWeight: 'bold' }}>{course.date}</p>
          </div>
        ))
      ) : (
        <p>No courses available</p>
      )}
    </div>
  );
}

export default CourseDetails;