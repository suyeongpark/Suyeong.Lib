using System.Collections.Generic;

namespace Suyeong.Lib.Doc.PdfSDK
{
    public struct PdfPage
    {
        public PdfPage(int index, int width, int height, int rotate, PdfTextCollection pdfTexts)
        {
            this.Index = index;
            this.Width = width;
            this.Height = height;
            this.Rotate = rotate;
            this.PdfTexts = pdfTexts;
        }

        public int Index { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int Rotate { get; private set; }
        public PdfTextCollection PdfTexts { get; private set; }
    }

    public class PdfPageCollection : List<PdfPage>
    {
        public PdfPageCollection()
        {

        }

        public PdfPageCollection(IEnumerable<PdfPage> pdfPages) : base()
        {
            this.AddRange(pdfPages);
        }
    }
}
