using DIContainer.ActivatorEngine;
using DIContainer;


namespace DIContainer;


public class ServiceCollectionBuilder: IServiceCollectionBuilder
{
    private readonly List<ServiceDescriptor.ServiceDescriptor> _descriptors = new();
    private readonly IActivatorEngine _activatorEngine;
    public ServiceCollectionBuilder(IActivatorEngine activatorEngine)
    {
        _activatorEngine = activatorEngine;
    }
    
    public void Register(ServiceDescriptor.ServiceDescriptor serviceDescriptor)
    {
        _descriptors.Add(serviceDescriptor);
    }

    public IServiceCollection Build()
    {
        return new ServiceCollection(_descriptors, _activatorEngine);
    }
}