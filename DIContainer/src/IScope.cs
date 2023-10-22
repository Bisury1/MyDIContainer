namespace DIContainer;

public interface IScope : IDisposable, IAsyncDisposable
{
    public object RealizedService(Type service);
}