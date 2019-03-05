using System.Collections.Generic;

namespace Suyeong.Lib.Doc.PdfAcrobat
{
    public struct PdfText
    {
        public PdfText(int index, double leftX, double rightX, double topY, double bottomY, string text)
        {
            // pdf는 좌하단이 0,0
            this.Index = index;
            this.LeftX = leftX;
            this.RightX = rightX;
            this.TopY = topY;
            this.BottomY = bottomY;
            this.Width = rightX - leftX;
            this.Height = topY - bottomY;
            this.Text = text;
        }

        public int Index { get; private set; }
        public double LeftX { get; private set; }
        public double RightX { get; private set; }
        public double TopY { get; private set; }
        public double BottomY { get; private set; }
        public double Width { get; private set; }
        public double Height { get; private set; }
        public string Text { get; private set; }

        public static PdfText operator +(PdfText text1, PdfText text2)
        {
            int index = text1.Index < text2.Index ? text1.Index : text2.Index;
            double leftX = text1.LeftX < text2.LeftX ? text1.LeftX : text2.LeftX;
            double rightX = text1.RightX > text2.RightX ? text1.RightX : text2.RightX;
            double topY = text1.TopY > text2.TopY ? text1.TopY : text2.TopY;
            double bottomY = text1.BottomY < text2.BottomY ? text1.BottomY : text2.BottomY;
            string text = text1.Text + text2.Text;

            return new PdfText(index: index, leftX: leftX, rightX: rightX, topY: topY, bottomY: bottomY, text: text);
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
