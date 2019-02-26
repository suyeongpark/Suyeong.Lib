using System.Collections.Generic;

namespace Suyeong.Lib.Doc.PdfAcrobat
{
    public struct PdfText
    {
        public PdfText(int index, double x, double y, double width, double height, string text)
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

        public static PdfText operator +(PdfText text1, PdfText text2)
        {
            int index = text1.Index < text2.Index ? text1.Index : text2.Index;
            double x = text1.X < text2.X ? text1.X : text2.X;
            double y = text1.Y < text2.Y ? text1.Y : text2.Y;
            double width = text1.Width + text2.Width;
            double height = text1.Height + text2.Height;
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
