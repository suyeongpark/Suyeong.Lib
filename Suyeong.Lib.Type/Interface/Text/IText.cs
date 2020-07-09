namespace Suyeong.Lib.Type
{
    public interface IText<T> : IRect<T>
    {
        Orientation Orientation { get; }
        int Rotate { get; }
        string Text { get; }
    }
}
