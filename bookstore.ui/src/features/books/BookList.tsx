import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import { useKeycloak } from '@react-keycloak/web';
import { BookDto } from '../../models/book';
import { getBooks, deleteBook, updateBook, UpdateBookCommand } from '../../api/inventoryApi';
import { BookFormModal } from './BookFormModal';

interface BookListProps {
    refreshTrigger: boolean; 
}

export const BookList: React.FC<BookListProps> = ({ refreshTrigger }) => {
    const { keycloak, initialized } = useKeycloak();
    const [books, setBooks] = useState<BookDto[]>([]);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);
    
    const [isUpdateModalOpen, setIsUpdateModalOpen] = useState(false);
    const [selectedBook, setSelectedBook] = useState<BookDto | null>(null);

    const fetchBooks = async () => {
        if (initialized && keycloak.token) {
            setLoading(true);
            try {
                const data = await getBooks(keycloak.token);
                setBooks(data);
            } catch (err: any) {
                setError(err.message);
            } finally {
                setLoading(false);
            }
        }
    };
    
    useEffect(() => {
        fetchBooks();
    }, [initialized, keycloak.token, refreshTrigger]);

    const handleDelete = async (id: number) => {
        if (!keycloak.token || !window.confirm("Are you sure you want to delete this book?")) return;
        try {
            await deleteBook(id, keycloak.token);
            fetchBooks();
        } catch (error) {
            console.error("Failed to delete book:", error);
            alert("Error deleting book.");
        }
    };

    const handleUpdate = (book: BookDto) => {
        setSelectedBook(book);
        setIsUpdateModalOpen(true);
    };

    const handleUpdateSubmit = async (formData: Omit<UpdateBookCommand, 'id'>) => {
        if (!keycloak.token || !selectedBook) return;
        try {
            const command: UpdateBookCommand = { ...formData, id: selectedBook.id };
            await updateBook(selectedBook.id, command, keycloak.token);
            setIsUpdateModalOpen(false);
            fetchBooks();
        } catch (error: any) {
            console.error("Failed to update book:", error);
            const serverError = await error.response?.json();
            const errorMessage = serverError?.error || error.message;
            alert(`Error updating book: ${errorMessage}`);
        }
    };

    if (loading) return <div>Loading books...</div>;
    if (error) return <div style={{ color: 'red' }}>Error: {error}</div>;

    return (
        <>
            <table border={1} cellPadding={10} style={{ borderCollapse: 'collapse', width: '100%' }}>
                <thead>
                    <tr>
                        <th>ID</th>
                        <th>Title</th>
                        <th>Author</th>
                        <th>Price</th>
                        <th>Stock</th>
                        <th>Actions</th>
                    </tr>
                </thead>
                <tbody>
                    {books.map(book => (
                        <tr key={book.id}>
                            <td>{book.id}</td>
                            <td><Link to={`/books/${book.id}`}>{book.title}</Link></td>
                            <td>{book.author}</td>
                            <td>${book.price.toFixed(2)}</td>
                            <td>{book.stockQty}</td>
                            <td>
                                <button onClick={() => handleUpdate(book)}>Edit</button>
                                <button onClick={() => handleDelete(book.id)} style={{ marginLeft: '5px' }}>Delete</button>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>
            
            <BookFormModal
                isOpen={isUpdateModalOpen}
                onClose={() => setIsUpdateModalOpen(false)}
                onSubmit={handleUpdateSubmit}
                initialData={selectedBook}
            />
        </>
    );
};