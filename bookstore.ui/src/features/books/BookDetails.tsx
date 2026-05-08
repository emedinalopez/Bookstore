import React, { useEffect, useState } from 'react';
import { useParams, Link } from 'react-router-dom';
import { useKeycloak } from '@react-keycloak/web';
import { BookDto } from '../../models/book';
import { getBookById } from '../../api/inventoryApi';
import { useCart } from '../../context/CartContext';

export const BookDetails: React.FC = () => {    
    const { id } = useParams<{ id: string }>(); 
    const { keycloak, initialized } = useKeycloak();
    const { addToCart } = useCart();

    const [book, setBook] = useState<BookDto | null>(null);
    const [loading, setLoading] = useState<boolean>(false);
    const [error, setError] = useState<string | null>(null);
    const [quantity, setQuantity] = useState(1);

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

    const handleAddToCart = () => {
        if (book) {
            addToCart(book, quantity);
        }
    };

    if (loading) return <div>Loading book details...</div>;
    if (error) return <div style={{ color: 'red' }}>Error: {error}</div>;
    if (!book) return <div>Book not found.</div>;

    return (
        <div>
            <h2>{book.title}</h2>
            <p><strong>Author:</strong> {book.author}</p>
            <p><strong>Price:</strong> ${book.price.toFixed(2)}</p>
            <p><strong>Stock Available:</strong> {book.stockQty}</p>
            <div style={{ marginTop: '20px', display: 'flex', alignItems: 'center', gap: '10px' }}>
                <input
                    type="number"
                    value={quantity}
                    onChange={(e) => setQuantity(parseInt(e.target.value, 10) || 1)}
                    min="1"
                    max={book.stockQty}
                    style={{ width: '60px', padding: '5px' }}
                />
                <button onClick={handleAddToCart} disabled={book.stockQty === 0}>
                    Add to Cart
                </button>
            </div>

            <Link to="/books" style={{ display: 'block', marginTop: '20px' }}>Back to Catalog</Link>
        </div>
    );
};
