import { OrderStatus } from "./order";

export interface UpdateOrderCommand {
    id: number;
    customerName: string;
    status: OrderStatus;
}
