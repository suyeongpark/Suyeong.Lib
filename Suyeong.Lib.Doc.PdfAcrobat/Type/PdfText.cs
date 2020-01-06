using System.Collections.Generic;

namespace Suyeong.Lib.Doc.PdfAcrobat
{
    public struct PdfText
    {
        public PdfText(int index, int rotate, double leftX, double rightX, double topY, double bottomY, string text)
        {
            this.Index = index;
            this.Rotate = rotate;
            this.LeftX = leftX;
            this.RightX = rightX;
            this.TopY = topY;
            this.BottomY = bottomY;
            this.Width = this.RightX - this.LeftX;
            this.Height = this.TopY - this.BottomY;
            this.CenterX = (this.LeftX + this.RightX) * 0.5d;
            this.CenterY = (this.TopY + this.BottomY) * 0.5d;
            this.Text = text;
        }

        public int Index { get; private set; }
        public int Rotate { get; private set; }
        public double LeftX { get; private set; }
        public double RightX { get; private set; }
        public double TopY { get; private set; }
        public double BottomY { get; private set; }
        public double CenterX { get; private set; }
        public double CenterY { get; private set; }
        public double Width { get; private set; }
        public double Height { get; private set; }
        public string Text { get; private set; }

        public static PdfText operator +(PdfText text1, PdfText text2)
        {
            // 둘의 회전이 동일하면 같은 것을 쓰고, 다르면 글자가 더 긴 것을 쓴다.
            int rotate = text1.Rotate == text2.Rotate ? text1.Rotate : (text1.Text.Length < text2.Text.Length ? text2.Rotate : text1.Rotate);

            int index = text1.Index <= text2.Index ? text1.Index : text2.Index;
            double leftX = text1.LeftX <= text2.LeftX ? text1.LeftX : text2.LeftX;
            double rightX = text1.RightX >= text2.RightX ? text1.RightX : text2.RightX;
            double topY = text1.TopY >= text2.TopY ? text1.TopY : text2.TopY;
            double bottomY = text1.BottomY <= text2.BottomY ? text1.BottomY : text2.BottomY;
            string text = text1.Text + text2.Text;

            return new PdfText(index: index, rotate: rotate, leftX: leftX, rightX: rightX, bottomY: bottomY, topY: topY, text: text);
        }
    }

    public class PdfTextCollection : List<PdfText>
    {
        public PdfTextCollection()
        {

        }

        public PdfTextCollection(IEnumerable<PdfText> pdfTexts) : base()
        {
            this.AddRange(pdfTexts);
        }
    }


}
