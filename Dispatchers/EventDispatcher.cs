using Events.ExtremeConfigAwait;
using System;
using System.Threading.Tasks;

namespace StoryCLM.Events.Dispatchers
{
    public interface IEventDispatcher
    {
        Task DispatchAsync(object @event);
    }

    public class EventDispatcher : IEventDispatcher
    {
        ServiceFactory Factory { get; }
        public EventDispatcher(ServiceFactory factory)
        {
            Factory = factory;
        }

        public virtual async Task DispatchAsync(object @event)
        {
            await new SynchronizationContextRemover();

            var eventType = @event.GetType();
            var handlerType = typeof(IEventHandler<>).MakeGenericType(eventType);
            var handlers = Factory.GetInstances(handlerType);

            foreach (var handler in handlers)
                try //Ошибки не должны всплывать наружу, чтобы не нарушать работу инфраструктуру
                {
                    await ((Task)((dynamic)handler).HandleAsync((dynamic)@event));
                }
                catch (Exception ex)
                {
                }
        }
    }
}