using MediaDevices;

namespace EasyAndroidPictureImporter.Utils;

/// <summary>
/// Extensions methods for Device IO Objects like <see cref="MediaDirectoryInfo"/> or <see cref="MediaFileInfo"/>
/// </summary>
public static class DeviceIOExtensions
{
    public static MediaDirectoryInfo GetDirectoryIfExists(this MediaDirectoryInfo root, string path)
    {
        if (root == null)
            return null;

        if(path.Length == 0)
            return root;

        string subDir = path.Split(@"\")[0];

        return root.EnumerateDirectories()
            .FirstOrDefault(directory => directory.Name.Equals(subDir, StringComparison.OrdinalIgnoreCase))
            .GetDirectoryIfExists(path.Substring(subDir.Length).TrimStart('\\'));
    }

    public static bool ContainsOneOf(this string source, List<string> texts, StringComparison stringComparison = StringComparison.OrdinalIgnoreCase)
    {
        return texts.Any(t => source.Contains(t, stringComparison));
    }
}