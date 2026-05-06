import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import { useKeycloak } from '@react-keycloak/web';
import { BookDto } from '../../models/book';
import { getBookById } from '../../api/inventoryApi';

export const BookDetails: React.FC = () => {    
    const { id } = useParams<{ id: string }>(); 
    const { keycloak, initialized } = useKeycloak();

    const [book, setBook] = useState<BookDto | null>(null);
    const [loading, setLoading] = useState<boolean>(false);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        const fetchBook = async () => {
            if (initialized && keycloak.token && id) {
                setLoading(true);
                try {
                    const data = await getBookById(Number(id), keycloak.token);
                    setBook(data);
                } catch (err: any) {
                    setError(err.message);
                } finally {
                    setLoading(false);
                }
            }
        };

        fetchBook();
    }, [initialized, keycloak.token, id]);

    if (loading) return <div>Loading book details...</div>;
    if (error) return <div style={{ color: 'red' }}>Error: {error}</div>;
    if (!book) return <div>Book not found.</div>;

    return (
        <div>
            <h2>{book.title}</h2>
            <p><strong>Author:</strong> {book.author}</p>
            <p><strong>Price:</strong> ${book.price.toFixed(2)}</p>
            <p><strong>Stock Available:</strong> {book.stockQty}</p>
            {/* TODO: Add a button to add to cart or something, so that we can have orders */}
        </div>
    );
};
