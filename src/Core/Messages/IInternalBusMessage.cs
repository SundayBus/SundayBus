namespace SundayBus
{
    public interface IInternalBusMessage
    {
        PortController Source { get; }
        IBusMessage Message { get; }
    }
}