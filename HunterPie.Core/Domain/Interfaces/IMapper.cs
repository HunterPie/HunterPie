namespace HunterPie.Core.Domain.Interfaces;

public interface IMapper<T, K>
{
    public K Map(T data);
}
