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
}