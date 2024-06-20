using System.Runtime.InteropServices;

namespace EasyAndroidPictureImporter.Interop;

/// <summary>
/// The POINT structure defines the x- and y-coordinates of a point.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
// ReSharper disable InconsistentNaming
public struct POINT
{
    public POINT(int x, int y)
    {
        X = x;
        Y = y;
    }
    /// <summary>
    /// Specifies the x-coordinate of the point.
    /// </summary>
    public int X;

    /// <summary>
    /// Specifies the y-coordinate of the point.
    /// </summary>
    public int Y;
}
