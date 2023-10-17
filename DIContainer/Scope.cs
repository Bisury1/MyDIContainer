using System.Collections.Concurrent;

namespace DIContainer;

internal class Scope : IScope
{
    private readonly ServiceCollection _serviceCollection;
    private readonly ConcurrentDictionary<Type, object> _scopedInstances = new();
    private readonly ConcurrentStack<object> _disposablesInstances = new();

    public Scope(ServiceCollection serviceCollection)
    {
        _serviceCollection = serviceCollection;
    }

    public object RealizedService(Type serviceType)
    {
        var descriptor = _serviceCollection.GetServiceDescriptor(serviceType);

        if (descriptor.Lifetime == ServiceLifetime.Transient)
            return ScopeCreateInstance(serviceType);

        if (descriptor.Lifetime == ServiceLifetime.Scoped || _serviceCollection.RootScope == this)
            return _scopedInstances.GetOrAdd(serviceType,
                _ => ScopeCreateInstance(serviceType));

        return _serviceCollection.RootScope.RealizedService(serviceType);
    }

    public TService RealizedService<TService>()
    {
        return (TService)RealizedService(typeof(TService));
    }

    private object ScopeCreateInstance(Type serviceType)
    {
        var instance = _serviceCollection.CreateInstance(serviceType, this);
        if (instance is IDisposable or IAsyncDisposable)
            _disposablesInstances.Push(instance);

        return instance;
    }

    public void Dispose()
    {
        foreach (var instance in _disposablesInstances)
        {
            if (instance is IDisposable disposable)
                disposable.Dispose();
            else if (instance is IAsyncDisposable)
                throw new InvalidOperationException($"Can't asyncDispose {nameof(instance)} in dispose");
        }
    }

    public async ValueTask DisposeAsync()
    {
        foreach (var instance in _disposablesInstances)
        {
            if (instance is IAsyncDisposable asyncDisposable)
                await asyncDisposable.DisposeAsync();
            else if (instance is IDisposable disposable)
                disposable.Dispose();
        }
    }
}