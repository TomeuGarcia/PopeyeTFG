namespace Popeye.Scripts.ValueGating
{
    public interface IGateValueReader<T>
    {
        T GetValue();
    }
}