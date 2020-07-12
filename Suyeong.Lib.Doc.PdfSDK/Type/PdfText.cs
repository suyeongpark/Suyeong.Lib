using System.Collections.Generic;
using Suyeong.Lib.Type;

namespace Suyeong.Lib.Doc.PdfSDK
{
    public struct PdfText : IText<double>
    {
        public PdfText(
            int index,
            int groupIndex,
            Orientation orientation,
            double rotate,
            double ltX,
            double ltY,
            double lbX,
            double lbY,
            double rtX,
            double rtY,
            double rbX,
            double rbY,
            string text
        )
        {
            this.Index = index;
            this.GroupIndex = groupIndex;
            this.Rotate = rotate;
            this.Orientation = orientation;
            this.LTX = ltX;
            this.LBX = lbX;
            this.RTX = rtX;
            this.RBX = rbX;
            this.LTY = ltY;
            this.LBY = lbY;
            this.RTY = rtY;
            this.RBY = rbY;
            this.Text = text;

            this.MinX = this.LTX;
            this.MinY = this.LBY;
            this.MaxX = this.RTX;
            this.MaxY = this.RTY;

            if (this.MinX > this.LBX)
            {
                this.MinX = this.LBX;
            }

            if (this.MinX > this.RTX)
            {
                this.MinX = this.RTX;
            }

            if (this.MinX > this.RBX)
            {
                this.MinX = this.RBX;
            }

            if (this.MaxX < this.LTX)
            {
                this.MaxX = this.LTX;
            }

            if (this.MaxX < this.LBX)
            {
                this.MaxX = this.LBX;
            }

            if (this.MaxX < this.RBX)
            {
                this.MaxX = this.RBX;
            }

            if (this.MinY > this.LTY)
            {
                this.MinY = this.LTY;
            }

            if (this.MinY > this.RTY)
            {
                this.MinY = this.RTY;
            }

            if (this.MinY > this.RBY)
            {
                this.MinY = this.RBY;
            }

            if (this.MaxY < this.LTY)
            {
                this.MaxY = this.LTY;
            }

            if (this.MaxY < this.LBY)
            {
                this.MaxY = this.LBY;
            }

            if (this.MaxY < this.RBY)
            {
                this.MaxY = this.RBY;
            }

            this.CenterX = (this.MinX + this.MaxX) * 0.5d;
            this.CenterY = (this.MinY + this.MaxY) * 0.5d;

            this.X = this.MinY;
            this.Y = this.MinY;
            this.Width = this.MaxX - this.MinX;
            this.Height = this.MaxY - this.MinY;
            this.Area = this.Width * this.Height;
            this.DiagonalSquare = this.Width * this.Width + this.Height * this.Height;
        }

        public int Index { get; private set; }
        public int GroupIndex { get; private set; }
        public Orientation Orientation { get; private set; }
        public double Rotate { get; private set; }
        public double LTX { get; private set; }
        public double LTY { get; private set; }
        public double LBX { get; private set; }
        public double LBY { get; private set; }
        public double RTX { get; private set; }
        public double RTY { get; private set; }
        public double RBX { get; private set; }
        public double RBY { get; private set; }
        public double MinX { get; private set; }
        public double MinY { get; private set; }
        public double MaxX { get; private set; }
        public double MaxY { get; private set; }
        public double CenterX { get; private set; }
        public double CenterY { get; private set; }
        public double X { get; private set; }
        public double Y { get; private set; }
        public double Width { get; private set; }
        public double Height { get; private set; }
        public double DiagonalSquare { get; private set; }
        public double Area { get; private set; }
        public string Text { get; private set; }
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
