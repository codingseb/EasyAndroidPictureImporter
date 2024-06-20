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
        foreach (string oldFiles in Directory.GetFiles(Path.GetTempPath(), "EAPI*.*"))
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
