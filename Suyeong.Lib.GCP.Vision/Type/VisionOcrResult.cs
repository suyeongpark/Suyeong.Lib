using System.Collections.Generic;

namespace Suyeong.Lib.GCP.Vision
{
    public struct VisionOcrResult
    {
        public VisionOcrResult(int index, int x, int y, int width, int height, float score, string text)
        {
            this.Index = index;
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
            this.Score = score;
            this.Text = text;
        }

        public int Index { get; private set; }
        public int X { get; private set; }
        public int Y { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public float Score { get; private set; }
        public string Text { get; private set; }
    }

    public class VisionOcrResults : List<VisionOcrResult>
    {
        public VisionOcrResults()
        {

        }

        public VisionOcrResults(IEnumerable<VisionOcrResult> visionOcrResults) : base()
        {
            this.AddRange(visionOcrResults);
        }
    }
}
