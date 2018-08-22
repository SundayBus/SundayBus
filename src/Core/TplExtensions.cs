using System;
using System.Collections.Generic;
using System.Threading.Tasks.Dataflow;

namespace SundayBus
{
    public static class TplExtensions
    {
        public static ISourceBlock<T> Subscribe<T>(this IPort source)
        {
            var t = new TransformManyBlock<IBusMessage, T>(m => MatchPublish<T>(m));
            source.FromBus.LinkTo(t);
            source.ToBus.Post(new SubscribeMessage(typeof(T)));
            return t;
        }

        public static void Subscribe<T>(this IPort source, Action<T> action)
        {
            source.Subscribe<T>().LinkTo(new ActionBlock<T>(action));
        }

        public static void Publish<T>(this IPort source, T message)
        {
            source.ToBus.Post(new PublishMessage(message, typeof(T)));
        }

        private static IEnumerable<T> MatchPublish<T>(IBusMessage m)
        {
            switch (m)
            {
                case IPublishMessage pub:
                    switch (pub.Message)
                    {
                        case T match:
                            yield return match;
                            break;
                    }
                    break;
            }
        }
    }
}