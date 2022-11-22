using HunterPie.Core.Logger;
using System;

namespace HunterPie.Core.Architecture;

#nullable enable
public class SmartEvent<TSource, TEventArgs>
    where TSource : class
    where TEventArgs : EventArgs
{
    private readonly object _sync = new();
    private EventHandler<TEventArgs>? _event;

    public void Hook(EventHandler<TEventArgs> handler)
    {
        lock (_sync)
            _event += handler;
    }

    public void Unhook(EventHandler<TEventArgs> handler)
    {
        lock (_sync)
            _event -= handler;
    }

    public void Invoke(object? sender, TEventArgs e)
    {
        lock (_sync)
        {
            if (_event is null)
                return;

            foreach (EventHandler<TEventArgs> sub in _event.GetInvocationList())
                try
                {
                    sub(sender, e);
                }
                catch (Exception err)
                {
                    Log.Error("Exception in {0}: {1}", sub.Method.Name, err);
                }
        }
    }
}

public class SmartEvent<TEventArgs> : SmartEvent<object, TEventArgs> where TEventArgs : EventArgs { }