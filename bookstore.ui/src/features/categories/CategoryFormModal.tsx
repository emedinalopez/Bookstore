import React, { useState, useEffect } from 'react';
import { Modal, Button, Form } from 'react-bootstrap';
import { CategoryDto } from '../../models/category';

interface CategoryFormModalProps {
    show: boolean;
    onHide: () => void;
    onSubmit: (formData: { name: string }) => void;
    initialData?: CategoryDto | null;
}

export const CategoryFormModal: React.FC<CategoryFormModalProps> = ({ show, onHide, onSubmit, initialData }) => {
    const [name, setName] = useState('');

    useEffect(() => {
        setName(initialData ? initialData.name : '');
    }, [initialData, show]);

    if (!show) return null;

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        onSubmit({ name });
    };

    return (
        <Modal show={show} onHide={onHide}>
            <Modal.Header closeButton>
                <Modal.Title>{initialData ? 'Edit Category' : 'Create New Category'}</Modal.Title>
            </Modal.Header>
            <Modal.Body>
                <Form onSubmit={handleSubmit}>
                    <Form.Group className="mb-3" controlId="formCategoryName">
                        <Form.Label>Category Name</Form.Label>
                        <Form.Control
                            type="text"
                            value={name}
                            onChange={(e) => setName(e.target.value)}
                            required
                            autoFocus
                        />
                    </Form.Group>
                </Form>
            </Modal.Body>
            <Modal.Footer>
                <Button variant="secondary" onClick={onHide}>
                    Cancel
                </Button>
                <Button variant="primary" onClick={handleSubmit}>
                    {initialData ? 'Update Category' : 'Create Category'}
                </Button>
            </Modal.Footer>
        </Modal>
    );
};

const modalOverlayStyle: React.CSSProperties = { position: 'fixed', top: 0, left: 0, right: 0, bottom: 0, backgroundColor: 'rgba(0, 0, 0, 0.7)', display: 'flex', justifyContent: 'center', alignItems: 'center' };
const modalContentStyle: React.CSSProperties = { background: 'white', padding: '20px', borderRadius: '5px', width: '400px' };
const formGroupStyle: React.CSSProperties = { display: 'flex', flexDirection: 'column', marginBottom: '10px' };