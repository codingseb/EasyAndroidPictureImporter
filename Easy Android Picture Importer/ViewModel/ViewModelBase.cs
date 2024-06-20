using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EasyAndroidPictureImporter.ViewModel;

public class ViewModelBase : INotifyPropertyChanged
{
    /// <inheritdoc/>
    public event PropertyChangedEventHandler PropertyChanged;

    /// <summary>
    /// To notify that a property has changed on th ecurrent object
    /// </summary>
    /// <param name="propertyName">The name of the property that changed</param>
    public virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    /// <summary>
    /// To notify that a property has changed on the specified object
    /// </summary>
    /// <param name="sender">The object containing the property that changed</param>
    /// <param name="propertyName">The name of the property that changed</param>
    public virtual void NotifyPropertyChanged(object sender, string propertyName)
    {
        PropertyChanged(sender, new PropertyChangedEventArgs(propertyName));
    }

    /// <summary>
    /// To notify that a property has changed on the specified object
    /// </summary>
    /// <param name="sender">The object containing the property that changed</param>
    /// <param name="e">The event args of the change</param>
    public virtual void NotifyPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        PropertyChanged(sender, e);
    }
}