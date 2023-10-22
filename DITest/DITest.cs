using DIContainer;
using DIContainer.ActivatorEngine;
using DIContainer;

namespace DITest;

public class DITest
{
    private readonly IActivatorEngine _activatorEngine = new ExpressionActivatorEngine();
    
    private interface IScopeTest
    {
    }

    private class TestScopeClass : IScopeTest
    {
    }

    private class ClassWithCtor
    {
        public IScopeTest testClass { get; }
        public ClassWithCtor(IScopeTest test)
        {
            testClass = test;
        }
    }

    private IServiceCollection RegisterServiceAndCreateServiceCollection(ServiceLifetime lifetime)
    {
        var builder = new ServiceCollectionBuilder(_activatorEngine);
        if (lifetime == ServiceLifetime.Transient)
            builder.AddTransient<IScopeTest, TestScopeClass>();
        else if (lifetime == ServiceLifetime.Scoped)
            builder.AddScoped<IScopeTest, TestScopeClass>();
        else
            builder.AddSingleton<IScopeTest, TestScopeClass>();
        return builder.Build();
    }

    private void TestScope<T>(Action<T, T> assertFunc1, Action<T, T>? assertFunc2, ServiceLifetime lifetime)
    {
        var serviceCollection = RegisterServiceAndCreateServiceCollection(lifetime);
        IScope scope = serviceCollection.CreateScope();
        T testClass1 = (T)scope.RealizedService(typeof(T));
        T testClass2 = (T)scope.RealizedService(typeof(T));
        assertFunc1(testClass1, testClass2);
        if (assertFunc2 is null)
            return;
        IScope scope2 = serviceCollection.CreateScope();
        T testClassFromAnotherScope = (T)scope2.RealizedService(typeof(T));
        assertFunc2(testClass1, testClassFromAnotherScope);
    }
    
    [Fact]
    public void TestTransientScope() 
        => TestScope<IScopeTest>(Assert.NotEqual, null, ServiceLifetime.Transient);

    [Fact]
    public void TestScopedScope()
        => TestScope<IScopeTest>(Assert.Equal, Assert.NotEqual, ServiceLifetime.Scoped);
    
    [Fact]
    public void TestSingletonScope() 
        => TestScope<IScopeTest>(Assert.Equal, Assert.Equal, ServiceLifetime.Singleton);

    [Fact]
    public void TestConstructor()
    {
        IServiceCollectionBuilder builder = new ServiceCollectionBuilder(_activatorEngine);
        builder.AddTransient<IScopeTest, TestScopeClass>();
        builder.AddTransient<ClassWithCtor, ClassWithCtor>();
        var serviceCollection = builder.Build();
        IScope scope = serviceCollection.CreateScope();
        var service = (ClassWithCtor)scope.RealizedService(typeof(ClassWithCtor));
        Assert.NotNull(service.testClass);
    }
}