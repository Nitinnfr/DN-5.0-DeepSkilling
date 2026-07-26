// src/App.js
import React from 'react';
import './App.css';
import BookDetails from './BookDetails';
import CourseDetails from './CourseDetails';
import BlogDetails from './BlogDetails';
import { books, courses, blogs } from './data';

function App() {
  const showBlogs = true;

  return (
    <div className="container">
      <div className="column border-right">
        <CourseDetails courses={courses} />
      </div>

      <div className="column border-right">
        <BookDetails books={books} />
      </div>

      <div className="column">
        <BlogDetails blogs={blogs} showBlogs={showBlogs} />
      </div>
    </div>
  );
}

export default App;