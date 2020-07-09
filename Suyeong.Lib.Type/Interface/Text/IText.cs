namespace Suyeong.Lib.Type
{
    public interface IText<T> : IRect<T>
    {
        Orientation Orientation { get; }
        T Rotate { get; }
        string Text { get; }
    }
}
