// src/BookDetails.js
import React from 'react';

function BookDetails(props) {
  // Element Variable approach to store mapped JSX elements
  const bookdet = (
    <ul style={{ listStyleType: 'none', padding: 0 }}>
      {props.books.map((book) => (
        <div key={book.id} style={{ marginBottom: '15px' }}>
          <h3>{book.bname}</h3>
          <h4>{book.price}</h4>
        </div>
      ))}
    </ul>
  );

  return (
    <div className="st2">
      <h1>Book Details</h1>
      {bookdet}
    </div>
  );
}

export default BookDetails;