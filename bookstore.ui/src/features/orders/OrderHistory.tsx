import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import { useKeycloak } from '@react-keycloak/web';
import { Table, Button } from 'react-bootstrap';
import { OrderDto } from '../../models/order';
import { getOrders } from '../../api/orderApi';

export const OrderHistory: React.FC = () => {
    const { keycloak, initialized } = useKeycloak();
    const [orders, setOrders] = useState<OrderDto[]>([]);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        const fetchOrders = async () => {
            if (initialized && keycloak.token) {
                setLoading(true);
                try {
                    const data = await getOrders(keycloak.token);
                    setOrders(data);
                } catch (err: any) {
                    setError(err.message);
                } finally {
                    setLoading(false);
                }
            }
        };

        fetchOrders();
    }, [initialized, keycloak.token]);

    if (loading) return <div>Loading order history...</div>;
    if (error) return <div style={{ color: 'red' }}>Error: {error}</div>;

    return (
        <div>
            {orders.length === 0 ? (
                <p>No orders found.</p>
            ) : (                
                <Table striped bordered hover responsive>
                    <thead>
                        <tr>
                            <th>Order ID</th>
                            <th>Customer Name</th>
                            <th>Order Date</th>
                            <th>Status</th>
                            <th>Total Price</th>
                            <th>Actions</th>
                        </tr>
                    </thead>
                    <tbody>
                        {orders.map(order => {
                            const totalPrice = order.orderItems.reduce((sum, item) => sum + (item.unitPrice * item.quantity), 0);
                            return (
                                <tr key={order.id}>
                                    <td>{order.id}</td>
                                    <td>{order.customerName}</td>
                                    <td>{new Date(order.orderDate).toLocaleDateString()}</td>
                                    <td>{order.status}</td>
                                    <td>${totalPrice.toFixed(2)}</td>
                                    <td>
                                        <Link to={`/orders/${order.id}`}>
                                            <Button variant="info" size="sm">View Details</Button>
                                        </Link>
                                    </td>
                                </tr>
                            );
                        })}
                    </tbody>
                </Table>
            )}
        </div>
    );
};