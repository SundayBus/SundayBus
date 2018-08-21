using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace SundayBus
{
    public class PortController
    {
        private readonly ISourceBlock<IInternalBusMessage> _fromBus;
        private readonly ITargetBlock<IInternalBusMessage> _toBus;
        private HashSet<Type> _subscribed = new HashSet<Type>();
        private HashSet<Type> _ignored = new HashSet<Type>();
        private readonly TransformManyBlock<IInternalBusMessage, IBusMessage> _toPort;
        private readonly TransformManyBlock<IBusMessage, IInternalBusMessage> _fromPort;

        public PortController(ISourceBlock<IInternalBusMessage> fromBus, ITargetBlock<IInternalBusMessage> toBus)
        {
            _fromBus = fromBus;
            _toBus = toBus;
            var sched = new ConcurrentExclusiveSchedulerPair();
            _toPort = new TransformManyBlock<IInternalBusMessage, IBusMessage>(m => ToPort(m), new ExecutionDataflowBlockOptions { TaskScheduler = sched.ExclusiveScheduler });
            _fromPort = new TransformManyBlock<IBusMessage, IInternalBusMessage>(m => FromPort(m), new ExecutionDataflowBlockOptions { TaskScheduler = sched.ExclusiveScheduler });
            _fromBus.LinkTo(_toPort);
            _fromPort.LinkTo(_toBus);
            Port = new Port(_toPort, _fromPort);
        }

        public IPort Port { get; }

        private IEnumerable<IInternalBusMessage> FromPort(IBusMessage msg)
        {
            switch (msg)
            {
                case SubscribeMessage sub:
                    _subscribed.Add(sub.MessageType);
                    break;
            }
            yield return new InternalBusMessage(this, msg);
        }
        private IEnumerable<IBusMessage> ToPort(IInternalBusMessage msg)
        {
            if (msg.Source == this)
                yield break;

            switch (msg.Message)
            {
                case PublishMessage pub:
                    if (!_subscribed.Contains(pub.MessageType))
                    {
                        if (_ignored.Contains(pub.MessageType))
                            break;
                        foreach (var t in _subscribed)
                        {
                            if (t.IsAssignableFrom(pub.MessageType))
                            {
                                _subscribed.Add(pub.MessageType);
                                break;
                            }
                        }
                    }
                    else
                    {
                        yield return pub;
                        break;
                    }
                    if (_subscribed.Contains(pub.MessageType))
                    {
                        yield return pub;
                    }
                    else
                    {
                        _ignored.Add(pub.MessageType);
                    }
                    break;
            }
        }
    }
}