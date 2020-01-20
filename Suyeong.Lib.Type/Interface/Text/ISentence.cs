using System.Collections.Generic;

namespace Suyeong.Lib.Type
{
    public interface ISentence<T> : IRect<T>
    {
        string Text { get; }
        IWord<T> StartWord { get; }
        IWord<T> EndWord { get; }
        IEnumerable<IWord<T>> Words { get; }
    }
}
