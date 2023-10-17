namespace DIContainer;

public enum ServiceLifetime: byte
{
    Transient,
    Scoped,
    Singleton
}

public interface IServiceCollectionBuilder
{
    void Register(ServiceDescriptor serviceDescription);
    IServiceCollection Build();

}

public class ServiceCollectionBuilder: IServiceCollectionBuilder
{
    private List<ServiceDescriptor> _descriptors = new();
    
    public void Register(ServiceDescriptor serviceDescriptor)
    {
        _descriptors.Add(serviceDescriptor);
    }

    public IServiceCollection Build()
    {
        return new ServiceCollection(_descriptors);
    }
}