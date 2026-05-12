import React from 'react';
import { Link } from 'react-router-dom';
import { Card, Button, Row, Col } from 'react-bootstrap';

export const HomePage: React.FC = () => {
    return (
        <div>
            <h2>Welcome to the Dashboard</h2>
            <p>Select a module to manage your store:</p>
            
            <Row className="mt-4">
                <Col md={6} lg={4} className="mb-3">                    
                    <Card>
                        <Card.Body>
                            <Card.Title>📚 Inventory</Card.Title>
                            <Card.Text>
                                Browse the book catalog, manage stock, and view book details.
                            </Card.Text>
                            <Link to="/books">
                                <Button variant="primary">Go to Inventory</Button>
                            </Link>
                        </Card.Body>
                    </Card>
                </Col>
                <Col md={6} lg={4} className="mb-3">
                    <Card>
                        <Card.Body>
                            <Card.Title>📦 Orders</Card.Title>
                            <Card.Text>
                                View order history, check order statuses, and place new orders.
                            </Card.Text>
                            <Link to="/orders">
                                <Button variant="primary">Go to Orders</Button>
                            </Link>
                        </Card.Body>
                    </Card>
                </Col>
            </Row>
        </div>
    );
};