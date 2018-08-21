using System;
using System.Collections.Generic;
using System.Threading.Tasks.Dataflow;

namespace SundayBus
{
    public class Port : IPort
    {
        private readonly ISourceBlock<IBusMessage> _fromBus;
        private readonly ITargetBlock<IBusMessage> _toBus;
        private Dictionary<Type, Action<object>> _actions = new Dictionary<Type, Action<object>>();

        public Port(ISourceBlock<IBusMessage> fromBus, ITargetBlock<IBusMessage> toBus)
        {
            _fromBus = fromBus;
            _toBus = toBus;
        }
        public void Publish<T>(T message)
        {
            _toBus.Post(new PublishMessage(message, typeof(T)));
        }

        public void Subscribe<T>(Action<T> callback)
        {
            _actions[typeof(T)] = o => callback((T) o);
            _toBus.Post(new SubscribeMessage(typeof(T)));
        }

        private void Process(IBusMessage msg)
        {
            switch (msg)
            {
                case IPublishMessage pub:
                    if (_actions.TryGetValue(pub.MessageType, out var a))
                    {
                        a(pub.Message);
                    }
                    break;
            }
        }
    }
}