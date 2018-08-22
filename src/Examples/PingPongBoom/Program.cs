using System;
using System.Threading.Tasks;
using SundayBus;

namespace PingPongBoom
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var bus = new Bus();
            var ping = await bus.GetPort();
            var pong = await bus.GetPort();
            var boom = await bus.GetPort();

            ping.Subscribe<string>(s =>
            {
                Console.WriteLine($"Pinger received: {s}");
                //Thread.Sleep(1000);
                ping.Publish("Ping");
            });

            pong.Subscribe<string>(s =>
            {
                Console.WriteLine($"Ponger received: {s}");
                //Thread.Sleep(1000);
                pong.Publish("Pong");
            });

            boom.Subscribe<string>(s =>
            {
                Console.WriteLine($"Boomer received: {s}");
                //Thread.Sleep(1000);
                pong.Publish("Boom");
            });


            ping.Publish("Ping");

            Console.ReadLine();
        }
    }
}
