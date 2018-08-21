using System;

namespace SundayBus
{
    public class PublishMessage : IPublishMessage
    {
        public PublishMessage(object message, Type messageType)
        {
            Message = message;
            MessageType = messageType;
        }
        public object Message { get; }
        public Type MessageType { get; }
    }
}