using System.Reflection;

namespace DIContainer.ActivatorEngine;

public class ReflectionActivatorEngine: BaseActivatorEngine
{
    protected override Func<IScope, object> CreateActivatorForTypeDescriptor(ConstructorInfo ctor, ParameterInfo[] args)
    {
        return s =>
        {
            var argsForServiceCtor = new object[args.Length];

            for (int i = 0; i < args.Length; i++)
            {
                argsForServiceCtor[i] = s.RealizedService(args[i].ParameterType);
            }

            return ctor.Invoke(argsForServiceCtor);
        };
    }
}