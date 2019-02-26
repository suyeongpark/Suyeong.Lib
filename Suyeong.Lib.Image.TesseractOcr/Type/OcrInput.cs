using System.Collections.Generic;

namespace Suyeong.Lib.Image.TesseractOcr
{
    public struct OcrInput
    {
        public OcrInput(int index, int width, int height, string imagePath)
        {
            this.Index = index;
            this.Width = width;
            this.Height = height;
            this.ImagePath = imagePath;
        }

        public int Index { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public string ImagePath { get; private set; }
    }

    public class OcrInputs : List<OcrInput>
    {
        public OcrInputs()
        {

        }

        public OcrInputs(IEnumerable<OcrInput> ocrInputs) : base()
        {
            this.AddRange(ocrInputs);
        }
    }
}
