namespace InventoryService.Domain.Entities
{
    public  class Book
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int StockQty { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
    }
}
