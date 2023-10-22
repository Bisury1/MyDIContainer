using System.Collections.Concurrent;
using System.Collections.Immutable;
using DIContainer.ActivatorEngine;

namespace DIContainer;

public class ServiceCollection : IServiceCollection
{
    private readonly ImmutableDictionary<Type, ServiceDescriptor.ServiceDescriptor> _descriptors;
    private readonly ConcurrentDictionary<Type, Func<IScope, object>> _serviceActivators = new();
    internal readonly Scope RootScope;
    private readonly IActivatorEngine _activatorEngine;
    
    internal ServiceCollection(IEnumerable<ServiceDescriptor.ServiceDescriptor> serviceDescriptors, IActivatorEngine activatorEngine)
    {
        _activatorEngine = activatorEngine;
        _descriptors = serviceDescriptors.ToImmutableDictionary(d => d.ServiceType);
        RootScope = new Scope(this);
    }
    
    internal ServiceDescriptor.ServiceDescriptor GetServiceDescriptor(Type serviceType)
    {
        if (!_descriptors.TryGetValue(serviceType, out var result))
            throw new InvalidOperationException($"{serviceType} is not register");
        return result;
    }

    private Func<IScope, object> CreateActivator(Type serviceType)
    {
        var descriptor = GetServiceDescriptor(serviceType);

        return _activatorEngine.CreateActivator(descriptor);
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
