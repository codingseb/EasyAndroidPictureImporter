using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.Metrics;

namespace EasyAndroidPictureImporter.ViewModel;

/// <summary>
/// To bubble up each changes in sub properties or observable collections in the root INotifyPropertyChanged
/// </summary>
public class NotifyPropertyChangedOnChildsChanges : ViewModelBase
{
    /// <summary>
    /// Called When a property changed, used to recursivelly subscribe or un subscribe to changes observation
    /// </summary>
    /// <param name="propertyName">The name of the property that changed</param>
    /// <param name="before">The value of the property before the change</param>
    /// <param name="after">The value of the property after the change</param>
    public void OnPropertyChanged(string propertyName, object before, object after)
    {
        Unsubcribe(before);
        Subcribe(after);
        NotifyPropertyChanged(propertyName);
    }

    private void Subcribe(object obj)
    {
        if (obj is IList enumerable)
        {
            foreach (var item in enumerable)
            {
                Subcribe(item);
            }

            if (obj is INotifyCollectionChanged notifyCollectionChanged)
            {
                notifyCollectionChanged.CollectionChanged += OnCollectionChanged;
            }
        }
        else if (obj is INotifyPropertyChanged notifyPropertyChanged)
        {
            notifyPropertyChanged.PropertyChanged += OnSubObjectPropertyChanged;
        }
    }

    private void Unsubcribe(object obj)
    {
        if (obj is IList enumerable)
        {
            foreach (var item in enumerable)
            {
                Unsubcribe(item);
            }

            if (obj is INotifyCollectionChanged notifyCollectionChanged)
            {
                notifyCollectionChanged.CollectionChanged -= OnCollectionChanged;
            }
        }
        else if (obj is INotifyPropertyChanged notifyPropertyChanged)
        {
            notifyPropertyChanged.PropertyChanged -= OnSubObjectPropertyChanged;
        }
    }

    private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.OldItems != null)
        {
            foreach (var item in e.OldItems)
            {
                OnPropertyChanged(null, item, null);
            }
        }

        if (e.NewItems != null)
        {
            foreach (var item in e.NewItems)
            {
                OnPropertyChanged(null, null, item);
            }
        }

        NotifyPropertyChanged("!CollectionChanged!");
    }

    private void OnSubObjectPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        NotifyPropertyChanged("!PropertyChanged!");
    }
}