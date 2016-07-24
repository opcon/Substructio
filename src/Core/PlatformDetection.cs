using System;
using System.IO;

namespace Substructio.Core
{
    /// <summary>
    /// Operating system platform
    /// </summary>
    public enum Platform
    {
        /// <summary>
        /// Windows
        /// </summary>
        Windows,

        /// <summary>
        /// Linux
        /// </summary>
        Linux,

        /// <summary>
        /// MacOSX
        /// </summary>
        MacOSX
    }

    /// <summary>
    ///     Platform detection code
    /// </summary>
    public static class PlatformDetection
    {
        /// <summary>
        ///     Checks which platform we are running on
        ///     Code adapted from here: https://stackoverflow.com/questions/10138040/how-to-detect-properly-windows-linux-mac-operating-systems
        /// </summary>
        /// <returns>The platform we are running on</returns>
        public static Platform RunningPlatform()
        {
            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Unix:
                    // Well, there are chances MacOSX is reported as Unix instead of MacOSX.
                    // Instead of platform check, we'll do a feature checks (Mac specific root folders)
                    if (Directory.Exists("/Applications")
                        & Directory.Exists("/System")
                        & Directory.Exists("/Users")
                        & Directory.Exists("/Volumes"))
                        return Platform.MacOSX;
                    else
                        return Platform.Linux;

                case PlatformID.MacOSX:
                    return Platform.MacOSX;

                default:
                    return Platform.Windows;
            }
        }

        /// <summary>
        /// Converts a Windows <see cref="Environment.OSVersion"/> version to 
        /// the corresponding friendly name release number
        /// </summary>
        /// <returns>The friendly version name</returns>
        public static string GetWindowsVersionFriendlyName()
        {
            switch (Environment.OSVersion.Version.Major)
            {
                case 5:
                    switch (Environment.OSVersion.Version.Minor)
                    {
                        case 0:
                            return "2000";
                        case 1:
                            return "XP";
                        case 2:
                            return "XP Professional x64";
                    }
                    break;
                case 6:
                    switch (Environment.OSVersion.Version.Minor)
                    {
                        case 0:
                            return "Vista";
                        case 1:
                            return "7";
                        case 2:
                            return "8";
                        case 3:
                            return "8.1";
                        case 4:
                            return "10";
                    }
                    break;
                case 10:
                    switch (Environment.OSVersion.Version.Minor)
                    {
                        case 0:
                            return "10";
                    }
                    break;
            }

            // Return empty string if we haven't matched a version
            return "";

        }
    }
}
