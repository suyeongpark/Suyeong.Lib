using System;

namespace Suyeong.Lib.Type
{
    public interface IPoint<T> : IShape<T> where T : IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
    {
        T X { get; }
        T Y { get; }
    }
}
