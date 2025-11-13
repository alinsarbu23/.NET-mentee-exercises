namespace Cafe.Domain.Events
{
    public interface IOrderEventPublisher
    {
        void Publisher(OrderPlaced evt);
    }
}
