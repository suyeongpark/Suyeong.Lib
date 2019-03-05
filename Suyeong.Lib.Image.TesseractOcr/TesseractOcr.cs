using System;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using Tesseract;

namespace Suyeong.Lib.Image.TesseractOcr
{
    public static class TesseractOcr
    {
        const float LIMIT_ACCURACY = 0.5f;
        const string TESSERACT_LANGUAGE = "eng";
        const string REGEX_PATTERN_SPECIAL_WORD = "^[^0-9A-Za-z가-힣]{1,}$";

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

                            // 공백이거나 특수문자로 이루어진 텍스트는 처리하지 않는다.
                            if (!string.IsNullOrEmpty(text) && !Regex.IsMatch(text, REGEX_PATTERN_SPECIAL_WORD))
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
