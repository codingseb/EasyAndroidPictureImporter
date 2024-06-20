using MediaDevices;
using System.Diagnostics.CodeAnalysis;

namespace EasyAndroidPictureImporter.Utils;

/// <summary>
/// To Compare to MediaDevice
/// </summary>
public class MediaDeviceComparer : IEqualityComparer<MediaDevice>
{
    /// <inheritdoc/>
    public bool Equals(MediaDevice x, MediaDevice y)
    {
        return x.DeviceId.Equals(y.DeviceId);
    }

    /// <inheritdoc/>
    public int GetHashCode([DisallowNull] MediaDevice obj)
    {
        return obj.DeviceId.GetHashCode();
    }
}