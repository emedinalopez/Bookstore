import React, { useEffect, useState } from 'react';
import { useKeycloak } from '@react-keycloak/web';
import { Table, Button } from 'react-bootstrap';
import { CategoryDto } from '../../models/category';
import { getCategories, deleteCategory } from '../../api/inventoryApi';

interface CategoryListProps {
    refreshTrigger: boolean;
    onEdit: (category: CategoryDto) => void;
}

export const CategoryList: React.FC<CategoryListProps> = ({ refreshTrigger, onEdit }) => {
    const { keycloak, initialized } = useKeycloak();
    const [categories, setCategories] = useState<CategoryDto[]>([]);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);

    const fetchCategories = async () => {
        if (initialized && keycloak.token) {
            setLoading(true);
            try {
                const data = await getCategories(keycloak.token);
                setCategories(data);
            } catch (err: any) {
                setError(err.message);
            } finally {
                setLoading(false);
            }
        }
    };

    useEffect(() => {
        fetchCategories();
    }, [initialized, keycloak.token, refreshTrigger]);

    const handleDelete = async (id: number) => {
        if (!keycloak.token || !window.confirm("Are you sure? Deleting a category will fail if it's assigned to any books.")) return;
        try {
            await deleteCategory(id, keycloak.token);
            fetchCategories();
        } catch (err: any) {
            alert(`Error: ${err.message}`);
        }
    };

    if (loading) return <div>Loading categories...</div>;
    if (error) return <div style={{ color: 'red' }}>Error: {error}</div>;

    return (
        <Table striped bordered hover responsive>
            <thead>
                <tr>
                    <th>ID</th>
                    <th>Name</th>
                    <th>Actions</th>
                </tr>
            </thead>
            <tbody>
                {categories.map(cat => (
                    <tr key={cat.id}>
                        <td>{cat.id}</td>
                        <td>{cat.name}</td>
                        <td>                            
                            <Button variant="primary" size="sm" onClick={() => onEdit(cat)}>
                                Edit
                            </Button>
                            <Button variant="danger" size="sm" onClick={() => handleDelete(cat.id)} className="ms-2">
                                Delete
                            </Button>
                        </td>
                    </tr>
                ))}
            </tbody>
        </Table>
    );
};