namespace SundayBus
{
    public class Bus : IBus
    {
        public IPort GetPort()
        {
            return new Port();
        }
    }
}