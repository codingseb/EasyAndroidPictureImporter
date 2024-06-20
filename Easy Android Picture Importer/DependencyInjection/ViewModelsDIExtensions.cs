using Autofac;
using EasyAndroidPictureImporter.ViewModel;

namespace EasyAndroidPictureImporter.DependencyInjection;

/// <summary>
/// Add ViewModels to the DI
/// </summary>
public static class ViewModelsDIExtensions
{
    public static void RegisterViewModels(this ContainerBuilder builder)
    {
        builder.RegisterType<MainViewModel>()
            .AsSelf()
            .SingleInstance();
    }
}