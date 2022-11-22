using HunterPie.Core.Architecture.Collections;
using System;
using System.Reflection;

namespace HunterPie.Core.Architecture.Events;

public interface ISmartEvent : IDisposable
{
    public ThreadSafeObservableCollection<MethodInfo> References { get; }
}
