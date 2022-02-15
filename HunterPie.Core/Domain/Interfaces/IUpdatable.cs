namespace HunterPie.Core.Domain.Interfaces
{
    /// <summary>
    /// Interface for classes that can be updated from a data structure
    /// </summary>
    /// <typeparam name="T">The data structure to update from</typeparam>
    public interface IUpdatable<T> where T : struct
    {
        internal void Update(T data);
    }
}
