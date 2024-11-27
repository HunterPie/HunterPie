using HunterPie.Core.Client;
using HunterPie.Core.Extensions;
using System.IO;
using System.Linq;
using System;

namespace HunterPie.Usecases;
internal class VerifyHunterPiePathUseCase
{

    public static bool Invoke()
    {
        // Get the current execution directory
        string executablePath = AppDomain.CurrentDomain.BaseDirectory;

        // Check for temporary or unusual paths
        if (IsTemporaryPath(executablePath) || HasArchiveAttribute(executablePath))
        {
            return false;
        }

        return true;
    }

    private static bool IsTemporaryPath(string path)
    {
        // Check for patterns typical of temporary folders
        string[] tempIndicators = { "temp", "tmp" };
        string lowerPath = path.ToLowerInvariant();

        return tempIndicators.Any(indicator => lowerPath.Contains(indicator));
    }

    private static bool HasArchiveAttribute(string path)
    {
        try
        {
            // Get attributes of the directory
            FileAttributes attributes = File.GetAttributes(path);

            // Check if the directory or file has the Archive attribute
            return attributes.HasFlag(FileAttributes.Temporary) || attributes.HasFlag(FileAttributes.Archive);
        }
        catch (Exception)
        {
            return true;
        }
    }

}
