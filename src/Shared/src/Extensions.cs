﻿using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Types;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Shared
{
    public static class Extensions
    {
        private const string SectionName = "app";

        public static ISharedBuilder AddShared(this IServiceCollection services, string sectionName = SectionName)
        {
            var builder = SharedBuilder.Create(services);
            var options = builder.GetOptions<AppOptions>(sectionName);
            builder.Services.AddMemoryCache();
            services.AddSingleton(options);
            services.AddSingleton<IServiceId, ServiceId>();
            if (!options.DisplayBanner || string.IsNullOrWhiteSpace(options.Name))
            {
                return builder;
            }

            var version = options.DisplayVersion ? $" {options.Version}" : string.Empty;
            Console.WriteLine(Figgle.FiggleFonts.Doom.Render($"{options.Name}{version}"));

            return builder;
        }

        public static IApplicationBuilder UseShared(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var initializer = scope.ServiceProvider.GetRequiredService<IStartupInitializer>();
                Task.Run(() => initializer.InitializeAsync()).GetAwaiter().GetResult();
            }

            return app;
        }

        public static TModel GetOptions<TModel>(this IConfiguration configuration, string sectionName)
            where TModel : new()
        {
            var model = new TModel();
            configuration.GetSection(sectionName).Bind(model);
            return model;
        }

        public static TModel GetOptions<TModel>(this ISharedBuilder builder, string settingsSectionName)
            where TModel : new()
        {
            using var serviceProvider = builder.Services.BuildServiceProvider();
            var configuration = serviceProvider.GetService<IConfiguration>();
            return configuration.GetOptions<TModel>(settingsSectionName);
        }

        public static string Underscore(this string value)
            => string.Concat(value.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x : x.ToString()))
                .ToLowerInvariant();
    }
}
