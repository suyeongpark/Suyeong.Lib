namespace Suyeong.Lib.Type
{
    public interface ICircle<T> : IDiagram<T>
    {
        T Radius { get; }
    }
}
