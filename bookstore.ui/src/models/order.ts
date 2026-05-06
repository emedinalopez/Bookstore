export enum OrderStatus {
    Pending = 0,
    Shipped = 1,
    Completed = 2,
    Cancelled = 3,
    OnHold = 4
}

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
    status: OrderStatus;
    orderItems: OrderItemDto[];
}
