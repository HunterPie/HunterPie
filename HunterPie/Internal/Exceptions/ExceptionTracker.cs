using HunterPie.Core.Crypto;
using HunterPie.Core.Extensions;
using HunterPie.Core.Logger;
using HunterPie.Internal.Poogie;
using System;
using System.Collections.Generic;

namespace HunterPie.Internal.Exceptions;
internal class ExceptionTracker
{
    private static readonly Dictionary<string, ulong> Exceptions = new();

    public static void TrackException(Exception exception)
    {
        string exceptionHash = HashService.Hash(exception.Message);

        lock (Exceptions)
        {
            if (!Exceptions.ContainsKey(exceptionHash))
            {
                Exceptions.Add(exceptionHash, 0);

                exception.Also(SendException);
            }

            // TODO: Send this data to the API to keep track of unexpected errors
            Exceptions[exceptionHash]++;
        }
    }

    private static void SendException(Exception exception)
    {
        RemoteCrashReporter.Send(exception);
        Log.Error(exception.ToString());
    }
}
