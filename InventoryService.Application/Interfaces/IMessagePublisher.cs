namespace InventoryService.Application.Interfaces
{
    public interface IMessagePublisher
    {
        void Publish(object message);
    }
}
