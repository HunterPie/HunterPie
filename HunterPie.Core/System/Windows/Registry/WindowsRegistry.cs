using HunterPie.Core.Domain.Interfaces;
using Microsoft.Win32;
using System;
using Regedit = Microsoft.Win32.Registry;

namespace HunterPie.Core.System.Windows.Registry;
internal class WindowsRegistry : ILocalRegistry
{
    private readonly RegistryKey _key;

    public WindowsRegistry()
    {
        _key = Regedit.CurrentUser.CreateSubKey(@"SOFTWARE\HunterPie");
    }

    public bool Exists(string name) => _key.GetValue(name) is not null;
    public object Get(string name) => _key.GetValue(name);
    public T Get<T>(string name) => (T)Convert.ChangeType(Get(name), typeof(T));
    public void Set<T>(string name, T value) => _key.SetValue(name, value);
    public void Delete(string name) => _key.DeleteValue(name);
}
