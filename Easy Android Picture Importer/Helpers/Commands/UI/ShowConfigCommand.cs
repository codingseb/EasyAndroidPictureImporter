using System.Data;
using System.Windows;

namespace EasyAndroidPictureImporter.Helpers.Commands;

/// <summary>
/// This Command Show the config window
/// </summary>
public class ShowConfigCommand : CommandBase
{
    public override void Execute(object parameter)
    {
        Window parentWindow = parameter as Window;

        MessageBox.Show("Implement ConfigWindow", "Todo");

        //if ((parentWindow ??= App.Current.MainWindow) is not null)
        //{
        //    if (parentWindow.OwnedWindows.Cast<Window>().FirstOrDefault(window => window is ConfigWindow) is ConfigWindow configWindow)
        //    {
        //        configWindow.Show();
        //    }
        //    else
        //    {
        //        configWindow = new ConfigWindow()
        //        {
        //            Owner = parentWindow
        //        };

        //        configWindow.Show();
        //    }
        //}
        //else
        //{
        //    (new ConfigWindow()).Show();
        //}
    }
}