using System;

public static class OperatingSystem
{
    public static bool IsWindowsVersionAtLeast(int major, int minor = 0, int build = 0)
    {
        var os = Environment.OSVersion;
        if (os.Platform != PlatformID.Win32NT)
            return false;

        var current = new Version(os.Version.Major, os.Version.Minor, os.Version.Build);
        var target = new Version(major, minor, build);

        return current >= target;
    }
}