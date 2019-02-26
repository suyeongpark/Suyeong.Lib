using System.Collections.Generic;

namespace Suyeong.Lib.Image.TesseractOcr
{
    public struct OcrText
    {
        public OcrText(int index, double x, double y, double width, double height, string text)
        {
            this.Index = index;
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
            this.Text = text;
        }

        public int Index { get; private set; }
        public double X { get; private set; }
        public double Y { get; private set; }
        public double Width { get; private set; }
        public double Height { get; private set; }
        public string Text { get; private set; }
    }

    public class OcrTexts : List<OcrText>
    {
        public OcrTexts()
        {

        }

        public OcrTexts(IEnumerable<OcrText> pdfTexts) : base()
        {
            this.AddRange(pdfTexts);
        }
    }
}
