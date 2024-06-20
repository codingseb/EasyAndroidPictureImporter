using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace EasyAndroidPictureImporter.ViewModel;

/// <summary>
/// To bubble up each changes in sub properties or observable collections in the root INotifyPropertyChanged
/// </summary>
public class NotifyPropertyChangedOnChildsChanges : ViewModelBase
{
    /// <summary>
    /// Called When a property changed, used to recursivelly subscribe or un subscribe to changes observation
    /// </summary>
    /// <param name="_">Not used here, hust put null in it</param>
    /// <param name="before">The value of the property before the change</param>
    /// <param name="after">The value of the property after the change</param>
    public void OnPropertyChanged(string _, object before, object after)
    {
        if (before is INotifyCollectionChanged beforeCollectionChanged)
        {
            beforeCollectionChanged.CollectionChanged -= OnCollectionChanged;

            if (after is IEnumerable enumerable)
            {
                foreach (var item in enumerable)
                {
                    OnPropertyChanged(null, item, null);
                }
            }
        }
        else if (before is INotifyPropertyChanged beforeObservable)
        {
            beforeObservable.PropertyChanged -= OnSubObjectPropertyChanged;
        }

        if(after is INotifyCollectionChanged afterCollectionChanged)
        {
            afterCollectionChanged.CollectionChanged += OnCollectionChanged;

            if (after is IEnumerable enumerable)
            {
                foreach (var item in enumerable)
                {
                    OnPropertyChanged(null, null, item);
                }
            }
        }
        else if (after is INotifyPropertyChanged afterObservable)
        {
            afterObservable.PropertyChanged += OnSubObjectPropertyChanged;
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

        if(e.NewItems != null)
        {
            foreach (var item in e.NewItems)
            {
                OnPropertyChanged(null, null, item);
            }
        }

        NotifyPropertyChanged(sender, "!CollectionChanged!");
    }

    private void OnSubObjectPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        NotifyPropertyChanged(sender, e);
    }
}