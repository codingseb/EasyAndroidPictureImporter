using Autofac;
using EasyAndroidPictureImporter.Helpers.Commands;

namespace EasyAndroidPictureImporter.DependencyInjection.DIExtensions;

/// <summary>
/// Add Commands (ICommand) to the DI
/// </summary>
public static class CommandsDIExtensions
{
    /// <summary>
    /// Add Commands (ICommand) to the DI
    /// </summary>
    /// <param name="builder">The DI Container builder on which to work</param>
    public static void RegisteCommands(this ContainerBuilder builder)
    {
        builder.RegisterType<ShowConfigCommand>()
            .AsSelf()
            .SingleInstance();
}
}