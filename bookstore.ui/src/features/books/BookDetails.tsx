import React, { useEffect, useState } from 'react';
import { useParams, Link } from 'react-router-dom';
import { useKeycloak } from '@react-keycloak/web';
import { Card, Button, Form, Spinner, Alert } from 'react-bootstrap';
import { BookDto } from '../../models/book';
import { CategoryDto } from '../../models/category';
import { getBookById, getCategories } from '../../api/inventoryApi';
import { useCart } from '../../context/CartContext';

export const BookDetails: React.FC = () => {
    const { id } = useParams<{ id: string }>();
    const { keycloak, initialized } = useKeycloak();
    const { addToCart } = useCart();

    const [book, setBook] = useState<BookDto | null>(null);
    const [categories, setCategories] = useState<CategoryDto[]>([]);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);
    const [quantity, setQuantity] = useState(1);

    useEffect(() => {
        const fetchData = async () => {
            if (initialized && keycloak.token && id) {
                setLoading(true);
                try {
                    // Fetch BOTH the book and the categories at the same time
                    const [bookData, categoriesData] = await Promise.all([
                        getBookById(Number(id), keycloak.token),
                        getCategories(keycloak.token)
                    ]);
                    setBook(bookData);
                    setCategories(categoriesData);
                } catch (err: any) {
                    setError(err.message);
                } finally {
                    setLoading(false);
                }
            }
        };

        fetchData();
    }, [initialized, keycloak.token, id]);

    const handleAddToCart = () => {
        if (book) {
            addToCart(book, quantity);
        }
    };

    if (loading) return <Spinner animation="border" />;
    if (error) return <Alert variant="danger">Error: {error}</Alert>;
    if (!book) return <Alert variant="warning">Book not found.</Alert>;
    
    const categoryName = categories.find(c => c.id === book.categoryId)?.name || 'Unknown Category';

    return (
        <Card>
            <Card.Header as="h3">{book.title}</Card.Header>
            <Card.Body>
                <Card.Text><strong>Author:</strong> {book.author}</Card.Text>                                
                <Card.Text><strong>Category:</strong> {categoryName}</Card.Text>                
                <Card.Text><strong>Price:</strong> ${book.price.toFixed(2)}</Card.Text>
                <Card.Text><strong>Stock Available:</strong> {book.stockQty}</Card.Text>
                
                <div className="d-flex align-items-center gap-2 mt-4">
                    <Form.Control
                        type="number"
                        value={quantity}
                        onChange={(e) => setQuantity(parseInt(e.target.value, 10) || 1)}
                        min="1"
                        max={book.stockQty}
                        style={{ width: '80px' }}
                    />
                    <Button 
                        variant="primary" 
                        onClick={handleAddToCart} 
                        disabled={book.stockQty === 0}
                    >
                        {book.stockQty === 0 ? 'Out of Stock' : 'Add to Cart'}
                    </Button>
                </div>
            </Card.Body>
            <Card.Footer>
                <Link to="/books">
                    <Button variant="secondary">Back to Catalog</Button>
                </Link>
            </Card.Footer>
        </Card>
    );
};