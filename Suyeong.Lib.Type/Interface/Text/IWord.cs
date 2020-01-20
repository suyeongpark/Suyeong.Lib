using System.Collections.Generic;

namespace Suyeong.Lib.Type
{
    public interface IWord<T> : IRect<T>
    {
        TextOrientation TextOrientation { get; }
        string Text { get; }
        IEnumerable<ICharacter<T>> Characters { get; }
    }
}
