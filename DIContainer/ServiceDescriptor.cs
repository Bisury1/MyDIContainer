namespace DIContainer;

public abstract class ServiceDescriptor
{
    public Type ServiceType { get; init; }
    public ServiceLifetime Lifetime { get; init; }

    protected ServiceDescriptor(Type serviceType, ServiceLifetime serviceLifetime)
    {
        ServiceType = serviceType;
        Lifetime = serviceLifetime;
    }
}

public class FactoryServiceDescriptor : ServiceDescriptor
{
    public Func<IScope, object> Factory { get; set; }
    
    public FactoryServiceDescriptor(Type serviceType, ServiceLifetime serviceLifetime, Func<IScope, object> factory) 
        : base(serviceType, serviceLifetime)
    {
        Factory = factory;
    }
}

public class ImplementationServiceDescriptor : ServiceDescriptor
{
    public object Implementation { get; set; }

    public ImplementationServiceDescriptor(Type serviceType, ServiceLifetime serviceLifetime, object implementation) 
        : base(serviceType, serviceLifetime)
    {
        Implementation = implementation; 
    }
}

public class TypeServiceDescriptor : ServiceDescriptor
{
    public Type ImplementationType { get; set; }
    
    public TypeServiceDescriptor(Type serviceType, ServiceLifetime serviceLifetime, Type implementationType) 
        : base(serviceType, serviceLifetime)
    {
        ImplementationType = implementationType;
    }
}