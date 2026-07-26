// src/Posts.js
import React, { Component } from 'react';
import Post from './Post';

class Posts extends Component {
  // Step 5: Initialize component state in constructor
  constructor(props) {
    super(props);
    this.state = {
      posts: [],
      error: null
    };
  }

  // Step 6: Create method to fetch posts from external API
  loadPosts = () => {
    fetch('https://jsonplaceholder.typicode.com/posts')
      .then((response) => {
        if (!response.ok) {
          throw new Error('Failed to fetch posts');
        }
        return response.json();
      })
      .then((data) => {
        this.setState({ posts: data });
      })
      .catch((err) => {
        alert(`Error fetching posts: ${err.message}`);
        this.setState({ error: err.message });
      });
  };

  // Step 7: Call loadPosts() in componentDidMount lifecycle hook
  componentDidMount() {
    this.loadPosts();
  }

  // Step 9: Catch rendering errors in child components
  componentDidCatch(error, info) {
    alert(`An error occurred in Posts component: ${error.toString()}`);
    console.error('Error Details:', info);
  }

  // Step 8: Render title and post content
  render() {
    const { posts, error } = this.state;

    if (error) {
      return <h2>Something went wrong while loading posts.</h2>;
    }

    return (
      <div style={{ padding: '20px', maxWidth: '800px', margin: '0 auto', fontFamily: 'Arial, sans-serif' }}>
        <h1>Blog Posts</h1>
        {posts.length === 0 ? (
          <p>Loading posts...</p>
        ) : (
          posts.map((post) => (
            <Post key={post.id} title={post.title} body={post.body} />
          ))
        )}
      </div>
    );
  }
}

export default Posts;