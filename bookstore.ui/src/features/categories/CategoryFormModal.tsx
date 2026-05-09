import React, { useState, useEffect } from 'react';
import { CategoryDto } from '../../models/category';

interface CategoryFormModalProps {
    isOpen: boolean;
    onClose: () => void;
    onSubmit: (formData: { name: string }) => void;
    initialData?: CategoryDto | null;
}

export const CategoryFormModal: React.FC<CategoryFormModalProps> = ({ isOpen, onClose, onSubmit, initialData }) => {
    const [name, setName] = useState('');

    useEffect(() => {
        setName(initialData ? initialData.name : '');
    }, [initialData, isOpen]);

    if (!isOpen) return null;

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        onSubmit({ name });
    };

    return (
        <div style={modalOverlayStyle}>
            <div style={modalContentStyle}>
                <h2>{initialData ? 'Edit Category' : 'Create New Category'}</h2>
                <form onSubmit={handleSubmit}>
                    <div style={formGroupStyle}>
                        <label>Category Name</label>
                        <input type="text" value={name} onChange={(e) => setName(e.target.value)} required />
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

const modalOverlayStyle: React.CSSProperties = { position: 'fixed', top: 0, left: 0, right: 0, bottom: 0, backgroundColor: 'rgba(0, 0, 0, 0.7)', display: 'flex', justifyContent: 'center', alignItems: 'center' };
const modalContentStyle: React.CSSProperties = { background: 'white', padding: '20px', borderRadius: '5px', width: '400px' };
const formGroupStyle: React.CSSProperties = { display: 'flex', flexDirection: 'column', marginBottom: '10px' };