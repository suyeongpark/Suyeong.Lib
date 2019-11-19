using System.Collections.Generic;

namespace Suyeong.Lib.GoogleVision.OCR
{
    public struct OcrText
    {
        public OcrText(int index, int rotate, int x, int y, int width, int height, string text)
        {
            this.Index = index;
            this.Rotate = rotate;
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
            this.Text = text;
        }

        public int Index { get; private set; }
        public int Rotate { get; private set; }
        public int X { get; private set; }
        public int Y { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public string Text { get; private set; }
    }

    public class OcrTexts : List<OcrText>
    {
        public OcrTexts()
        {

        }

        public OcrTexts(IEnumerable<OcrText> ocrTexts) : base()
        {
            this.AddRange(ocrTexts);
        }
    }
}
