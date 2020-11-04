﻿using System;
using AspNetCoreApi.Infrastructure.Settings;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Core;

namespace AspNetCoreApi.Infrastructure.Logging
{
    /// <summary>
    /// Logging Configuration
    /// </summary>
    public static class LoggingConfiguration
    {
        /// <summary>
        /// <see cref="IServiceCollection"/> extension method to configure logging
        /// </summary>
        /// <param name="services">Services collection</param>
        /// <param name="applicationSettings">Application settings</param>
        /// <param name="seqSettings">Seq settings</param>
        public static void ConfigureLogging(
            this IServiceCollection services,
            ApplicationSettings applicationSettings,
            SeqSettings seqSettings)
        {
            if (applicationSettings is null)
            {
                throw new ArgumentNullException(nameof(applicationSettings));
            }

            if (seqSettings is null)
            {
                throw new ArgumentNullException(nameof(seqSettings));
            }

            var serilogLevelSwitch = new LoggingLevelSwitch
            {
                MinimumLevel = seqSettings.MinimumLogEventLevel
            };

            var config = new LoggerConfiguration()
                .MinimumLevel.ControlledBy(serilogLevelSwitch)
                .WriteTo.Trace()
                .WriteTo.Console()
                .Enrich.WithProperty("ApplicationName", applicationSettings.Name)
                .Enrich.WithProperty("Environment", applicationSettings.Environment)
                .Enrich.WithProperty("Version", applicationSettings.Version)
                .Enrich.WithProcessId()
                .Enrich.WithProcessName()
                .Enrich.WithMachineName()
                .Enrich.WithThreadId()
                .Enrich.FromLogContext();

            if (seqSettings.IsEnabled)
            {
                var seqLevelSwitch = new LoggingLevelSwitch
                {
                    MinimumLevel = seqSettings.MinimumLogEventLevel
                };

                config.WriteTo.Seq(seqSettings.Uri, apiKey: seqSettings.Key, controlLevelSwitch: seqLevelSwitch);
            }

            Log.Logger = config.CreateLogger();

#pragma warning disable Serilog004 // Constant MessageTemplate verifier
            Log.Logger.Information($"================= {applicationSettings.Name} Started =================");
#pragma warning restore Serilog004 // Constant MessageTemplate verifier

            services.AddLogging(builder => builder.AddSerilog(dispose: true));
        }
    }
}
