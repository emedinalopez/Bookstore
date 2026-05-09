import React, { useState } from 'react';
import { useKeycloak } from '@react-keycloak/web';
import { CategoryDto } from '../models/category';
import { createCategory, updateCategory, UpdateCategoryCommand } from '../api/inventoryApi';
import { CategoryList } from '../features/categories/CategoryList';
import { CategoryFormModal } from '../features/categories/CategoryFormModal';

export const CategoriesPage: React.FC = () => {
    const { keycloak } = useKeycloak();
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [editingCategory, setEditingCategory] = useState<CategoryDto | null>(null);
    const [refreshTrigger, setRefreshTrigger] = useState(false);

    const handleOpenCreate = () => {
        setEditingCategory(null);
        setIsModalOpen(true);
    };

    const handleOpenEdit = (category: CategoryDto) => {
        setEditingCategory(category);
        setIsModalOpen(true);
    };

    const handleSubmit = async (formData: { name: string }) => {
        if (!keycloak.token) return;

        try {
            if (editingCategory) {                
                const command: UpdateCategoryCommand = { id: editingCategory.id, ...formData };
                await updateCategory(editingCategory.id, command, keycloak.token);
            } else {                
                await createCategory(formData, keycloak.token);
            }
            setRefreshTrigger(prev => !prev);
            setIsModalOpen(false);
        } catch (err: any) {
            alert(`Error: ${err.message}`);
        }
    };

    return (
        <div>
            <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '20px' }}>
                <h2>Category Management</h2>
                <button onClick={handleOpenCreate}>+ Add New Category</button>
            </div>
            
            <CategoryList refreshTrigger={refreshTrigger} onEdit={handleOpenEdit} />

            <CategoryFormModal
                isOpen={isModalOpen}
                onClose={() => setIsModalOpen(false)}
                onSubmit={handleSubmit}
                initialData={editingCategory}
            />
        </div>
    );
};