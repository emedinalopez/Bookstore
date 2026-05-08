import React from 'react';
import { OrderHistory } from '../features/orders/OrderHistory';

export const OrdersPage: React.FC = () => {
    return (
        <div>
            <h2>Order Management</h2>
            <p>View past orders and their statuses.</p>
            <div style={{marginTop: '20px'}}>
                <OrderHistory />
            </div>
        </div>
    );
};