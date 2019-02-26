using System.Collections.Generic;

namespace Suyeong.Lib.Image.TesseractOcr
{
    public struct OcrPage
    {
        public OcrPage(int index, int width, int height, OcrTexts ocrTexts)
        {
            this.Index = index;
            this.Width = width;
            this.Height = height;
            this.OcrTexts = ocrTexts;
        }

        public int Index { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public OcrTexts OcrTexts { get; private set; }
    }

    public class OcrPages : List<OcrPage>
    {
        public OcrPages()
        {

        }

        public OcrPages(IEnumerable<OcrPage> ocrPages) : base()
        {
            this.AddRange(ocrPages);
        }
    }

    public class OcrPageDic : Dictionary<int, OcrPage>
    {
        public OcrPageDic()
        {

        }
    }
}
