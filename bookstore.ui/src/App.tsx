import React from 'react';
import { BrowserRouter as Router, Route, Routes, Link } from 'react-router-dom';
import { CartProvider, useCart } from './context/CartContext';
import { HomePage } from './pages/HomePage';
import { BooksPage } from './pages/BooksPage';
import { BookDetails } from './features/books/BookDetails';
import { PlaceOrderForm } from './features/orders/PlaceOrderForm';
import { OrdersPage } from './pages/OrdersPage';
import { OrderDetails } from './features/orders/OrderDetails';
import { CategoriesPage } from './pages/CategoriesPage';

const CartIndicator = () => {
    const { cartItems } = useCart();
    const itemCount = cartItems.reduce((sum, item) => sum + item.orderQuantity, 0);
    return (
        <Link to="/place-order">
            Cart ({itemCount})
        </Link>
    );
};

function App() {
  return (    
    <CartProvider>
        <Router>
            <div style={{ padding: '20px', fontFamily: 'Arial, sans-serif' }}>
                <header style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '20px', borderBottom: '1px solid #ccc', paddingBottom: '10px' }}>
                    <Link to="/" style={{ textDecoration: 'none', color: 'black' }}>
                        <h1>Bookstore</h1>
                    </Link>
                    <nav style={{ display: 'flex', gap: '20px' }}>
                        <Link to="/">Dashboard</Link>
                        <Link to="/books">Inventory</Link>
                        <Link to="/categories">Categories</Link>
                        <Link to="/orders">Orders</Link>                        
                        <CartIndicator />
                    </nav>
                </header>
                
                <main>
                    <Routes>
                        <Route path="/" element={<HomePage />} />
                        <Route path="/books" element={<BooksPage />} />
                        <Route path="/books/:id" element={<BookDetails />} />                        
                        <Route path="/place-order" element={<PlaceOrderForm />} />                        
                        <Route path="/orders" element={<OrdersPage />} />
                        <Route path="/orders/:id" element={<OrderDetails />} />
                        <Route path="/categories" element={<CategoriesPage />} />
                    </Routes>
                </main>
            </div>
        </Router>
    </CartProvider>
  );
}

export default App;