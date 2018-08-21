namespace SundayBus
{
    public class InternalBusMessage : IInternalBusMessage
    {
        public InternalBusMessage(PortController source, IBusMessage message)
        {
            this.Source = source;
            this.Message = message;
        }

        public PortController Source { get; }

        public IBusMessage Message { get; }
    }
}