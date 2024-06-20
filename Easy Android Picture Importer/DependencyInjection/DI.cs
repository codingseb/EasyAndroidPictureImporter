﻿using Autofac;
using Autofac.Core.Resolving.Pipeline;
using EasyAndroidPictureImporter.DependencyInjection.DIExtensions;
using EasyAndroidPictureImporter.DependencyInjection.Middlewares;
using EasyAndroidPictureImporter.Utils;

namespace EasyAndroidPictureImporter.DependencyInjection;

/// <summary>
/// To specify how to build our application with dependency injection
/// and get the container
/// </summary>
public static class DI
{
    /// <summary>
    /// The container of DI structure
    /// </summary>
    public static IContainer Container { get; } = Build();

    private static IContainer Build()
    {
        var builder = new ContainerBuilder();

        // To add global middleware
        builder.ComponentRegistryBuilder.Registered += (sender, args) =>
        {
            args.ComponentRegistration.PipelineBuilding += (_, pipeline) =>
            {
                pipeline.Use(new OnActivationCallInitMiddleware());
                //pipeline.Use(new ConfigurableResolveMiddleware(PipelinePhase.Activation, context =>
                //{
                //    if(context.Instance is IInitializable initializable)
                //    {
                //        initializable.Init();
                //    }
                //}));
            };
        };

        // Register types here

        builder.RegisterType<MediaDeviceComparer>()
            .AsSelf()
            .SingleInstance();

        builder.RegisterViewModels();
        builder.RegisteCommands();

        return builder.Build();
    }
}
