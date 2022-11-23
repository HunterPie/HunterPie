using HunterPie.Core.Logger;
using System.Collections.Generic;
using System.Linq;

namespace HunterPie.Core.Architecture.Events;
#nullable enable
internal class SmartEventsTracker
{
    private static SmartEventsTracker? _instance;
    public static SmartEventsTracker Instance => _instance ??= new SmartEventsTracker();

    public List<ISmartEvent> TrackedEvents { get; } = new();

    public static void Track(ISmartEvent smartEvent)
    {
        lock (Instance.TrackedEvents)
            Instance.TrackedEvents.Add(smartEvent);
    }

    public static void Untrack(ISmartEvent smartEvent)
    {
        lock (Instance.TrackedEvents)
            Instance.TrackedEvents.Remove(smartEvent);
    }

    public static int CountReferences() => Instance.TrackedEvents.Sum(se => se.References.Count);

    public static int ActiveEvents() => Instance.TrackedEvents.Count;

    public static void DisposeEvents()
    {
        foreach (ISmartEvent @event in Instance.TrackedEvents.ToArray())
        {
            if (@event.References.Any())
                @event.References.ToList().ForEach(reference =>
                        Log.Warn(
                        $"Detected dangling event reference at {reference.DeclaringType?.FullName ?? "unknown"}::{reference.Name}"
                        )
                    );

            @event.Dispose();
        }
    }
}
