namespace EventBus.Messages
{
    public class BookStockChangedIntegrationEvent
    {
        public int BookId { get; set; }
        public int NewStockQuantity { get; set; }
    }
}
