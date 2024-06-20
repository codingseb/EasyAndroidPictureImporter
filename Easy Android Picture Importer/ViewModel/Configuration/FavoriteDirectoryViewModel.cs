namespace EasyAndroidPictureImporter.ViewModel;

public class FavoriteDirectoryViewModel(string rootPath, bool addEachChildAsFavorite = false)
    : NotifyPropertyChangedOnChildsChanges
{
    public string RootPath { get; set; } = rootPath;

    public bool Show { get; set; } = true;

    public bool AddEachChildAsFavorite { get; set; } = addEachChildAsFavorite;
}