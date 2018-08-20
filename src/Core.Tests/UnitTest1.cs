using System;
using SundayBus;
using Xunit;

namespace Core.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            IBus bus = new Bus();
            IPort port = bus.GetPort();
            var latch = new System.Threading.CountdownEvent(1);
            port.Subscribe<string>(s =>
            {
                if (string.Equals("Hello", s)) latch.Signal();
            });
            IPort pub = bus.GetPort();
            pub.Publish("Hello");
            Assert.True(latch.Wait(1000));
        }
    }
}