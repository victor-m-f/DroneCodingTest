using DroneCodingTest.Application.UseCases.Base;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace DroneCodingTest.Application.Configuration;

public static class ApplicationConfiguration
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.InjectUseCases();
    }

    private static void InjectUseCases(this IServiceCollection services)
    {
        var useCaseInterfaceType = typeof(IRequestHandler<,>);
        var useCaseBaseType = typeof(UseCaseBase);

        foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
        {
            if (IsDerivedFromGenericType(type, useCaseBaseType))
            {
                var interfaceType = type.GetInterfaces().FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == useCaseInterfaceType);

                if (interfaceType != null)
                {
                    _ = services.AddScoped(interfaceType, type);
                }
            }
        }
    }

    private static bool IsDerivedFromGenericType(Type type, Type genericType)
    {
        while (type != null && type != typeof(object))
        {
            var typeWithGenericTypeDefinition = type.IsGenericType ? type.GetGenericTypeDefinition() : type;
            if (genericType == typeWithGenericTypeDefinition)
            {
                return true;
            }

            type = type.BaseType!;
        }

        return false;
    }
}
