// src/Post.js
import React from 'react';

class Post extends React.Component {
  render() {
    const { title, body } = this.props;
    return (
      <div style={{ borderBottom: '1px solid #ccc', marginBottom: '15px', paddingBottom: '10px' }}>
        <h3 style={{ color: '#2c3e50', textTransform: 'capitalize' }}>{title}</h3>
        <p style={{ color: '#555' }}>{body}</p>
      </div>
    );
  }
}

export default Post;