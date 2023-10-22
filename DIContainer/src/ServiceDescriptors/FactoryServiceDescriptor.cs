namespace DIContainer.ServiceDescriptor;

public class FactoryServiceDescriptor : ServiceDescriptor
{
    public Func<IScope, object> Factory { get; }
    
    public FactoryServiceDescriptor(Type serviceType, ServiceLifetime serviceLifetime, Func<IScope, object> factory) 
        : base(serviceType, serviceLifetime)
    {
        Factory = factory;
    }
}