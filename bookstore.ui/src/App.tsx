import React from 'react';
import { BookList } from './features/books/BookList';

function App() {
  return (
    <div style={{ padding: '20px', fontFamily: 'Arial, sans-serif' }}>
      <header style={{ marginBottom: '20px', borderBottom: '1px solid #ccc' }}>
        <h1>Bookstore Management System</h1>
      </header>
      
      <main>        
        <BookList />
      </main>
    </div>
  );
}

export default App;
