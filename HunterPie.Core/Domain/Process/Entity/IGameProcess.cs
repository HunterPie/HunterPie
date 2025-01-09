using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Domain.Memory;
using HunterPie.Core.Game.Events;
using System;

namespace HunterPie.Core.Domain.Process.Entity;

public interface IGameProcess : IEventDispatcher
{
    public string Name { get; init; }
    public GameProcessType Type { get; init; }
    public IMemoryAsync Memory { get; init; }

    public event EventHandler<EventArgs>? Focus;
    public event EventHandler<EventArgs>? Blur;
    public event EventHandler<SimpleValueChangeEventArgs<ProcessStatus>>? StatusChange;
}