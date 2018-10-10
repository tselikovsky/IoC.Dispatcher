using StoryCLM.Events.Converters;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoryCLM.Events.Converters
{
    public abstract class Converter<Tin, Tout> : IConverter<Tin> , IConverter<Tin, Tout>//where Tout : IMessage
    {
        public abstract Task<Tout> ConvertToAsync(Tin item);

        public async Task<object> ConvertAsync(object item) => await ConvertToAsync((Tin)item);

        async Task<Tout>  IConverter<Tin, Tout>.ConvertAsync(Tin item) => await ConvertToAsync(item);
    }

    public abstract class CollectionConverter<Tin, Tout> : IConverter<IEnumerable<Tin>> 
    {
        public abstract Task<Tout> ConvertToAsync(IEnumerable<Tin> item);

        public async Task<object> ConvertAsync(object item) => await ConvertToAsync(((IEnumerable<object>)item).Select(o=>(Tin)o));

    }
}
