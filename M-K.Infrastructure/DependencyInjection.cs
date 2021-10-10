using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http.Headers;
using M_KShared.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SmsMs.Application.Common.Interfaces;
using SmsMs.Infrastructure.Services.SmartSms.Core;

namespace SmsMs.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureConfigureServices(this IServiceCollection services, IConfiguration config)
        {
            //HttpContex
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddHttpClient("SmartSmsApi", client =>
            {
                client.BaseAddress = new Uri(AppSettingsManager.SmartSms("BaseUrl"));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });

            services.AddSingleton<ISmsService, SmartSmsService>();

            return services;
        }

        public static IApplicationBuilder AddInfrastructureConfigure(this IApplicationBuilder app)
        {
            return app;
        }
    }
}
