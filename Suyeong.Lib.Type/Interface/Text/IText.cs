using System;

namespace Suyeong.Lib.Type
{
    public interface IText<T> : IRect<T> where T : IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
    {
        string Text { get; }
    }
}
