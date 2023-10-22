namespace DIContainer.ServiceDescriptor;

public class ImplementationServiceDescriptor : ServiceDescriptor
{
    public object Implementation { get; set; }

    public ImplementationServiceDescriptor(Type serviceType, ServiceLifetime serviceLifetime, object implementation) 
        : base(serviceType, serviceLifetime)
    {
        Implementation = implementation; 
    }
}