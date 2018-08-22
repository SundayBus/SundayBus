using System.Threading.Tasks;

namespace SundayBus
{
    public interface IBus
    {
        Task<IPort> GetPort();
    }
}