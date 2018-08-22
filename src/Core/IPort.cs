using System;
using System.Threading.Tasks.Dataflow;

namespace SundayBus
{
    public interface IPort
    {
        ISourceBlock<IBusMessage> FromBus { get; }
        ITargetBlock<IBusMessage> ToBus { get; }
    }
}