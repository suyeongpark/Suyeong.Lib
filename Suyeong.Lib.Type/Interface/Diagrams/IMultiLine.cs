﻿using System.Collections.Generic;

namespace Suyeong.Lib.Type
{
    public interface IMultiLine<T>
    {
        int Index { get; }
        ILine<T> StartLine { get; }
        ILine<T> EndLine { get; }
        IEnumerable<ILine<T>> Lines { get; }
        T Length { get; }
    }
}
