using Autofac;
using EasyAndroidPictureImporter.ViewModel;

namespace EasyAndroidPictureImporter.DependencyInjection.DIExtensions;

/// <summary>
/// Add ViewModels to the DI
/// </summary>
public static class ViewModelsDIExtensions
{
    /// <summary>
    /// Add ViewModels to the DI
    /// </summary>
    /// <param name="builder">The DI Container builder on which to work</param>
    public static void RegisterViewModels(this ContainerBuilder builder)
    {
        builder.RegisterType<MainViewModel>()
            .AsSelf()
            .SingleInstance();
    }
}