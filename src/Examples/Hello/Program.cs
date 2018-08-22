using System;
using System.Threading.Tasks;
using SundayBus;

namespace Hello
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var bus = new Bus();
            var pub = await bus.GetPort();
            var sub = await bus.GetPort();
            sub.Subscribe<string>(s => Console.WriteLine(s));
            pub.Publish("hello");
            Console.ReadLine();
        }
    }
}
