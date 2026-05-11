import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useCart } from '../../context/CartContext';
import { useKeycloak } from '@react-keycloak/web';
import { createOrder } from '../../api/orderApi';
import { CreateOrderCommand, OrderItemCommand } from '../../models/createOrderCommand';

export const PlaceOrderForm: React.FC = () => {
    const { cartItems, clearCart, removeFromCart } = useCart();
    const { keycloak } = useKeycloak();
    const navigate = useNavigate();

    const [customerName, setCustomerName] = useState('');
    const [isSubmitting, setIsSubmitting] = useState(false);
    const [error, setError] = useState<string | null>(null);

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        if (!keycloak.token || cartItems.length === 0) return;

        setIsSubmitting(true);
        setError(null);

        const orderItems: OrderItemCommand[] = cartItems.map(item => ({
            bookId: item.id,
            quantity: item.orderQuantity,
            unitPrice: item.price
        }));

        const command = { 
            customerName: customerName,
            items: cartItems.map(item => ({
                bookId: item.id,
                bookTitle: item.title,
                quantity: item.orderQuantity,
                unitPrice: item.price
            }))
        };

        try {
            await createOrder(command, keycloak.token);
            alert('Order placed successfully!');
            clearCart();
            navigate('/orders');
        } catch (err: any) {
            setError(err.message);
        } finally {
            setIsSubmitting(false);
        }
    };
    
    const totalPrice = cartItems.reduce((total, item) => total + item.price * item.orderQuantity, 0);

    return (
        <div>
            <h2>Checkout</h2>
            {cartItems.length === 0 ? (
                <p>Your cart is empty.</p>
            ) : (
                <>
                    <h3>Order Summary</h3>
                    <ul style={{ listStyle: 'none', padding: 0 }}>
                        {cartItems.map(item => (
                            <li key={item.id} style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '10px' }}>
                                <span>{item.title} - {item.orderQuantity} x ${item.price.toFixed(2)}</span>                                
                                <button onClick={() => removeFromCart(item.id)} style={{ padding: '2px 5px', backgroundColor: '#fdd' }}>
                                    Remove
                                </button>
                            </li>
                        ))}
                    </ul>
                    <h4>Total Price: ${totalPrice.toFixed(2)}</h4>
                    <hr />
                    <form onSubmit={handleSubmit}>
                        <div style={{ marginBottom: '10px' }}>
                            <label htmlFor="customerName">Your Name:</label>
                            <input
                                type="text"
                                id="customerName"
                                value={customerName}
                                onChange={(e) => setCustomerName(e.target.value)}
                                required
                                style={{ marginLeft: '10px' }}
                            />
                        </div>
                        <div style={{ display: 'flex', gap: '10px' }}>
                            <button type="submit" disabled={isSubmitting || !customerName}>
                                {isSubmitting ? 'Placing Order...' : 'Place Order'}
                            </button>                            
                            <button type="button" onClick={clearCart} style={{ backgroundColor: '#ffcccc' }}>
                                Clear Cart
                            </button>
                        </div>
                        {error && <p style={{ color: 'red' }}>Error: {error}</p>}
                    </form>
                </>
            )}
        </div>
    );
};