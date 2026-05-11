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
        price: 0,
        stockQty: 0,
        categoryId: 1
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
            setFormData({ title: '', author: '', price: 0, stockQty: 0, categoryId: 1 });
        }
    }, [initialData, show]);

    if (!show) return null;

    const handleChange = (e: any) => {
        const { name, value } = e.target;    
        let processedValue: string | number = value;
    
        if (name === 'price') {            
            processedValue = parseFloat(value) || 0;
        } else if (name === 'stockQty' || name === 'categoryId') {            
            processedValue = parseInt(value, 10);            
            if (isNaN(processedValue)) {
                processedValue = 0;
            }
        }
        
        setFormData(prev => ({
            ...prev,
            [name]: processedValue,
        }));
    };

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        onSubmit(formData);
    };

    return (
        <Modal show={show} onHide={onHide}>
            <Modal.Header closeButton>
                <Modal.Title>{initialData ? 'Edit Book' : 'Create New Book'}</Modal.Title>
            </Modal.Header>
            <Modal.Body>
                <Form onSubmit={handleSubmit}>
                    <Form.Group className="mb-3" controlId="formBookTitle">
                        <Form.Label>Title</Form.Label>
                        <Form.Control type="text" name="title" value={formData.title} onChange={handleChange} required />
                    </Form.Group>

                    <Form.Group className="mb-3" controlId="formBookAuthor">
                        <Form.Label>Author</Form.Label>
                        <Form.Control type="text" name="author" value={formData.author} onChange={handleChange} required />
                    </Form.Group>
                    
                    <Form.Group className="mb-3" controlId="formBookPrice">
                        <Form.Label>Price</Form.Label>
                        <Form.Control type="number" name="price" value={formData.price} onChange={handleChange} step="0.01" required />
                    </Form.Group>

                    <Form.Group className="mb-3" controlId="formBookStock">
                        <Form.Label>Stock Quantity</Form.Label>
                        <Form.Control type="number" name="stockQuantity" value={formData.stockQty} onChange={handleChange} required />
                    </Form.Group>

                    <Form.Group className="mb-3" controlId="formBookCategory">
                        <Form.Label>Category</Form.Label>
                        <Form.Select name="categoryId" value={formData.categoryId} onChange={handleChange} required>
                            {categories.length === 0 && <option disabled>Loading...</option>}
                            {categories.map(cat => (
                                <option key={cat.id} value={cat.id}>{cat.name}</option>
                            ))}
                        </Form.Select>
                    </Form.Group>
                </Form>
            </Modal.Body>
            <Modal.Footer>
                <Button variant="secondary" onClick={onHide}>
                    Cancel
                </Button>
                <Button variant="primary" onClick={handleSubmit}>
                    {initialData ? 'Update Book' : 'Create Book'}
                </Button>
            </Modal.Footer>
        </Modal>
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
