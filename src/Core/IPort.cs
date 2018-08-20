using System;

namespace SundayBus
{
    public interface IPort
    {
        void Subscribe<T>(Action<T> callback);
        void Publish<T>(T message);
    }
}