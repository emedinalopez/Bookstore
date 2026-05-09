import React, { useEffect, useState } from 'react';
import { useKeycloak } from '@react-keycloak/web';
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
        <table border={1} cellPadding={10} style={{ borderCollapse: 'collapse', width: '100%' }}>
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
                            <button onClick={() => onEdit(cat)}>Edit</button>
                            <button onClick={() => handleDelete(cat.id)} style={{ marginLeft: '5px' }}>Delete</button>
                        </td>
                    </tr>
                ))}
            </tbody>
        </table>
    );
};