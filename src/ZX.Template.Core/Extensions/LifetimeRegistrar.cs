using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ZX.Template.Core.Extensions
{
    public interface IScoped;

    public interface ISingleton;

    public interface ITransient;

    public static class LifetimeRegistrar
    {
        public static IServiceCollection AddLifetimeService(this IServiceCollection services)
        {
            services
            .RegisterLifetimesByInhreit(typeof(IScoped))
            .RegisterLifetimesByInhreit(typeof(ISingleton))
            .RegisterLifetimesByInhreit(typeof(ITransient));

            return services;
        }

        public static IServiceCollection RegisterLifetimesByInhreit(this IServiceCollection services, Type lifetimeType)
        {
            //var assemblies1 = AppDomain.CurrentDomain.GetAssemblies();
            //var types1 = assemblies1
            //    .SelectMany(x => x.GetTypes())
            //    .Where(t => lifetimeType.IsAssignableFrom(t) && t.IsClass && !t.IsAbstract)
            //    .ToList();


            var types = GetAssemblies()
            .SelectMany(x => x.GetTypes())
            .Where(t => lifetimeType.IsAssignableFrom(t) && t.IsClass && !t.IsAbstract)
            .ToList();


            foreach (var type in types)
            {
                var interfaces = type.GetInterfaces().Where(t => t != lifetimeType);
                interfaces.ToList().ForEach(t =>
                {
                    if (lifetimeType == typeof(IScoped)) services.AddScoped(t, type);
                    else if (lifetimeType == typeof(ISingleton)) services.AddSingleton(t, type);
                    else if (lifetimeType == typeof(ITransient)) services.AddTransient(t, type);
                });
            }

            return services;
        }

        public static IEnumerable<Assembly> GetAssemblies()
        {
            var Assemblies = Assembly.GetEntryAssembly().GetReferencedAssemblies();
            foreach (var a in Assemblies)
            {
                var asm = Assembly.Load(a);
                yield return asm;
            }
            yield return Assembly.GetEntryAssembly();
        }
    }
}
