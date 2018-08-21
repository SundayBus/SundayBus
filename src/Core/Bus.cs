using System.Threading.Tasks.Dataflow;

namespace SundayBus
{
    public class Bus : IBus
    {
        private BroadcastBlock<IInternalBusMessage> _broadcast;
        private TransformManyBlock<IInternalBusMessage, IInternalBusMessage> _fromPorts;

        public Bus()
        {

        }

        private IEnumerable<IInternalBusMessage> Process(IInternalBusMessage)
        {

        }
        public IPort GetPort()
        {
            return new Port();
        }
    }
}