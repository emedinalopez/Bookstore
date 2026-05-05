import React, { useEffect, useState } from 'react';
import { BookDto } from '../../models/book';
import { getBooks } from '../../api/inventoryApi';

export const BookList: React.FC = () => {
    const [books, setBooks] = useState<BookDto[]>([]);
    const [loading, setLoading] = useState<boolean>(false);
    const [error, setError] = useState<string | null>(null);
    
    const TEMP_TOKEN = "eyJhbGciOiJSUzI1NiIsInR5cCIgOiAiSldUIiwia2lkIiA6ICJBV0p5SlZScTlyX1pmLTl6MHlYaFhyUUptcjA0OTYwQndxQW5vRF9LOEFvIn0.eyJleHAiOjE3Nzc5MDc3NzksImlhdCI6MTc3NzkwNzQ3OSwianRpIjoib25ydHJvOjNiMWEwMjNlLTA2YjgtNTVhZi1jY2IzLTAwMTgxMDk5MzUyZSIsImlzcyI6Imh0dHA6Ly9sb2NhbGhvc3Q6ODA4MC9yZWFsbXMvQm9va3N0b3JlUmVhbG0iLCJhdWQiOiJhY2NvdW50Iiwic3ViIjoiYjE3YWY0OGEtNTdkYi00MWNmLWE0MWMtNjQwZDJjZTBmOWM5IiwidHlwIjoiQmVhcmVyIiwiYXpwIjoiYm9va3N0b3JlLWFwaSIsInNpZCI6IlZyZlMtUktCZHhrWjdMbk12Qkw5NElGQyIsImFjciI6IjEiLCJhbGxvd2VkLW9yaWdpbnMiOlsiIl0sInJlYWxtX2FjY2VzcyI6eyJyb2xlcyI6WyJvZmZsaW5lX2FjY2VzcyIsInVtYV9hdXRob3JpemF0aW9uIiwiZGVmYXVsdC1yb2xlcy1ib29rc3RvcmVyZWFsbSJdfSwicmVzb3VyY2VfYWNjZXNzIjp7ImFjY291bnQiOnsicm9sZXMiOlsibWFuYWdlLWFjY291bnQiLCJtYW5hZ2UtYWNjb3VudC1saW5rcyIsInZpZXctcHJvZmlsZSJdfX0sInNjb3BlIjoicHJvZmlsZSBlbWFpbCIsImVtYWlsX3ZlcmlmaWVkIjp0cnVlLCJuYW1lIjoiVGVzdCBVc2VyIiwicHJlZmVycmVkX3VzZXJuYW1lIjoidGVzdHVzZXIiLCJnaXZlbl9uYW1lIjoiVGVzdCIsImZhbWlseV9uYW1lIjoiVXNlciIsImVtYWlsIjoidGVzdHVzZXJAdGVzdGluZy5jb20ifQ.Ryc7z7cesjacT5eoQnVxSRY2A25z5dXCAr2ZlhNyRLuANk3FuOuHPvdLEg_z5f1NlzYc9ujEdaKw62ZLXa1Qg1kR1MZmdEdEd7XYkJW_9mmjbM-D6yD5GVyCEJJrQeSjZQWztYTs8wPI0A2MNFvD7nF5Z1iksldBTGELM7Q13PJ4-Cw4Uz6M5iGQyNo2Y5YV5M8_gtP11Ria-IqPh83av0eqb3QxDz0pxyL6f55rpwyjBqyc8Jtnkk8dQTeVibTWcM-8gA3CB8BCZGn4bV2Fp5-pLCLpxGJRMzY6zMRJUXh3AXfvze7SJ-4zphJtsAgNeINq_VrQtcrIB8dHXGViRw";

    useEffect(() => {
        const fetchBooks = async () => {
            setLoading(true);
            try {
                const data = await getBooks(TEMP_TOKEN);
                setBooks(data);
            } catch (err: any) {
                setError(err.message);
            } finally {
                setLoading(false);
            }
        };

        fetchBooks();
    }, []);

    if (loading) return <div>Loading books...</div>;
    if (error) return <div style={{ color: 'red' }}>Error: {error}</div>;

    return (
        <div>
            <h2>Book Catalog</h2>
            {books.length === 0 ? (
                <p>No books available.</p>
            ) : (
                <table border={1} cellPadding={10} style={{ borderCollapse: 'collapse', width: '100%' }}>
                    <thead>
                        <tr>
                            <th>ID</th>
                            <th>Title</th>
                            <th>Author</th>
                            <th>Price</th>
                            <th>Stock</th>
                        </tr>
                    </thead>
                    <tbody>
                        {books.map(book => (
                            <tr key={book.id}>
                                <td>{book.id}</td>
                                <td>{book.title}</td>
                                <td>{book.author}</td>
                                <td>${book.price.toFixed(2)}</td>
                                <td>{book.stockQty}</td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            )}
        </div>
    );
};
