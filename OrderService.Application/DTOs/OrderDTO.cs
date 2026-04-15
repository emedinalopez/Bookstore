namespace OrderService.Application.DTOs
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }        
        public string Status { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public List<OrderItemDTO> OrderItems { get; set; } = [];
    }
}
