namespace DIContainer.ServiceDescriptor;

public class TypeServiceDescriptor : ServiceDescriptor
{
    public Type ImplementationType { get; }
    
    public TypeServiceDescriptor(Type serviceType, ServiceLifetime serviceLifetime, Type implementationType) 
        : base(serviceType, serviceLifetime)
    {
        ImplementationType = implementationType;
    }
}