import { OrderDto } from '../models/order';
import { CreateOrderCommand } from '../models/createOrderCommand';

const API_BASE_URL = 'https://localhost:5147/api';

export const getOrders = async (token: string): Promise<OrderDto[]> => {
    const response = await fetch(`${API_BASE_URL}/orders`, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        }
    });

    if (!response.ok) {
        throw new Error('Failed to fetch orders.');
    }
    return await response.json();
};

export const createOrder = async (command: CreateOrderCommand, token: string): Promise<OrderDto> => {
    const response = await fetch(`${API_BASE_URL}/orders`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        },
        body: JSON.stringify(command)
    });

    if (!response.ok) {
        // Try to get more detailed error info from the response body
        const errorData = await response.json().catch(() => null);
        const errorMessage = errorData?.error || 'Failed to create order.';
        throw new Error(errorMessage);
    }
    return await response.json();
};

export const getOrderById = async (id: number, token: string): Promise<OrderDto> => {
    const response = await fetch(`${API_BASE_URL}/orders/${id}`, {
        method: 'GET',
        headers: {
            'Authorization': `Bearer ${token}`
        }
    });

    if (!response.ok) {
        throw new Error(`Failed to fetch order with ID ${id}.`);
    }
    return await response.json();
};