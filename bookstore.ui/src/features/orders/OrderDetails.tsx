import React, { useEffect, useState } from 'react';
import { useParams, Link } from 'react-router-dom';
import { useKeycloak } from '@react-keycloak/web';
import { getOrderById } from '../../api/orderApi';
import { OrderDto, OrderStatus } from '../../models/order';

const getStatusString = (status: OrderStatus): string => {    
    return "Status";
};

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

    if (loading) return <div>Loading order details...</div>;
    if (error) return <div style={{ color: 'red' }}>Error: {error}</div>;
    if (!order) return <div>Order not found.</div>;
    
    const totalPrice = order.orderItems.reduce((sum, item) => sum + (item.unitPrice * item.quantity), 0);

    return (
        <div>
            <h2>Order Details - ID: {order.id}</h2>
            <p><strong>Customer:</strong> {order.customerName}</p>
            <p><strong>Order Date:</strong> {new Date(order.orderDate).toLocaleString()}</p>
            <p><strong>Status:</strong> {getStatusString(order.status)}</p>
            <hr />
            <h3>Items</h3>
            <ul>
                {order.orderItems.map(item => (
                    <li key={item.id}>
                        Book ID: {item.bookId} - {item.quantity} x ${item.unitPrice.toFixed(2)}
                    </li>
                ))}
            </ul>
            <h3>Total: ${totalPrice.toFixed(2)}</h3>
            <Link to="/orders" style={{ display: 'block', marginTop: '20px' }}>Back to Order History</Link>
        </div>
    );
};