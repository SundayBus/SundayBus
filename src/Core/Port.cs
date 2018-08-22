using System;
using System.Collections.Generic;
using System.Threading.Tasks.Dataflow;

namespace SundayBus
{
    public class Port : IPort
    {
        public Port(ISourceBlock<IBusMessage> fromBus, ITargetBlock<IInternalBusMessage> toBus)
        {
            FromBus = fromBus;
            ToBus = new TransformBlock<IBusMessage, IInternalBusMessage>(m => new InternalBusMessage(this, m));
        }

        public ISourceBlock<IBusMessage> FromBus { get; }
        public ITargetBlock<IBusMessage> ToBus { get; }
    }
}