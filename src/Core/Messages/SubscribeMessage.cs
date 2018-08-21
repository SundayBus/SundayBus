using System;

namespace SundayBus
{
    public class SubscribeMessage : ISubscribeMessage
    {
        public SubscribeMessage(Type messageType)
        {
            MessageType = messageType;
        }
        public Type MessageType { get; }
    }
}