using EasyAndroidPictureImporter.Utils;
using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;

namespace EasyAndroidPictureImporter;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        try
        {
            Directory.Delete(PathUtils.TempPath, true);
        }
        catch { }

        try
        {
            Directory.CreateDirectory(PathUtils.TempPath);
        }
        catch { }

        foreach (string oldFiles in Directory.GetFiles(PathUtils.TempPath))
        {
            try
            {
                File.Delete(oldFiles);
            }
            catch { }
        }

        base.OnStartup(e);
    }
}
