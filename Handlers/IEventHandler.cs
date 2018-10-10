using System.Threading.Tasks;

namespace StoryCLM.Events
{
    public interface IEventHandler<in T> where T : IEvent
    {
        Task HandleAsync(T notification);
    }
}
