using System.Threading.Tasks;

namespace StoryCLM.Events.Converters
{
    public interface IConverter<in Tin>
    {
        Task<object> ConvertAsync(object item);
    }
    public interface IConverter<in Tin, Tout>
    {
        Task<Tout> ConvertAsync(Tin item);
    }
}
