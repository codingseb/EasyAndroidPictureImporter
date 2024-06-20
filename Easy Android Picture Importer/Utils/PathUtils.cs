using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace EasyAndroidPictureImporter.Utils;

/// <summary>
/// A common place to group io paths relative to the application and io utils functions
/// </summary>
public static class PathUtils
{
    /// <summary>
    /// The directory where the current application started
    /// </summary>
    public static string StartupPath => Path.GetDirectoryName((Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly()).Location);

    /// <summary>
    /// The directory of the application in AppData\Roaming
    /// </summary>
    public static string AppRoamingPath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Assembly.GetExecutingAssembly().GetName().Name);

    /// <summary>
    /// The directory of the application in AppData\Local
    /// </summary>
    public static string LocalPath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Assembly.GetExecutingAssembly().GetName().Name);

    /// <summary>
    /// The specific temp path of the app
    /// </summary>
    public static string TempPath => Path.Combine(Path.GetTempPath(), Assembly.GetExecutingAssembly().GetName().Name);

    /// <summary>
    /// Search for the specified file in the specified directory if not found go up a directory at a time until it reach the root path.
    /// </summary>
    /// <param name="startDirectory">The first directory where to start searching</param>
    /// <param name="fileName">The file name (without full path) of the file to find</param>
    /// <returns>If found return the directory path where it is found. It return null otherwise.</returns>
    public static string FindFileInDirectoryAncestors(string startDirectory, string fileName)
    {
        if (File.Exists(Path.Combine(startDirectory, fileName)))
        {
            return startDirectory;
        }
        else if (startDirectory.Equals(Path.GetPathRoot(startDirectory)))
        {
            return null;
        }
        else
        {
            return FindFileInDirectoryAncestors(Path.GetDirectoryName(startDirectory), fileName);
        }
    }

    /// <summary>
    /// Clean the specified filename of all invalids characters
    /// </summary>
    /// <param name="fileName">The file name to clean</param>
    /// <param name="replacement">The text to use to replace all invalids characters (by default : "_")</param>
    public static string GetSafeFilename(string fileName, string replacement = "_")
    {
        return string.Join(replacement, fileName.Split(Path.GetInvalidFileNameChars()));
    }
}