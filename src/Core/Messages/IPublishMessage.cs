using System;

namespace SundayBus
{
    public interface IPublishMessage : IBusMessage
    {
        object Message { get; }
        Type MessageType { get; }
    }
}