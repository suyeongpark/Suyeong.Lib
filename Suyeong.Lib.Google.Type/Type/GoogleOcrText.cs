using System;
using System.Collections.Generic;

namespace Suyeong.Lib.Google.Type
{
    public struct GoogleOcrText
    {
        public GoogleOcrText(
            int index, 
            int rotate, 
            int leftX, 
            int rightX, 
            int topY, 
            int bottomY, 
            string text
        )
        {
            this.Index = index;
            this.Rotate = rotate;
            this.LeftX = leftX;
            this.RightX = rightX;
            this.TopY = topY;
            this.BottomY = bottomY;
            this.Width = this.RightX - this.LeftX;
            this.Height = this.BottomY - this.TopY;
            this.CenterX = (int)Math.Round((this.LeftX + this.RightX) * 0.5d, MidpointRounding.AwayFromZero);
            this.CenterY = (int)Math.Round((this.TopY + this.BottomY) * 0.5d, MidpointRounding.AwayFromZero);
            this.Text = text;
        }

        public int Index { get; private set; }
        public int Rotate { get; private set; }
        public int LeftX { get; private set; }
        public int RightX { get; private set; }
        public int TopY { get; private set; }
        public int BottomY { get; private set; }
        public int CenterX { get; private set; }
        public int CenterY { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public string Text { get; private set; }
    }

    public class GoogleOcrTextCollection : List<GoogleOcrText>
    {
        public GoogleOcrTextCollection()
        {

        }

        public GoogleOcrTextCollection(IEnumerable<GoogleOcrText> ocrTexts) : base()
        {
            this.AddRange(ocrTexts);
        }
    }

    public class GoogleOcrTextGroup : Dictionary<int, GoogleOcrTextCollection>
    {
        public GoogleOcrTextGroup()
        {

        }
    }
}
