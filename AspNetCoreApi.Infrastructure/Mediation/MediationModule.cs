﻿using System.Collections.Generic;
using System.Reflection;
using Autofac;
using FluentValidation;
using MediatR;
using MediatR.Pipeline;

namespace AspNetCoreApi.Infrastructure.Mediation
{
    /// <summary>
    /// Request Management Autofac Module
    /// </summary>
    public class MediationModule : Autofac.Module
    {
        private readonly IEnumerable<Assembly> mediatrAssemblies;

        /// <summary>
        /// Initializes a new instance of the <see cref="MediationModule"/> class
        /// </summary>
        /// <param name="mediatrAssemblies">Mediatr assemblies</param>
        public MediationModule(IEnumerable<Assembly> mediatrAssemblies)
        {
            this.mediatrAssemblies = mediatrAssemblies;
        }

        /// <summary>
        /// Registers RequestManagement components to dependency injection container
        /// </summary>
        /// <param name="builder">Container builder</param>
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterType<Mediator>()
                .As<IMediator>()
                .InstancePerLifetimeScope();

            var mediatrOpenTypes = new[]
            {
                typeof(IValidator<>),
                typeof(IRequestHandler<,>),
                typeof(INotificationHandler<>),
            };

            var assemblies = new List<Assembly>();
            assemblies.AddRange(this.mediatrAssemblies);
            assemblies.Add(typeof(OperationResult).Assembly);

            foreach (var assembly in assemblies)
            {
                foreach (var mediatrOpenType in mediatrOpenTypes)
                {
                    builder
                        .RegisterAssemblyTypes(assembly)
                        .AsClosedTypesOf(mediatrOpenType)
                        .AsImplementedInterfaces();
                }
            }

            // TODO: add fluent validation (https://docs.fluentvalidation.net/en/latest/aspnet.html)
            builder.RegisterGeneric(typeof(ValidationBehavior<,>)).As(typeof(IPipelineBehavior<,>)).WithMetadata("Order", -500);
            builder.RegisterGeneric(typeof(RequestPreProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>)).WithMetadata("Order", -100);
            builder.RegisterGeneric(typeof(RequestPostProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>)).WithMetadata("Order", 100);
        }
    }
}
