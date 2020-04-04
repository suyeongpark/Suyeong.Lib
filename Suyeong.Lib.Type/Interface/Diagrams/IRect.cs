namespace Suyeong.Lib.Type
{
    public interface IRect<T> : IDiagram<T>
    {
        T Width { get; }
        T Height { get; }
        T DiagonalSquare { get; }
    }
}
