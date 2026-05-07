import React, { useState, useEffect } from 'react';
import { BookDto } from '../../models/book';

interface BookFormModalProps {
    isOpen: boolean;
    onClose: () => void;
    onSubmit: (formData: any) => void;
    initialData?: BookDto | null;
}

export const BookFormModal: React.FC<BookFormModalProps> = ({ isOpen, onClose, onSubmit, initialData }) => {
    const [formData, setFormData] = useState({
        title: '',
        author: '',
        price: 0,
        stockQuantity: 0,
        categoryId: 1 
    });
    
    useEffect(() => {
        if (initialData) {
            setFormData({
                title: initialData.title,
                author: initialData.author,
                price: initialData.price,
                stockQuantity: initialData.stockQty,
                categoryId: initialData.categoryId
            });
        } else {            
            setFormData({ title: '', author: '', price: 0, stockQuantity: 0, categoryId: 1 });
        }
    }, [initialData, isOpen]);

    if (!isOpen) return null;

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const { name, value, type } = e.target;
        setFormData(prev => ({
            ...prev,
            [name]: type === 'number' ? parseFloat(value) || 0 : value
        }));
    };

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        onSubmit(formData);
    };

    return (
        <div style={modalOverlayStyle}>
            <div style={modalContentStyle}>
                <h2>{initialData ? 'Edit Book' : 'Create New Book'}</h2>
                <form onSubmit={handleSubmit}>
                    <div style={formGroupStyle}>
                        <label>Title</label>
                        <input type="text" name="title" value={formData.title} onChange={handleChange} required />
                    </div>
                    <div style={formGroupStyle}>
                        <label>Author</label>
                        <input type="text" name="author" value={formData.author} onChange={handleChange} required />
                    </div>
                    <div style={formGroupStyle}>
                        <label>Price</label>
                        <input type="number" name="price" value={formData.price} onChange={handleChange} step="0.01" required />
                    </div>
                    <div style={formGroupStyle}>
                        <label>Stock Quantity</label>
                        <input type="number" name="stockQuantity" value={formData.stockQuantity} onChange={handleChange} required />
                    </div>                    
                    <div style={formGroupStyle}>
                        <label>Category ID</label>
                        <input type="number" name="categoryId" value={formData.categoryId} onChange={handleChange} required />
                    </div>
                    <div style={{ marginTop: '20px', display: 'flex', justifyContent: 'flex-end', gap: '10px' }}>
                        <button type="button" onClick={onClose}>Cancel</button>
                        <button type="submit">{initialData ? 'Update' : 'Create'}</button>
                    </div>
                </form>
            </div>
        </div>
    );
};

const modalOverlayStyle: React.CSSProperties = {
    position: 'fixed', top: 0, left: 0, right: 0, bottom: 0,
    backgroundColor: 'rgba(0, 0, 0, 0.7)', display: 'flex',
    justifyContent: 'center', alignItems: 'center'
};
const modalContentStyle: React.CSSProperties = {
    background: 'white', padding: '20px', borderRadius: '5px', width: '400px'
};
const formGroupStyle: React.CSSProperties = {
    display: 'flex', flexDirection: 'column', marginBottom: '10px'
};
