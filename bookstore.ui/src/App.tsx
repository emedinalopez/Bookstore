import React from 'react';
import { BrowserRouter as Router, Route, Routes, Link } from 'react-router-dom';
import { BookList } from './features/books/BookList';
import { BookDetails } from './features/books/BookDetails';

function App() {
  return (
    <Router>
      <div style={{ padding: '20px', fontFamily: 'Arial, sans-serif' }}>
        <header style={{ marginBottom: '20px', borderBottom: '1px solid #ccc', paddingBottom: '10px' }}>
          <Link to="/" style={{ textDecoration: 'none', color: 'black' }}>
            <h1>Bookstore Management System</h1>
          </Link>
        </header>
        
        <main>
          <Routes>
            <Route path="/" element={<BookList />} />
            <Route path="/books/:id" element={<BookDetails />} />
            {/* Will add the rest of the routes as they are developed */}
          </Routes>
        </main>
      </div>
    </Router>
  );
}

export default App;
