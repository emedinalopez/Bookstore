import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import { BookDto } from '../../models/book';
import { getBooks } from '../../api/inventoryApi';
import { useKeycloak } from '@react-keycloak/web';

export const BookList: React.FC = () => {
    const { keycloak, initialized } = useKeycloak();
    const [books, setBooks] = useState<BookDto[]>([]);
    const [loading, setLoading] = useState<boolean>(false);
    const [error, setError] = useState<string | null>(null);
        

    useEffect(() => {
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

        fetchBooks();
    }, [initialized, keycloak.token]);

    if (!initialized) return <div>Initializing authentication...</div>;

    if (loading) return <div>Loading books...</div>;
    if (error) return <div style={{ color: 'red' }}>Error: {error}</div>;

    return (
        <div>
            <h2>Book Catalog</h2>
            {books.length === 0 ? (
                <p>No books available.</p>
            ) : (
                <table border={1} cellPadding={10} style={{ borderCollapse: 'collapse', width: '100%' }}>
                    <thead>
                        <tr>
                            <th>ID</th>
                            <th>Title</th>
                            <th>Author</th>
                            <th>Price</th>
                            <th>Stock</th>
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
                            </tr>
                        ))}
                    </tbody>
                </table>
            )}
        </div>
    );
};
