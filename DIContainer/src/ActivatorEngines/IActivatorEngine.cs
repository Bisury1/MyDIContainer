namespace DIContainer.ActivatorEngine;

public interface IActivatorEngine
{
    public Func<IScope, object> CreateActivator(ServiceDescriptor.ServiceDescriptor serviceDescriptor);
}