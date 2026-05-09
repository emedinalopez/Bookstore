import { BookDto } from '../models/book';
import { CategoryDto } from '../models/category';

const API_BASE_URL = 'http://localhost:5053/api'; 

export interface CreateBookCommand {
    title: string;
    author: string;
    price: number;
    stockQuantity: number;
    categoryId: number;
}

export interface UpdateBookCommand {
    id: number;
    title: string;
    author: string;
    price: number;
    stockQuantity: number;
    categoryId: number;
}

export interface CreateCategoryCommand {
    name: string;
}

export interface UpdateCategoryCommand {
    id: number;
    name: string;
}

export const getBooks = async (token: string): Promise<BookDto[]> => {
    const response = await fetch(`${API_BASE_URL}/books`, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',            
            'Authorization': `Bearer ${token}` 
        }
    });

    if (!response.ok) {
        throw new Error(`Failed to get the books: ${response.statusText}`);
    }

    return await response.json();
};

export const getBookById = async (id: number, token: string): Promise<BookDto> => {
    const response = await fetch(`${API_BASE_URL}/books/${id}`, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        }
    });

    if (!response.ok) {
        if (response.status === 404) {
            throw new Error(`Book with ID ${id} not found.`);
        }
        throw new Error(`Failed to fetch book: ${response.statusText}`);
    }

    return await response.json();
};

export const createBook = async (command: CreateBookCommand, token: string): Promise<BookDto> => {
    const response = await fetch(`${API_BASE_URL}/books`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        },
        body: JSON.stringify(command)
    });
    if (!response.ok) throw new Error('Failed to create book.');
    return await response.json();
};

export const updateBook = async (id: number, command: UpdateBookCommand, token: string): Promise<BookDto> => {
    const response = await fetch(`${API_BASE_URL}/books/${id}`, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        },
        body: JSON.stringify(command)
    });
    if (!response.ok){
        const error: any = new Error("Failed to update book");
        error.response = response;
        throw error;
    }
    return await response.json();
};

export const deleteBook = async (id: number, token: string): Promise<void> => {
    const response = await fetch(`${API_BASE_URL}/books/${id}`, {
        method: 'DELETE',
        headers: {
            'Authorization': `Bearer ${token}`
        }
    });
    if (!response.ok) throw new Error('Failed to delete book.');
};

export const getCategories = async (token: string): Promise<CategoryDto[]> => {
    const response = await fetch(`${API_BASE_URL}/categories`, {
        headers: { 'Authorization': `Bearer ${token}` }
    });
    if (!response.ok) throw new Error('Failed to fetch categories.');
    return await response.json();
};

export const createCategory = async (command: CreateCategoryCommand, token: string): Promise<CategoryDto> => {
    const response = await fetch(`${API_BASE_URL}/categories`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        },
        body: JSON.stringify(command)
    });
    if (!response.ok) throw new Error('Failed to create category.');
    return await response.json();
};

export const updateCategory = async (id: number, command: UpdateCategoryCommand, token: string): Promise<CategoryDto> => {
    const response = await fetch(`${API_BASE_URL}/categories/${id}`, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        },
        body: JSON.stringify(command)
    });
    if (!response.ok) throw new Error('Failed to update category.');
    return await response.json();
};

export const deleteCategory = async (id: number, token: string): Promise<void> => {
    const response = await fetch(`${API_BASE_URL}/categories/${id}`, {
        method: 'DELETE',
        headers: {
            'Authorization': `Bearer ${token}`
        }
    });
    if (!response.ok) {        
        if (response.status === 400) {
            const errorData = await response.json();
            throw new Error(errorData.error || 'Failed to delete category.');
        }
        throw new Error('Failed to delete category.');
    }
};