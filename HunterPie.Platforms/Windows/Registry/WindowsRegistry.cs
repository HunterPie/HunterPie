using HunterPie.Core.Domain.Interfaces;
using Microsoft.Win32;
using WinRegistry = Microsoft.Win32.Registry;

namespace HunterPie.Platforms.Windows.Registry;

internal class WindowsRegistry : ILocalRegistry
{
    private readonly RegistryKey _key;

    public WindowsRegistry()
    {
        _key = WinRegistry.CurrentUser.CreateSubKey(@"SOFTWARE\HunterPie");
    }

    public Task SetAsync<T>(string name, T value) where T : class =>
        Task.Run(() => _key.SetValue(name, value));

    public Task<bool> ExistsAsync(string name) =>
        Task.Run(() => _key.GetValue(name) is not null);

    public Task<object?> GetAsync(string name) =>
        Task.Run(() => _key.GetValue(name));

    public Task<T?> GetAsync<T>(string name) where T : class =>
        Task.Run(() => Convert.ChangeType(_key.GetValue(name), typeof(T)) as T);

    public Task DeleteAsync(string name) =>
        Task.Run(() => _key.DeleteValue(name));
}