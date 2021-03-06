﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CacheManager.Core;
using Datory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using SSCMS.Abstractions;
using SSCMS.Abstractions.Plugins;
using SSCMS.Core.Services.AuthManager;
using SSCMS.Core.Services.CreateManager;
using SSCMS.Core.Services.DatabaseManager;
using SSCMS.Core.Services.ParseManager;
using SSCMS.Core.Services.PathManager;
using SSCMS.Core.Services.PluginManager;
using SSCMS.Core.Services.SettingsManager;

namespace SSCMS.Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static ISettingsManager AddSettingsManager(this IServiceCollection services, IConfiguration configuration, string contentRootPath, string webRootPath)
        {
            var settingsManager = new SettingsManager(configuration, contentRootPath, webRootPath);
            services.TryAdd(ServiceDescriptor.Singleton<ISettingsManager>(settingsManager));

            return settingsManager;
        }

        public static void AddCache(this IServiceCollection services, string redisConnectionString)
        {
            services.AddCacheManagerConfiguration(async settings =>
            {
                var isBackPlane = false;
                if (!string.IsNullOrEmpty(redisConnectionString))
                {
                    var redis = new Redis(redisConnectionString);
                    var (isConnectionWorks, _) = await redis.IsConnectionWorksAsync();
                    if (isConnectionWorks)
                    {
                        settings
                            .WithMicrosoftMemoryCacheHandle()
                            .WithExpiration(ExpirationMode.None, TimeSpan.Zero)
                            .And
                            .WithRedisConfiguration("redis", config =>
                            {
                                if (!string.IsNullOrEmpty(redis.Password))
                                {
                                    config.WithPassword(redis.Password);
                                }
                                if (redis.AllowAdmin)
                                {
                                    config.WithAllowAdmin();
                                }

                                config
                                    .WithDatabase(redis.Database)
                                    .WithEndpoint(redis.Host, redis.Port);
                            })
                            .WithMaxRetries(1000)
                            .WithRetryTimeout(100)
                            .WithJsonSerializer()
                            .WithRedisBackplane("redis")
                            .WithRedisCacheHandle("redis");

                        isBackPlane = true;
                    }
                }

                if (!isBackPlane)
                {
                    settings
                        .WithMicrosoftMemoryCacheHandle()
                        .WithExpiration(ExpirationMode.None, TimeSpan.Zero);
                }
            });
            services.AddCacheManager();
        }

        public static void AddRepositories(this IServiceCollection services)
        {
            var baseType = typeof(IRepository);

            var types = AssemblyUtils.Assemblies
                .SelectMany(a => a.DefinedTypes)
                .Select(type => type.AsType())
                .Where(x => x != baseType && baseType.IsAssignableFrom(x)).ToArray();
            var implementTypes = types.Where(x => x.IsClass).ToArray();
            var interfaceTypes = types.Where(x => x.IsInterface).ToArray();
            foreach (var implementType in implementTypes)
            {
                var interfaceType = interfaceTypes.FirstOrDefault(x => x.IsAssignableFrom(implementType));
                if (interfaceType != null)
                {
                    //if (interfaceType == typeof(IContentRepository)) continue;

                    services.AddScoped(interfaceType, implementType);
                }
            }
        }

        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthManager, AuthManager>();
            services.AddScoped<IPathManager, PathManager>();
            services.AddScoped<ICreateManager, CreateManager>();
            services.AddScoped<IDatabaseManager, DatabaseManager>();
            services.AddScoped<IParseManager, ParseManager>();
            services.AddScoped<IPluginManager, PluginManager>();
            //services.AddScoped<IContentRepository, ContentRepository>();
        }

        public static void AddPlugins(this IServiceCollection services)
        {
            var baseType = typeof(IPlugin);
            var types = AssemblyUtils.Assemblies
                .SelectMany(a => a.DefinedTypes)
                .Select(type => type.AsType())
                .Where(x => x != baseType && baseType.IsAssignableFrom(x)).ToArray();
            var implementTypes = types.Where(x => !x.IsAbstract && x.IsClass).ToArray();
            var interfaceTypes = types.Where(x => x.IsInterface).ToArray();
            foreach (var implementType in implementTypes)
            {
                services.AddScoped(baseType, implementType);
                var interfaceType = interfaceTypes.FirstOrDefault(x => x.IsAssignableFrom(implementType));
                if (interfaceType != null && interfaceType != baseType)
                {
                    services.AddScoped(interfaceType, implementType);
                }
            }

            IServiceProvider provider = services.BuildServiceProvider();
            var logger = provider.GetService<ILoggerFactory>().CreateLogger<IServiceCollection>();

            foreach (var action in AssemblyUtils.GetInstances<IConfigureServices>())
            {
                logger.LogInformation("Executing ConfigureServices '{0}'", action.GetType().FullName);
                action.ConfigureServices(services, provider);
                provider = services.BuildServiceProvider();
            }
        }
    }
}