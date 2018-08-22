namespace SundayBus
{
    public interface IInternalBusMessage
    {
        IPort Source { get; }
        IBusMessage Message { get; }
    }
}