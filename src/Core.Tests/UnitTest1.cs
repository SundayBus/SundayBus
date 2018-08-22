using System;
using System.Threading.Tasks.Dataflow;
using SundayBus;
using Xunit;

namespace Core.Tests
{
    public class UnitTest1
    {
        [Fact]
        public async void Test1()
        {
            IBus bus = new Bus();
            IPort port = await bus.GetPort();
            var latch = new System.Threading.CountdownEvent(1);

            port.Subscribe<string>(s =>
            {
                if (string.Equals("Hello", s)) latch.Signal();
            });

            IPort pub = await bus.GetPort();

            pub.Publish("Hello");
            Assert.True(latch.Wait(1000));
        }
    }
}