export interface OrderItemCommand {
    bookId: number;
    quantity: number;
    unitPrice: number;
}

export interface CreateOrderCommand {
    customerName: string;
    orderItems: OrderItemCommand[];
}