namespace DIContainer;

public interface IServiceCollectionBuilder
{
    void Register(ServiceDescriptor.ServiceDescriptor serviceDescription);
    IServiceCollection Build();
}