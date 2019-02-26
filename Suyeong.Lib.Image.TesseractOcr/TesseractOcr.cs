using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Tesseract;

namespace Suyeong.Lib.Image.TesseractOcr
{
    public static class TesseractOcr
    {
        const float LIMIT_ACCURACY = 0.5f;
        const string TESSERACT_LANGUAGE = "eng";

        public static OcrPages GetOcrPages(string dataPath, IEnumerable<Bitmap> bitmaps)
        {
            OcrPages ocrPages = new OcrPages();

            if (Directory.Exists(dataPath))
            {
                using (TesseractEngine engine = new TesseractEngine(datapath: dataPath, language: TESSERACT_LANGUAGE, engineMode: EngineMode.TesseractAndCube))
                {
                    engine.DefaultPageSegMode = PageSegMode.SingleBlock;

                    int index = 0;

                    foreach (Bitmap bitmap in bitmaps)
                    {
                        ocrPages.Add(new OcrPage(index: index++, width: bitmap.Width, height: bitmap.Height, ocrTexts: ExtractOcrPage(bitmap: bitmap, engine: engine)));
                    }
                }
            }
            else
            {
                throw new NullReferenceException();
            }

            return ocrPages;
        }

        public static OcrPage GetOcrPage(string dataPath, Bitmap bitmap, int pageIndex = 0)
        {
            OcrPage ocrPage = new OcrPage();

            if (Directory.Exists(dataPath))
            {
                using (TesseractEngine engine = new TesseractEngine(datapath: dataPath, language: TESSERACT_LANGUAGE, engineMode: EngineMode.TesseractAndCube))
                {
                    engine.DefaultPageSegMode = PageSegMode.SingleBlock;

                    ocrPage = new OcrPage(index: pageIndex, width: bitmap.Width, height: bitmap.Height, ocrTexts: ExtractOcrPage(bitmap: bitmap, engine: engine));
                }
            }
            else
            {
                throw new NullReferenceException();
            }

            return ocrPage;
        }

        static OcrTexts ExtractOcrPage(Bitmap bitmap, TesseractEngine engine)
        {
            OcrTexts ocrTexts = new OcrTexts();

            int index = 0;
            string text;

            using (Page page = engine.Process(image: bitmap))
            using (ResultIterator iterator = page.GetIterator())
            {
                iterator.Begin();

                do
                {
                    Rect rect;

                    if (iterator.TryGetBoundingBox(level: PageIteratorLevel.Word, bounds: out rect))
                    {
                        float wordAccuracy = iterator.GetConfidence(PageIteratorLevel.Word);

                        if (wordAccuracy > LIMIT_ACCURACY)
                        {
                            text = iterator.GetText(PageIteratorLevel.Word).Trim();

                            if (!string.IsNullOrEmpty(text))
                            {
                                ocrTexts.Add(new OcrText(index: index++, x: rect.X1, y: rect.Y1, width: rect.Width, height: rect.Height, text: text));
                            }
                        }
                    }

                } while (iterator.Next(PageIteratorLevel.Word));
            }

            return ocrTexts;
        }
    }
}
