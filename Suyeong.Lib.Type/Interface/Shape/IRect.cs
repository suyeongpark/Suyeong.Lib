using System;

namespace Suyeong.Lib.Type
{
    public interface IRect<T> : IShape<T> where T : IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
    {
        T MinX { get; }
        T MinY { get; }
        T MaxX { get; }
        T MaxY { get; }
        T CenterX { get; }
        T CenterY { get; }
        T Width { get; }
        T Height { get; }
        T Rotate { get; }
        T Area { get; }
    }
}
