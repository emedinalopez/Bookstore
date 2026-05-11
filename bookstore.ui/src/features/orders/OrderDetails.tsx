import React, { useEffect, useState } from 'react';
import { useParams, Link } from 'react-router-dom';
import { useKeycloak } from '@react-keycloak/web';
import { Card, ListGroup, Button, Spinner } from 'react-bootstrap';
import { getOrderById } from '../../api/orderApi';
import { OrderDto } from '../../models/order';

export const OrderDetails: React.FC = () => {
    const { id } = useParams<{ id: string }>();
    const { keycloak, initialized } = useKeycloak();

    const [order, setOrder] = useState<OrderDto | null>(null);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        const fetchOrder = async () => {
            if (initialized && keycloak.token && id) {
                setLoading(true);
                try {
                    const data = await getOrderById(Number(id), keycloak.token);
                    setOrder(data);
                } catch (err: any) {
                    setError(err.message);
                } finally {
                    setLoading(false);
                }
            }
        };

        fetchOrder();
    }, [initialized, keycloak.token, id]);

    if (loading) return <Spinner animation="border" />;
    if (error) return <p className="text-danger">Error: {error}</p>;
    if (!order) return <p>Order not found.</p>;
    
    const totalPrice = order.orderItems.reduce((sum, item) => sum + (item.unitPrice * item.quantity), 0);

    return (
        <Card>
            <Card.Header as="h3">Order Details - ID: {order.id}</Card.Header>
            <Card.Body>
                <Card.Text><strong>Customer:</strong> {order.customerName}</Card.Text>
                <Card.Text><strong>Order Date:</strong> {new Date(order.orderDate).toLocaleString()}</Card.Text>
                <Card.Text><strong>Status:</strong> {order.status}</Card.Text>
                <hr />
                <h4>Items Ordered</h4>
                <ListGroup variant="flush">
                    {order.orderItems.map(item => (
                        <ListGroup.Item key={item.id}>
                            Book ID: {item.bookId} - {item.quantity} x ${item.unitPrice.toFixed(2)}
                        </ListGroup.Item>
                    ))}
                </ListGroup>
            </Card.Body>
            <Card.Footer className="d-flex justify-content-between align-items-center">
                <h4>Total: ${totalPrice.toFixed(2)}</h4>
                <Link to="/orders">
                    <Button variant="secondary">Back to Order History</Button>
                </Link>
            </Card.Footer>
        </Card>
    );
};