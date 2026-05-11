export interface OrderItemDto {
    id: number;
    bookId: number;
    quantity: number;
    unitPrice: number;
}

export interface OrderDto {
    id: number;
    customerName: string;
    orderDate: string;
    status: string;
    orderItems: OrderItemDto[];
}
