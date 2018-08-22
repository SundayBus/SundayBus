using System;
using System.Threading;
using System.Threading.Tasks;
using SundayBus;

namespace PingPong
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var bus = new Bus();
            var ping = await bus.GetPort();
            var pong = await bus.GetPort();

            ping.Subscribe<string>(s =>
            {
                Console.WriteLine($"Pinger received: {s}");
                Thread.Sleep(1000);
                ping.Publish("Ping");
            });

            pong.Subscribe<string>(s =>
            {
                Console.WriteLine($"Ponger received: {s}");
                Thread.Sleep(1000);
                pong.Publish("Pong");
            });

            ping.Publish("Ping");

            Console.ReadLine();
        }
    }
}
