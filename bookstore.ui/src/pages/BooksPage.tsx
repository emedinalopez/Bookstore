import React, { useState } from 'react';
import { useKeycloak } from '@react-keycloak/web';
import { BookList } from '../features/books/BookList';
import { BookFormModal } from '../features/books/BookFormModal';
import { createBook, CreateBookCommand } from '../api/inventoryApi';

export const BooksPage: React.FC = () => {
    const [isModalOpen, setIsModalOpen] = useState(false);
    const { keycloak } = useKeycloak();
    const [refreshList, setRefreshList] = useState(false);

    const handleCreateBook = async (formData: CreateBookCommand) => {
        if (!keycloak.token) return;
        try {
            await createBook(formData, keycloak.token);
            setIsModalOpen(false);
            setRefreshList(prev => !prev);
        } catch (error) {
            console.error("Failed to create book:", error);
            alert("Error creating book.");
        }
    };

    return (
        <div>
            <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '20px' }}>
                <h2>Inventory Management</h2>
                <button onClick={() => setIsModalOpen(true)}>+ Add New Book</button>
            </div>
            
            <BookList refreshTrigger={refreshList} />

            <BookFormModal
                isOpen={isModalOpen}
                onClose={() => setIsModalOpen(false)}
                onSubmit={handleCreateBook}
            />
        </div>
    );
};
