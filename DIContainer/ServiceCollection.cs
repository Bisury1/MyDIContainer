using System.Collections.Concurrent;
using System.Collections.Immutable;
using BindingFlags = System.Reflection.BindingFlags;

namespace DIContainer;

public class ServiceCollection : IServiceCollection
{
    private readonly ImmutableDictionary<Type, ServiceDescriptor> _descriptors;
    private readonly ConcurrentDictionary<Type, Func<IScope, object>> _serviceActivators = new();
    internal readonly Scope RootScope;
    
    public ServiceCollection(IEnumerable<ServiceDescriptor> serviceDescriptors)
    {
        _descriptors = serviceDescriptors.ToImmutableDictionary(d => d.ServiceType);
        RootScope = new Scope(this);
    }
    
    internal ServiceDescriptor GetServiceDescriptor(Type serviceType)
    {
        if (!_descriptors.TryGetValue(serviceType, out var result))
            throw new InvalidOperationException($"{serviceType} is not register");
        return result;
    }

    private Func<IScope, object> CreateActivator(Type serviceType)
    {
        var descriptor = GetServiceDescriptor(serviceType);

        if (descriptor is FactoryServiceDescriptor factoryServiceDescriptor)
            return factoryServiceDescriptor.Factory;

        if (descriptor is ImplementationServiceDescriptor implementationServiceDescriptor)
            return _ => implementationServiceDescriptor;

        var typeServiceDescriptor = (TypeServiceDescriptor)descriptor;
        var ctor = typeServiceDescriptor.ImplementationType.GetConstructors(
            BindingFlags.Public | BindingFlags.Instance).Single();
        var args = ctor.GetParameters();

        return s =>
        {
            var argsForServiceCtor = new object[args.Length];

            for (int i = 0; i < args.Length; i++)
            {
                argsForServiceCtor[i] = s.RealizedService(args[i].ParameterType);
            }

            return ctor.Invoke(argsForServiceCtor);
        };
    }
    internal object CreateInstance(Type serviceType, IScope scope)
    {
        return _serviceActivators.GetOrAdd(serviceType, CreateActivator)(scope);
    }
    
    public IScope CreateScope()
    {
        return new Scope(this);
    }
}

public interface IScope : IDisposable, IAsyncDisposable
{
    public object RealizedService(Type service);
    public TService RealizedService<TService>();
}

public interface IServiceCollection
{
    IScope CreateScope();
}