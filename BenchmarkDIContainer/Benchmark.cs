using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using DIContainer;
using DIContainer.ActivatorEngine;

BenchmarkRunner.Run<DiContainerBenchmark>();

public interface ITestService
{
}

public class TestService : ITestService
{
}

public class TestClassWithCtor
{
    private ITestService _service;
    public TestClassWithCtor(ITestService service)
    {
        _service = service;
    }
}

[MemoryDiagnoser]
public class DiContainerBenchmark
{
    private readonly IServiceCollection _reflectionContainer, _expressionContainer;
    public DiContainerBenchmark()
    {
        var reflectionBuilder = new ServiceCollectionBuilder(new ReflectionActivatorEngine());
        var expressionBuilder = new ServiceCollectionBuilder(new ExpressionActivatorEngine());
        InitBuilder(reflectionBuilder);
        InitBuilder(expressionBuilder);
        _reflectionContainer = reflectionBuilder.Build();
        _expressionContainer = expressionBuilder.Build();
    }

    public void InitBuilder(IServiceCollectionBuilder collectionBuilder)
    {
        collectionBuilder.AddTransient<ITestService, TestService>().AddTransient<TestClassWithCtor, TestClassWithCtor>();
    }

    [Benchmark(Baseline = true)]
    public TestClassWithCtor TestStandartCreate() => new TestClassWithCtor(new TestService());

    [Benchmark]
    public TestClassWithCtor TestReflectionCreate() =>
        (TestClassWithCtor)_reflectionContainer.CreateScope().RealizedService(typeof(TestClassWithCtor));

    [Benchmark]
    public TestClassWithCtor TestExpressionCreate() =>
        (TestClassWithCtor)_expressionContainer.CreateScope().RealizedService(typeof(TestClassWithCtor));

}