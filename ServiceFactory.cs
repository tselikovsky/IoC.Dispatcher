using System;
using System.Collections.Generic;

namespace StoryCLM.Events
{
    /// <summary>
    /// Factory method used to resolve all services. For multiple instances, it will resolve against <see cref="IEnumerable{T}" />
    /// </summary>
    /// <param name="serviceType">Type of service to resolve</param>
    /// <returns>An instance of type <paramref name="serviceType" /></returns>
    public delegate object ServiceFactory(Type serviceType);

    public static class ServiceFactoryExtensions
    {
        public static T GetInstance<T>(this ServiceFactory factory)
            => (T)factory(typeof(T));

        public static IEnumerable<T> GetInstances<T>(this ServiceFactory factory)
            => (IEnumerable<T>)factory(typeof(IEnumerable<T>));

        public static object GetInstance(this ServiceFactory factory, Type instanceType)
            => factory(instanceType);

        public static IEnumerable<object> GetInstances(this ServiceFactory factory, Type instanceType)
            => (IEnumerable<object>)factory(typeof(IEnumerable<>).MakeGenericType(instanceType));
    }
}