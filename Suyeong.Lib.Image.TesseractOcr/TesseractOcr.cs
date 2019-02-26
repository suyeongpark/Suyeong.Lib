using System;
using System.Drawing;
using System.IO;
using Tesseract;

namespace Suyeong.Lib.Image.TesseractOcr
{
    public static class TesseractOcr
    {
        const float LIMIT_ACCURACY = 0.5f;
        const string TESSERACT_LANGUAGE = "eng";

        public static OcrPageDic GetOcrPageDic(OcrInputs ocrInputs, string dataPath)
        {
            OcrPageDic ocrPageDic = new OcrPageDic();

            if (Directory.Exists(dataPath))
            {
                using (TesseractEngine engine = new TesseractEngine(datapath: dataPath, language: TESSERACT_LANGUAGE, engineMode: EngineMode.TesseractAndCube))
                {
                    engine.DefaultPageSegMode = PageSegMode.SingleBlock;

                    OcrTexts ocrTexts;

                    foreach (OcrInput ocrInput in ocrInputs)
                    {
                        ocrTexts = ExtractOcrTexts(imagePath: ocrInput.ImagePath, engine: engine);
                        ocrPageDic.Add(ocrInput.Index, new OcrPage(index: ocrInput.Index, width: ocrInput.Width, height: ocrInput.Height, ocrTexts: ocrTexts));
                    }
                }
            }
            else
            {
                throw new NullReferenceException();
            }

            return ocrPageDic;
        }

        public static OcrPages GetOcrPages(OcrInputs ocrInputs, string dataPath)
        {
            OcrPages ocrPages = new OcrPages();

            if (Directory.Exists(dataPath))
            {
                using (TesseractEngine engine = new TesseractEngine(datapath: dataPath, language: TESSERACT_LANGUAGE, engineMode: EngineMode.TesseractAndCube))
                {
                    engine.DefaultPageSegMode = PageSegMode.SingleBlock;

                    OcrTexts ocrTexts;

                    foreach (OcrInput ocrInput in ocrInputs)
                    {
                        ocrTexts = ExtractOcrTexts(imagePath: ocrInput.ImagePath, engine: engine);
                        ocrPages.Add(new OcrPage(index: ocrInput.Index, width: ocrInput.Width, height: ocrInput.Height, ocrTexts: ocrTexts));
                    }
                }
            }
            else
            {
                throw new NullReferenceException();
            }

            return ocrPages;
        }

        public static OcrPage GetOcrPage(OcrInput ocrInput, string dataPath)
        {
            OcrPage ocrPage = new OcrPage();

            if (Directory.Exists(dataPath))
            {
                using (TesseractEngine engine = new TesseractEngine(datapath: dataPath, language: TESSERACT_LANGUAGE, engineMode: EngineMode.TesseractAndCube))
                {
                    engine.DefaultPageSegMode = PageSegMode.SingleBlock;

                    OcrTexts ocrTexts = ExtractOcrTexts(imagePath: ocrInput.ImagePath, engine: engine);
                    ocrPage = new OcrPage(index: ocrInput.Index, width: ocrInput.Width, height: ocrInput.Height, ocrTexts: ocrTexts);
                }
            }
            else
            {
                throw new NullReferenceException();
            }

            return ocrPage;
        }

        static OcrTexts ExtractOcrTexts(string imagePath, TesseractEngine engine)
        {
            OcrTexts ocrTexts = new OcrTexts();

            int index = 0;
            string text;

            using (Bitmap bitmap = new Bitmap(imagePath))
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
