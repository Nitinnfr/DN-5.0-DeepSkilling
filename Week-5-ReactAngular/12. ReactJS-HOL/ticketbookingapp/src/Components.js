// src/Components.js
import React from 'react';

// Displays when the user is logged in
export function UserGreeting() {
  return <h1>Welcome back</h1>;
}

// Displays when the user is a guest (logged out)
export function GuestGreeting() {
  return <h1>Please sign up.</h1>;
}

// Renders either UserGreeting or GuestGreeting based on isLoggedIn prop
export function Greeting(props) {
  const isLoggedIn = props.isLoggedIn;
  if (isLoggedIn) {
    return <UserGreeting />;
  }
  return <GuestGreeting />;
}

// Login Button Component
export function LoginButton(props) {
  return (
    <button onClick={props.onClick}>
      Login
    </button>
  );
}

// Logout Button Component
export function LogoutButton(props) {
  return (
    <button onClick={props.onClick}>
      Logout
    </button>
  );
}