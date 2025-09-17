using HunterPie.Core.Input;
using System;

namespace HunterPie.Features.Debug.Services;

internal class HotkeyServiceMock : IHotkeyService
{
    public int Register(string hotkey, Action callback)
    {
        return 0;
    }

    public void Unregister(int id) { }
}