import React, { useState, useEffect } from 'react';
import { useKeycloak } from '@react-keycloak/web';
import { Modal, Button, Form } from 'react-bootstrap';
import { BookDto } from '../../models/book';
import { CategoryDto } from '../../models/category';
import { getCategories } from '../../api/inventoryApi';

interface BookFormModalProps {
    show: boolean;
    onHide: () => void;
    onSubmit: (formData: any) => void;
    initialData?: BookDto | null;
}

export const BookFormModal: React.FC<BookFormModalProps> = ({ show, onHide, onSubmit, initialData }) => {
    const { keycloak, initialized } = useKeycloak();
    const [categories, setCategories] = useState<CategoryDto[]>([]);

    const [formData, setFormData] = useState({
        title: '',
        author: '',
        price: 0 as number | string,
        stockQty: 0 as number | string,
        categoryId: 1 as number | string
    });
    
    useEffect(() => {
        const fetchCategories = async () => {
            if (show && initialized && keycloak.token) {
                try {
                    const data = await getCategories(keycloak.token);
                    setCategories(data);
                    
                    if (!initialData && data.length > 0) {
                        setFormData(prev => ({ ...prev, categoryId: data[0].id }));
                    }
                } catch (error) {
                    console.error("Failed to load categories for dropdown", error);
                }
            }
        };
        fetchCategories();
    }, [show, initialized, keycloak.token, initialData]);
    
    useEffect(() => {
        if (initialData) {
            setFormData({
                title: initialData.title,
                author: initialData.author,
                price: initialData.price,
                stockQty: initialData.stockQty,
                categoryId: initialData.categoryId
            });
        } else {
            setFormData(prev => ({ ...prev, title: '', author: '', price: 0, stockQty: 0 }));
        }
    }, [initialData, show]);

    if (!show) return null;
    
    const handleChange = (e: any) => {
        const { name, value } = e.target;

        const isNumericField = name === 'price' || name === 'stockQty' || name === 'categoryId';
                
        const processedValue = isNumericField && value !== '' ? Number(value) : value;

        setFormData(prev => ({
            ...prev,
            [name]: processedValue,
        }));
    };

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();        
        
        const finalFormData = {
            ...formData,
            price: Number(formData.price) || 0,
            stockQty: Number(formData.stockQty) || 0,
            categoryId: Number(formData.categoryId) || 0,
        };
        
        onSubmit(finalFormData);
    };

    return (
        <Modal show={show} onHide={onHide}>
            <Modal.Header closeButton>
                <Modal.Title>{initialData ? 'Edit Book' : 'Create New Book'}</Modal.Title>
            </Modal.Header>
                        
            <Form onSubmit={handleSubmit}>
                <Modal.Body>
                    <Form.Group className="mb-3" controlId="formBookTitle">
                        <Form.Label>Title</Form.Label>
                        <Form.Control type="text" name="title" value={formData.title} onChange={handleChange} required autoFocus />
                    </Form.Group>

                    <Form.Group className="mb-3" controlId="formBookAuthor">
                        <Form.Label>Author</Form.Label>
                        <Form.Control type="text" name="author" value={formData.author} onChange={handleChange} required />
                    </Form.Group>
                    
                    <Form.Group className="mb-3" controlId="formBookPrice">
                        <Form.Label>Price</Form.Label>
                        <Form.Control type="number" name="price" value={formData.price} onChange={handleChange} step="0.01" min="0" required />
                    </Form.Group>

                    <Form.Group className="mb-3" controlId="formBookStock">
                        <Form.Label>Stock Quantity</Form.Label>
                        <Form.Control type="number" name="stockQty" value={formData.stockQty} onChange={handleChange} min="0" required />
                    </Form.Group>

                    <Form.Group className="mb-3" controlId="formBookCategory">
                        <Form.Label>Category</Form.Label>
                        <Form.Select name="categoryId" value={formData.categoryId} onChange={handleChange} required>
                            {categories.length === 0 && <option disabled value="">Loading...</option>}
                            {categories.map(cat => (
                                <option key={cat.id} value={cat.id}>{cat.name}</option>
                            ))}
                        </Form.Select>
                    </Form.Group>
                </Modal.Body>

                <Modal.Footer>
                    <Button variant="secondary" onClick={onHide}>
                        Cancel
                    </Button>                    
                    <Button variant="primary" type="submit">
                        {initialData ? 'Update Book' : 'Create Book'}
                    </Button>
                </Modal.Footer>
            </Form>
        </Modal>
    );
};