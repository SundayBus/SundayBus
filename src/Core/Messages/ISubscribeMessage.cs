using System;

namespace SundayBus
{
    public interface ISubscribeMessage : IBusMessage
    {
        Type MessageType { get; }
    }
}