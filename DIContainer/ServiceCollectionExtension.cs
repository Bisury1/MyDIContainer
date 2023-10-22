using DIContainer;
using DIContainer.ServiceDescriptor;

namespace DIContainer;

public static class ServiceCollectionBuilderExtension
{
    private static IServiceCollectionBuilder AddType(this IServiceCollectionBuilder serviceCollectionBuilder, 
        Type serviceType, ServiceLifetime serviceLifetime, Type implementationType)
    {
        serviceCollectionBuilder.Register(new TypeServiceDescriptor(serviceType,
            serviceLifetime, implementationType));
        return serviceCollectionBuilder;
    }

    private static IServiceCollectionBuilder AddFactory(this IServiceCollectionBuilder serviceCollectionBuilder,
        Type serviceType, ServiceLifetime serviceLifetime, Func<IScope, object> factory)
    {
        serviceCollectionBuilder.Register(new FactoryServiceDescriptor(serviceType, serviceLifetime, factory));
        return serviceCollectionBuilder;
    }

    private static IServiceCollectionBuilder AddImplementation(this IServiceCollectionBuilder serviceCollectionBuilder,
        Type serviceType, ServiceLifetime serviceLifetime, object serviceImplementation)
    {
        serviceCollectionBuilder.Register(new ImplementationServiceDescriptor(
            serviceType, serviceLifetime, serviceImplementation));
        return serviceCollectionBuilder;
    }

    public static IServiceCollectionBuilder AddTransient(this IServiceCollectionBuilder serviceCollectionBuilder,
        Type serviceType, Type implementationType) => serviceCollectionBuilder.AddType(
        serviceType, ServiceLifetime.Transient, implementationType);
    
    public static IServiceCollectionBuilder AddScoped(this IServiceCollectionBuilder serviceCollectionBuilder,
        Type serviceType, Type implementationType) => serviceCollectionBuilder.AddType(
        serviceType, ServiceLifetime.Scoped, implementationType);
    
    public static IServiceCollectionBuilder AddSingleton(this IServiceCollectionBuilder serviceCollectionBuilder,
        Type serviceType, Type implementationType) => serviceCollectionBuilder.AddType(
        serviceType, ServiceLifetime.Singleton, implementationType);
    
    public static IServiceCollectionBuilder AddTransient(this IServiceCollectionBuilder serviceCollectionBuilder,
        Type serviceType) => serviceCollectionBuilder.AddType(
        serviceType, ServiceLifetime.Transient, serviceType);
    
    public static IServiceCollectionBuilder AddScoped(this IServiceCollectionBuilder serviceCollectionBuilder,
        Type serviceType) => serviceCollectionBuilder.AddType(
        serviceType, ServiceLifetime.Scoped, serviceType);
    
    public static IServiceCollectionBuilder AddSingleton(this IServiceCollectionBuilder serviceCollectionBuilder,
        Type serviceType) => serviceCollectionBuilder.AddType(
        serviceType, ServiceLifetime.Singleton, serviceType);
    
    public static IServiceCollectionBuilder AddTransient<TService, TImplementation>(
        this IServiceCollectionBuilder serviceCollectionBuilder) => serviceCollectionBuilder.AddType(
        typeof(TService), ServiceLifetime.Transient, typeof(TImplementation));
    
    public static IServiceCollectionBuilder AddScoped<TService, TImplementation>(
        this IServiceCollectionBuilder serviceCollectionBuilder) => serviceCollectionBuilder.AddType(
        typeof(TService), ServiceLifetime.Scoped, typeof(TImplementation));
    
    public static IServiceCollectionBuilder AddSingleton<TService, TImplementation>(
        this IServiceCollectionBuilder serviceCollectionBuilder) => serviceCollectionBuilder.AddType(
        typeof(TService), ServiceLifetime.Singleton, typeof(TImplementation));

    public static IServiceCollectionBuilder AddTransient<TService>(
        this IServiceCollectionBuilder serviceCollectionBuilder,
        Func<IScope, object> factory) => serviceCollectionBuilder.AddFactory(
        typeof(TService), ServiceLifetime.Transient, factory);
    
    public static IServiceCollectionBuilder AddScoped<TService>(
        this IServiceCollectionBuilder serviceCollectionBuilder,
        Func<IScope, object> factory) => serviceCollectionBuilder.AddFactory(
        typeof(TService), ServiceLifetime.Scoped, factory);
    
    public static IServiceCollectionBuilder AddSingleton<TService>(
        this IServiceCollectionBuilder serviceCollectionBuilder,
        Func<IScope, object> factory) => serviceCollectionBuilder.AddFactory(
        typeof(TService), ServiceLifetime.Singleton, factory);
    
    public static IServiceCollectionBuilder AddTransient<TService>(
        this IServiceCollectionBuilder serviceCollectionBuilder,
        object implementation) => serviceCollectionBuilder.AddImplementation(
        typeof(TService), ServiceLifetime.Transient, implementation);
    
    public static IServiceCollectionBuilder AddScoped<TService>(
        this IServiceCollectionBuilder serviceCollectionBuilder,
        object implementation) => serviceCollectionBuilder.AddImplementation(
        typeof(TService), ServiceLifetime.Scoped, implementation);
    
    public static IServiceCollectionBuilder AddSingleton<TService>(
        this IServiceCollectionBuilder serviceCollectionBuilder,
        object implementation) => serviceCollectionBuilder.AddImplementation(
        typeof(TService), ServiceLifetime.Singleton, implementation);
}