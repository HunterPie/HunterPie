using HunterPie.Core.Logger;
using HunterPie.Domain.Interfaces;
using System;

namespace HunterPie.Internal.Intializers
{
    internal class ExceptionCatcherInitializer : IInitializer
    {
        public void Init()
        {
            AppDomain.CurrentDomain.FirstChanceException += (_, args) =>
            {
                Log.Error(args.Exception);
            };
        }
    }
}
