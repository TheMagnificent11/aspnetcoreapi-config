using System;
using System.Linq;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.Configuration;

namespace AspNetCoreApi.Infrastructure.Cors
{
    /// <summary>
    /// CORS Configuration
    /// </summary>
    public static class CorsConfiguration
    {
        /// <summary>
        /// Configures the CORS policy
        /// </summary>
        /// <param name="configuration">Configuration</param>
        /// <param name="policyBuilder">CORS policy builder</param>
        /// <param name="allowedOriginsSettingName">Name of the application setting that contains allowed origins (if null or empty, then all origins allowed)</param>
        /// <remarks>
        /// <paramref name="allowedOriginsSettingName"/> should contain origins separated by a semicolon
        /// </remarks>
        public static void ConfigureCorsPolicy(this IConfiguration configuration, CorsPolicyBuilder policyBuilder, string allowedOriginsSettingName)
        {
            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            if (policyBuilder is null)
            {
                throw new ArgumentNullException(nameof(policyBuilder));
            }

            var origins = (configuration.GetValue<string>(allowedOriginsSettingName) ?? string.Empty)
                .Split(';')
                .Distinct()
                .Where(x => !string.IsNullOrEmpty(x))
                .ToArray();

            policyBuilder = origins.Any() ? policyBuilder.WithOrigins(origins) : policyBuilder.AllowAnyOrigin();

            policyBuilder
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        }
    }
}