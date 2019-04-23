using System.Collections.Generic;

namespace Suyeong.Lib.Doc.PdfAcrobat
{
    public struct PdfText
    {
        public PdfText(int index, double x1, double x2, double y1, double y2, string text)
        {
            this.Index = index;

            // 글자 자체가 회전된 경우 문제가 되서 결국 여기서 left, right를 비교해줘야 함
            if (x1 < x2)
            {
                this.LeftX = x1;
                this.RightX = x2;
            }
            else
            {
                this.LeftX = x2;
                this.RightX = x1;
            }

            if (y1 < y2)
            {
                this.TopY = y2;
                this.BottomY = y1;
            }
            else
            {
                this.TopY = y1;
                this.BottomY = y2;
            }

            this.Width = this.RightX - this.LeftX;
            this.Height = this.TopY - this.BottomY;
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

            return new PdfText(index: index, x1: leftX, x2: rightX, y1: topY, y2: bottomY, text: text);
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
