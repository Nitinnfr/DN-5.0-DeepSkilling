// src/BlogDetails.js
import React from 'react';

function BlogDetails(props) {
  const content = props.blogs.map((blog) => (
    <div key={blog.id} style={{ marginBottom: '20px' }}>
      <h2>{blog.title}</h2>
      <p style={{ fontWeight: 'bold' }}>{blog.author}</p>
      <p style={{ color: '#555' }}>{blog.content}</p>
    </div>
  ));

  return (
    <div className="v1">
      <h1>Blog Details</h1>
      {/* Short-circuit evaluation */}
      {props.showBlogs && content}
    </div>
  );
}

export default BlogDetails;