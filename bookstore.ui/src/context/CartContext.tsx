import React, { createContext, useState, useContext, ReactNode } from 'react';
import { BookDto } from '../models/book';

interface CartItem extends BookDto {
    orderQuantity: number;
}

interface CartContextType {
    cartItems: CartItem[];
    addToCart: (book: BookDto, quantity: number) => void;
    removeFromCart: (bookId: number) => void;
    clearCart: () => void;
}

const CartContext = createContext<CartContextType | undefined>(undefined);

export const useCart = () => {
    const context = useContext(CartContext);
    if (!context) {
        throw new Error('useCart must be used within a CartProvider');
    }
    return context;
};

export const CartProvider: React.FC<{ children: ReactNode }> = ({ children }) => {
    const [cartItems, setCartItems] = useState<CartItem[]>([]);

    const addToCart = (book: BookDto, quantity: number) => {
        setCartItems(prevItems => {
            const existingItem = prevItems.find(item => item.id === book.id);
            if (existingItem) {                
                return prevItems.map(item =>
                    item.id === book.id
                        ? { ...item, orderQuantity: item.orderQuantity + quantity }
                        : item
                );
            }
            
            return [...prevItems, { ...book, orderQuantity: quantity }];
        });
        alert(`${quantity} x "${book.title}" added to cart!`);
    };

    const removeFromCart = (bookId: number) => {
        setCartItems(prevItems => prevItems.filter(item => item.id !== bookId));
    };

    const clearCart = () => {
        setCartItems([]);
    };

    return (
        <CartContext.Provider value={{ cartItems, addToCart, removeFromCart, clearCart }}>
            {children}
        </CartContext.Provider>
    );
};