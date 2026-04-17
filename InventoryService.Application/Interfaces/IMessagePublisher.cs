namespace InventoryService.Application.Interfaces
{
    public interface IMessagePublisher
    {
        Task PublishAsync(object message);
    }
}
