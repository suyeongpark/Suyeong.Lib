using System;
using System.Collections.Generic;

namespace Suyeong.Lib.Type
{
    // 좀 일반화된 형태. 베지어 커브만이 아니라 그냥 연속된 점으로 이루어진 모든 curve를 의미한다.
    // is closed가 true이면 폐곡선, 아니면 개곡선이 된다.
    public interface ICurve<T> : IShape<T> where T : IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
    {
        bool IsClosed { get; }
        IPoint<T> StartPoint { get; }
        IPoint<T> EndPoint { get; }
        List<IPoint<T>> Points { get; }
    }
}
