using System;

namespace SundayBus
{
    public class SubscribeMessage : ISubscribeMessage
    {
        public SubscribeMessage()
        { }

        public object Message { get; }

        public Type MessageType { get; }
    }
}