using System;

namespace Suyeong.Lib.Type
{
    public interface IEllipse<T> : IShape<T> where T : IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
    {
        T MinX { get; }
        T MinY { get; }
        T MaxX { get; }
        T MaxY { get; }
        T CenterX { get; }
        T CenterY { get; }
        T MinorAxis { get; }
        T MajorAxis { get; }
        T Rotate { get; }
        T Area { get; }
    }
}
