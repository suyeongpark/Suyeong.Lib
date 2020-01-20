using System.Collections.Generic;

namespace Suyeong.Lib.Type
{
    public interface IPoly<T> : IDiagram<T>
    {
        IEnumerable<IPoint<T>> Points { get; }
    }
}
