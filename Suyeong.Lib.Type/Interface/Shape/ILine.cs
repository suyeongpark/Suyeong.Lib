using System;

namespace Suyeong.Lib.Type
{
    public interface ILine<T> : IShape<T> where T : IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
    {
        T StartX { get; }
        T StartY { get; }
        T EndX { get; }
        T EndY { get; }
        T MinX { get; }
        T MinY { get; }
        T MaxX { get; }
        T MaxY { get; }
        T CenterX { get; }
        T CenterY { get; }
        T DeltaX { get; }
        T DeltaY { get; }
        T LengthSquare { get; }
    }
}
