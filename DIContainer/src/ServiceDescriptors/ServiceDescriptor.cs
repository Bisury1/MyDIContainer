namespace DIContainer.ServiceDescriptor;

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