namespace Cafe.Domain.Events
{
    public record OrderPlaced( Guid OrderId, DateTimeOffset At, string Description, decimal Subtotal, decimal Total);
}
