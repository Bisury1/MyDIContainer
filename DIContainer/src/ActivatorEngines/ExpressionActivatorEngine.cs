using System.Linq.Expressions;
using System.Reflection;

namespace DIContainer.ActivatorEngine;

public class ExpressionActivatorEngine: BaseActivatorEngine
{
    private readonly MethodInfo _resolveMethod =
        typeof(IScope).GetMethod("RealizedService") ?? throw new InvalidOperationException();

    protected override Func<IScope, object> CreateActivatorForTypeDescriptor(ConstructorInfo ctor, ParameterInfo[] args)
    {
        var scopeParam = Expression.Parameter(typeof(IScope), "scope");
        var ctorArgs = args.Select(arg => Expression.Convert(Expression.Call(
            scopeParam, _resolveMethod, Expression.Constant(arg.ParameterType)), arg.ParameterType));
        var lambdaResult = Expression.New(ctor, ctorArgs);
        var lambda = Expression.Lambda<Func<IScope, object>>(lambdaResult, scopeParam);
        return lambda.Compile();
    }
}