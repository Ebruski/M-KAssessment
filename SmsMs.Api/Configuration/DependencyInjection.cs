using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Builder;
using SmsMs.Api.ServiceBusMessaging.Core;
using SmsMs.Api.ServiceBusMessaging.Interface;
using SmsMs.Api.ServiceBusMessaging.Services;

namespace SmsMs.Messaging
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddSmsApiConfigureServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddSingleton<IServiceBusSms, ServiceBusSms>();
            services.AddSingleton<ISendClientSmsProcessor, SendClientSmsProcessor>();

            return services;
        }

        public static IApplicationBuilder AddInfrastructureConfigure(this IApplicationBuilder app)
        {
            var bus = app.ApplicationServices.GetService<IServiceBusSms>();
            bus.RegisterOnMessageHandlerAndReceiveMessages();

            return app;
        }
    }
}
