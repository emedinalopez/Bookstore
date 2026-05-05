import { BookDto } from '../models/book';

const API_BASE_URL = 'https://localhost:5053/api'; 

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
