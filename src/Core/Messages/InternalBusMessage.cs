namespace SundayBus
{
    public class InternalBusMessage : IInternalBusMessage
    {
        public InternalBusMessage(IPort source, IBusMessage message)
        {
            this.Source = source;
            this.Message = message;
        }

        public IPort Source { get; }

        public IBusMessage Message { get; }
    }
}