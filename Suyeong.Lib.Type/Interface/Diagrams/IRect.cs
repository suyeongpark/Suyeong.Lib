namespace Suyeong.Lib.Type
{
    public interface IRect<T> : IDiagram<T>
    {
        T X { get; }
        T Y { get; }
        T Width { get; }
        T Height { get; }
        T DiagonalSquare { get; }
    }
}
