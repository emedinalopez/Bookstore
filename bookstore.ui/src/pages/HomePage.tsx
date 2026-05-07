import React from 'react';
import { Link } from 'react-router-dom';

export const HomePage: React.FC = () => {
    return (
        <div>
            <h2>Welcome to the Dashboard</h2>
            <p>Select a module to manage your store:</p>

            <div style={{ display: 'flex', gap: '20px', marginTop: '30px' }}>
                {/* Link to the Books Catalog */}
                <Link 
                    to="/books" 
                    style={cardStyle}
                >
                    <h3 style={{ marginTop: 0 }}>📚 Inventory</h3>
                    <p>Browse the book catalog, manage stock, and view book details.</p>
                </Link>

                {/* Link to the Orders */}
                <Link 
                    to="/orders" 
                    style={cardStyle}
                >
                    <h3 style={{ marginTop: 0 }}>📦 Orders</h3>
                    <p>View order history, check order statuses, and place new orders.</p>
                </Link>
            </div>
        </div>
    );
};

const cardStyle: React.CSSProperties = {
    display: 'block',
    border: '1px solid #ccc',
    borderRadius: '8px',
    padding: '20px',
    width: '250px',
    textDecoration: 'none',
    color: '#333',
    backgroundColor: '#f9f9f9',
    boxShadow: '0 4px 6px rgba(0,0,0,0.1)',
    transition: 'transform 0.2s'
};