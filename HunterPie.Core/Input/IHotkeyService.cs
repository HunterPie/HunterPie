using System;

namespace HunterPie.Core.Input;

public interface IHotkeyService
{
    public int Register(string hotkey, Action callback);
    public void Unregister(int id);
}