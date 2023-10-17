using DIContainer;

namespace DITest;

public class DITest
{
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
        var builder = new ServiceCollectionBuilder();
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
        T testClass1 = scope.RealizedService<T>();
        T testClass2 = scope.RealizedService<T>();
        assertFunc1(testClass1, testClass2);
        if (assertFunc2 is null)
            return;
        IScope scope2 = serviceCollection.CreateScope();
        T testClassFromAnotherScope = scope2.RealizedService<T>();
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
        IServiceCollectionBuilder builder = new ServiceCollectionBuilder();
        builder.AddTransient<IScopeTest, TestScopeClass>();
        builder.AddTransient<ClassWithCtor, ClassWithCtor>();
        var serviceCollection = builder.Build();
        IScope scope = serviceCollection.CreateScope();
        var service = scope.RealizedService<ClassWithCtor>();
        Assert.NotNull(service.testClass);
    }
}