﻿namespace AspNetCoreApi.Infrastructure.Settings
{
    /// <summary>
    /// Application Settings
    /// </summary>
    public class ApplicationSettings
    {
        /// <summary>
        /// Gets or sets the application name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the application environment
        /// </summary>
        public string Environment { get; set; }

        /// <summary>
        /// Gets or sets the application version
        /// </summary>
        public string Version { get; set; }
    }
}
