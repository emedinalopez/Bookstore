import React from 'react';
import { BrowserRouter as Router, Route, Routes, Link } from 'react-router-dom';
import { HomePage } from './pages/HomePage';
import { BooksPage } from './pages/BooksPage';
import { BookDetails } from './features/books/BookDetails';

function App() {
  return (
    <Router>
      <div style={{ padding: '20px', fontFamily: 'Arial, sans-serif' }}>
        
        {/* Navigation Bar */}
        <header style={{ 
            marginBottom: '20px', 
            borderBottom: '1px solid #ccc', 
            paddingBottom: '10px',
            display: 'flex',
            justifyContent: 'space-between',
            alignItems: 'center'
        }}>
          <Link to="/" style={{ textDecoration: 'none', color: '#2c3e50' }}>
            <h1 style={{ margin: 0 }}>Bookstore Management System</h1>
          </Link>
          
          <nav style={{ display: 'flex', gap: '15px' }}>
             <Link to="/">Dashboard</Link>
             <Link to="/books">Inventory</Link>
             <Link to="/orders">Orders</Link>
          </nav>
        </header>
        
        {/* Main Content Area */}
        <main>
          <Routes>            
            <Route path="/" element={<HomePage />} />

            <Route path="/books" element={<BooksPage />} />
            <Route path="/books/:id" element={<BookDetails />} />            
            
            <Route path="/orders" element={<div>Orders Page is under construction</div>} />
          </Routes>
        </main>
        
      </div>
    </Router>
  );
}

export default App;
