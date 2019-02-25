using System.Collections.Generic;

namespace Suyeong.Lib.Doc.PdfAcrobat
{
    public struct PdfText
    {
        public PdfText(int index, int x, int y, int width, int height, string text)
        {
            this.Index = index;
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
            this.Text = text;
        }

        public int Index { get; private set; }
        public int X { get; private set; }
        public int Y { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public string Text { get; private set; }

        public static PdfText operator +(PdfText text1, PdfText text2)
        {
            int index = text1.Index < text2.Index ? text1.Index : text2.Index;
            int x = text1.X < text2.X ? text1.X : text2.X;
            int y = text1.Y < text2.Y ? text1.Y : text2.Y;
            int width = text1.Width + text2.Width;
            int height = text1.Height + text2.Height;
            string text = text1.Text + text2.Text;

            return new PdfText(index: index, x: x, y: y, width: width, height: height, text: text);
        }
    }

    public class PdfTexts : List<PdfText>
    {
        public PdfTexts()
        {

        }

        public PdfTexts(IEnumerable<PdfText> pdfTexts) : base()
        {
            this.AddRange(pdfTexts);
        }
    }

}
