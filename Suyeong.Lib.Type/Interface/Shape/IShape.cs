using System;

namespace Suyeong.Lib.Type
{
    public interface IShape<T> where T : IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
    {
        int Index { get; }
    }
}
