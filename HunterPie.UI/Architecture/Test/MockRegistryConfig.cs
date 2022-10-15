using HunterPie.Core.Domain.Interfaces;

namespace HunterPie.UI.Architecture.Test;
internal class MockRegistryConfig : ILocalRegistry
{
    public bool Exists(string name) => true;
    public object Get(string name) => "MOCK";
    public T Get<T>(string name) => default;
    public void Set<T>(string name, T value) { }
    public void Delete(string name) { }
}
