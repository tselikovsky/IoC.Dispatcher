using Events.ExtremeConfigAwait;
using StoryCLM.Events.Converters;
using StoryCLM.Events.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoryCLM.Events.Dispatchers
{
    public class ConverterDispatcherHelper<Tin>
    {
        ConverterDispatcher Dispatcher { get; set; }
        Tin From { get; set; }
        public ConverterDispatcherHelper(ConverterDispatcher dispatcher, Tin args)
        {
            Dispatcher = dispatcher;
            From = args;
        }
        
        public Task<Tout> To<Tout>()
        {
            return Dispatcher.ConvertAsync<Tin, Tout>(From);
        }
    }

    public class ConverterDispatcher
    {
        private readonly ServiceFactory _context;

        public ConverterDispatcher(ServiceFactory context)
        {
            _context = context;
        }

        public ConverterDispatcherHelper<Tin> Convert<Tin>(Tin args)
        {
            return new ConverterDispatcherHelper<Tin>(this, args);
        }


        public async Task<object> ConvertAsync(object externalMessage)
        {
            var eventType = externalMessage.GetType();
            var adapterType = typeof(IConverter<>).MakeGenericType(eventType);
            var adapter = _context.GetInstance(adapterType);
            if (adapter == null) throw new ConverterNotFoundException();
            return await(Task<object>)((dynamic)adapter).ConvertAsync((dynamic)externalMessage);
        }

        public async Task<Tout> ConvertAsync<Tin,Tout>(Tin externalMessage)
        {
            var adapter = _context.GetInstance<IConverter<Tin,Tout>>();
            if (adapter == null) throw new ConverterNotFoundException();
            return await adapter.ConvertAsync(externalMessage);
        }

        /// <summary>
        /// Convert collection of objects to collection of converted objects. Find converter for each object.
        /// </summary>
        /// <param name="externalMessages"></param>
        /// <returns></returns>
        public async Task<IEnumerable<object>> ConvertEachAsync(IEnumerable<object> externalMessages)
        {
            await new SynchronizationContextRemover();
            return await Task.WhenAll(externalMessages.Select(async m => await ConvertAsync(m)));
        }

        /// <summary>
        /// Convert collection of objects to collection of converted objects. 
        /// Find one converter for all collection.
        /// The converter is determined by the <paramref name="objectType"/> argument or (if previous is null) by the generic <typeparamref name="T"/>argument.
        /// </summary>
        /// <typeparam name="T">If <paramref name="objectType"/> is null, <typeparamref name="T"/> is used for converter determination.</typeparam>
        /// <param name="externalMessages"></param>
        /// <param name="objectType">Used for the converter determination. Used if the parameter <typeparamref name="T"/> is object.</param>
        /// <returns></returns>
        public async Task<IEnumerable<object>> ConvertEachAsync<T>(IEnumerable<T> externalMessages, Type objectType = null)
        {
            await new SynchronizationContextRemover();

            objectType = objectType ?? typeof(T);
            var adapterType = typeof(IConverter<>).MakeGenericType(objectType);
            var adapter = _context.GetInstance(adapterType);
            if (adapter == null) throw new ConverterNotFoundException();

            var tasks = externalMessages.Select(entity => ((Task<object>)((dynamic)adapter).ConvertAsync((dynamic)entity)));
            List<object> messages = new List<object>();
            foreach (var task in tasks)
                messages.Add(await task);
            return messages;
        }

        public async Task<object> ConvertAsOneAsync<T>(IEnumerable<T> externalMessages, Type objectType=null)
        {
            await new SynchronizationContextRemover();

            objectType = objectType ?? typeof(T);
            var adapterType = typeof(IConverter<>).MakeGenericType(typeof(IEnumerable<>).MakeGenericType(objectType));
            var adapter = _context.GetInstance(adapterType);
            if (adapter == null) throw new ConverterNotFoundException();
            return await (Task<object>)((dynamic)adapter).ConvertAsync(externalMessages);
        }
    }
}
