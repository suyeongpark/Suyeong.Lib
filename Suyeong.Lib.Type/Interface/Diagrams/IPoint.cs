namespace Suyeong.Lib.Type
{
    public interface IPoint<T>
    {
        int Index { get; }
        T X { get; }
        T Y { get; }
    }
}
