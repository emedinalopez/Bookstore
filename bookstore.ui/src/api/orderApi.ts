import { OrderDto, OrderStatus } from '../models/order';
import { CreateOrderCommand } from '../models/createOrderCommand';
import { UpdateOrderCommand } from '../models/updateOrderCommand';

const API_BASE_URL = 'http://localhost:5147/api';

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

export const updateOrder = async (id: number, command: UpdateOrderCommand, token: string): Promise<void> => {
    const response = await fetch(`${API_BASE_URL}/orders/${id}`, { 
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        },
        body: JSON.stringify(command)
    });

    if (!response.ok) {        
        const errorData = await response.json().catch(() => null);
        const errorMessage = errorData?.error || errorData?.title || 'Failed to update order.';
        throw new Error(errorMessage);
    }
};

export const deleteOrder = async (id: number, token: string): Promise<void> => {
    const response = await fetch(`${API_BASE_URL}/orders/${id}`, {
        method: 'DELETE',
        headers: {
            'Authorization': `Bearer ${token}`
        }
    });

    if (!response.ok) {
        throw new Error('Failed to delete order.');
    }
};