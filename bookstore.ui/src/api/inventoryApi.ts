import { BookDto } from '../models/book';

const API_BASE_URL = 'http://localhost:5053/api'; 

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
