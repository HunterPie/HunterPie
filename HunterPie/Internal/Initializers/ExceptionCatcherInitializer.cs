using HunterPie.Core.Logger;
using HunterPie.Domain.Interfaces;
using System;

namespace HunterPie.Internal.Initializers
{
    internal class ExceptionCatcherInitializer : IInitializer
    {
        public void Init()
        {
            AppDomain.CurrentDomain.UnhandledException += (_, args) =>
            {
                Log.Error(args.ExceptionObject.ToString());
            };
        }
    }
}
