using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace SundayBus
{
    public class Bus : IBus
    {
        private Dictionary<IPort, ITargetBlock<IBusMessage>> _portBlocks = new Dictionary<IPort, ITargetBlock<IBusMessage>>();
        private Dictionary<Type, HashSet<IPort>> _portTypes = new Dictionary<Type, HashSet<IPort>>();
        private ActionBlock<IInternalBusMessage> _processor;
        private ConcurrentExclusiveSchedulerPair _scheduler = new ConcurrentExclusiveSchedulerPair();
        private ActionBlock<TaskCompletionSource<IPort>> _portGetter;
        public Bus()
        {
            _processor = new ActionBlock<IInternalBusMessage>(m => Process(m), new ExecutionDataflowBlockOptions { TaskScheduler = _scheduler.ExclusiveScheduler });
            _portGetter = new ActionBlock<TaskCompletionSource<IPort>>(m => Process(m), new ExecutionDataflowBlockOptions { TaskScheduler = _scheduler.ExclusiveScheduler });
        }

        private void Process(IInternalBusMessage msg)
        {
            switch (msg.Message)
            {
                case IPublishMessage pub:
                    Publish(pub.Message, msg.Source);
                    break;
                case ISubscribeMessage sub:
                    Subscribe(msg.Source, sub.MessageType);
                    break;
            }
        }

        private void Subscribe(IPort port, Type type)
        {
            foreach (var p in _portTypes)
            {
                if (p.Value.Count > 0 && p.Key != type && (type.IsAssignableFrom(p.Key) || p.Key.IsAssignableFrom(type)))
                {
                    p.Value.Remove(port);
                    if (p.Value.Count == 0)
                    {
                        OnAllUnsubscribed(p.Key);
                    }
                }
            }

            if (!_portTypes.TryGetValue(type, out var set))
            {
                set = new HashSet<IPort>();
                _portTypes[type] = set;
            }

            set.Add(port);
        }

        private void Publish(object message, IPort source)
        {
            var type = message.GetType();
            foreach (var p in _portTypes)
            {
                if (p.Value.Count > 0 && (p.Key.IsAssignableFrom(type)))
                {
                    foreach (var r in p.Value)
                    {
                        if (r != source && _portBlocks.TryGetValue(r, out var block))
                        {
                            block.Post(new PublishMessage(message, p.Key));
                        }
                    }
                }
            }
        }

        private void OnAllUnsubscribed(Type type)
        {

        }
        private void Process(TaskCompletionSource<IPort> tcs)
        {
            var portBlock = new BufferBlock<IBusMessage>();

            var port = new Port(portBlock, _processor);

            _portBlocks[port] = portBlock;

            tcs.TrySetResult(port);
        }
        public Task<IPort> GetPort()
        {
            var tcs = new TaskCompletionSource<IPort>();
            _portGetter.Post(tcs);
            return tcs.Task;
        }
    }
}