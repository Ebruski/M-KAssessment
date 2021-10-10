using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace SmsMs.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationConfigureServices(this IServiceCollection services, IConfiguration config)
        {

            services.AddMediatR(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
