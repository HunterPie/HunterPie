using HunterPie.Core.Client;
using HunterPie.Core.Extensions;
using System;
using System.IO;
using System.Linq;

namespace HunterPie.Usecases;
internal class VerifyHunterPiePathUseCase
{

    public static bool Invoke()
    {
        string executablePath = ClientInfo.ClientPath;
        return !IsTemporaryPath(executablePath) && !HasArchiveAttribute(executablePath);
    }

    private static bool IsTemporaryPath(string path)
    {
        string[] tempIndicators = { "temp", "tmp" };
        string lowerPath = path.ToLowerInvariant();

        return tempIndicators.Any(indicator => lowerPath.Contains(indicator));
    }

    private static bool HasArchiveAttribute(string path)
    {
        try
        {
            FileAttributes attributes = File.GetAttributes(path);
            return attributes.HasFlag(FileAttributes.Temporary) || attributes.HasFlag(FileAttributes.Archive);
        }
        catch (Exception)
        {
            return true;
        }
    }

}