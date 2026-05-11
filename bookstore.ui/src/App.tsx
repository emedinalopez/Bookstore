import React from 'react';
import { BrowserRouter as Router, Route, Routes, Link } from 'react-router-dom';
import { CartProvider, useCart } from './context/CartContext';
import { Navbar, Nav, Container } from 'react-bootstrap';
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
        <Nav.Link as={Link} to="/place-order">
            Cart ({itemCount})
        </Nav.Link>
    );
};

function App() {
  return (    
    <CartProvider>
        <Router>
            <Navbar bg="dark" variant="dark" expand="lg" className="mb-4">
                <Container>
                    <Navbar.Brand as={Link} to="/">Bookstore</Navbar.Brand>
                    <Navbar.Toggle aria-controls="basic-navbar-nav" />
                    <Navbar.Collapse id="basic-navbar-nav">
                        <Nav className="me-auto">
                            <Nav.Link as={Link} to="/">Dashboard</Nav.Link>
                            <Nav.Link as={Link} to="/books">Inventory</Nav.Link>
                            <Nav.Link as={Link} to="/categories">Categories</Nav.Link>
                            <Nav.Link as={Link} to="/orders">Orders</Nav.Link>
                        </Nav>
                        <Nav>
                            <CartIndicator />
                        </Nav>
                    </Navbar.Collapse>
                </Container>
            </Navbar>
                        
            <Container>
                <main>
                    <Routes>                        
                        <Route path="/" element={<HomePage />} />
                        <Route path="/books" element={<BooksPage />} />
                        <Route path="/categories" element={<CategoriesPage />} />
                        <Route path="/books/:id" element={<BookDetails />} />
                        <Route path="/place-order" element={<PlaceOrderForm />} />
                        <Route path="/orders" element={<OrdersPage />} />
                        <Route path="/orders/:id" element={<OrderDetails />} />
                    </Routes>
                </main>
            </Container>
        </Router>
    </CartProvider>
  );
}

export default App;