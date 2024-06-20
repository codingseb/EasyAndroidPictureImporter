namespace EasyAndroidPictureImporter.DependencyInjection;

/// <summary>
/// Implement this interface to have the method Init called at activation during DI Resolve
/// </summary>
public interface IInitializable
{
    /// <summary>
    /// Will be called just after object constuction by the DI
    /// </summary>
    void Init();
}