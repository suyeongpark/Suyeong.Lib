using System.Collections.Generic;

namespace Suyeong.Lib.Doc.PdfSDK
{
    internal struct PdfTextRect
    {
        internal PdfTextRect(
            double ltX,
            double ltY,
            double lbX,
            double lbY,
            double rtX,
            double rtY,
            double rbX,
            double rbY
        )
        {
            this.LTX = ltX;
            this.LBX = lbX;
            this.RTX = rtX;
            this.RBX = rbX;
            this.LTY = ltY;
            this.LBY = lbY;
            this.RTY = rtY;
            this.RBY = rbY;

            this.CenterX = (this.LTX + this.RTX) * 0.5d;
            this.CenterY = (this.LTY + this.LBY) * 0.5d;
        }

        internal double LTX { get; private set; }
        internal double LTY { get; private set; }
        internal double LBX { get; private set; }
        internal double LBY { get; private set; }
        internal double RTX { get; private set; }
        internal double RTY { get; private set; }
        internal double RBX { get; private set; }
        internal double RBY { get; private set; }
        internal double CenterX { get; private set; }
        internal double CenterY { get; private set; }
    }

    internal class PdfTextRectCollection : List<PdfTextRect>
    {
        internal PdfTextRectCollection()
        {

        }

        internal PdfTextRectCollection(IEnumerable<PdfTextRect> pdfTexts) : base()
        {
            this.AddRange(pdfTexts);
        }
    }
}
