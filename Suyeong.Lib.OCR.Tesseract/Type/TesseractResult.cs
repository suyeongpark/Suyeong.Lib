using System.Collections.Generic;

namespace Suyeong.Lib.OCR.Tesseract
{
    public struct TesseractResult
    {
        public TesseractResult(int index, int x, int y, int width, int height, float accuracy, string text)
        {
            this.Index = index;
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
            this.Accuracy = accuracy;
            this.Text = text;
        }

        public int Index { get; private set; }
        public int X { get; private set; }
        public int Y { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public float Accuracy { get; private set; }
        public string Text { get; private set; }
    }

    public class TesseractResults : List<TesseractResult>
    {
        public TesseractResults()
        {

        }

        public TesseractResults(IEnumerable<TesseractResult> tesseractResults) : base()
        {
            this.AddRange(tesseractResults);
        }
    }
}
