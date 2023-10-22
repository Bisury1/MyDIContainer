using System.Diagnostics;
using System.Reflection;
using DIContainer.ServiceDescriptor;

namespace DIContainer.ActivatorEngine;

public abstract class BaseActivatorEngine: IActivatorEngine
{
    public Func<IScope, object> CreateActivator(ServiceDescriptor.ServiceDescriptor serviceDescriptor)
    {
        Debug.Assert(serviceDescriptor != null);

        switch (serviceDescriptor)
        {
            case FactoryServiceDescriptor factoryServiceDescriptor:
                return factoryServiceDescriptor.Factory;
            case ImplementationServiceDescriptor implementationServiceDescriptor:
                return _ => implementationServiceDescriptor;
            default:
                var descriptor = (TypeServiceDescriptor)serviceDescriptor;
                var ctor = descriptor.ImplementationType.GetConstructors(
                    BindingFlags.Public | BindingFlags.Instance).Single();
                var args = ctor.GetParameters();
                return CreateActivatorForTypeDescriptor(ctor, args);
        }
    }

    protected abstract Func<IScope, object> CreateActivatorForTypeDescriptor(ConstructorInfo ctor, ParameterInfo[] args);
}