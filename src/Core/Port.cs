using System;

namespace SundayBus
{
    public class Port : IPort
    {
        public void Publish<T>(T message)
        {
            //TODO:
        }

        public void Subscribe<T>(Action<T> callback)
        {
            //TODO:
        }
    }
}