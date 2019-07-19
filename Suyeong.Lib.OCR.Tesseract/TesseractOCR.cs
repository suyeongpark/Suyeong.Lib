using System.Drawing;
using Tesseract;

namespace Suyeong.Lib.OCR.Tesseract
{
    public static class TesseractOCR
    {
        public static TesseractResults DetectTextsWithWord(string imagePath, string dataPath, string language)
        {
            TesseractResults results = new TesseractResults();

            using (TesseractEngine engine = new TesseractEngine(datapath: dataPath, language: language, engineMode: EngineMode.TesseractAndCube))
            {
                engine.DefaultPageSegMode = PageSegMode.SingleBlock;

                using (Bitmap bitmap = new Bitmap(imagePath))
                using (Page page = engine.Process(image: bitmap))
                using (ResultIterator iterator = page.GetIterator())
                {
                    PageIteratorLevel iteratorLevel = PageIteratorLevel.Word;

                    Rect rect;
                    int index = 0;
                    float accuracy;
                    string text;

                    iterator.Begin();

                    do
                    {
                        if (iterator.TryGetBoundingBox(level: iteratorLevel, bounds: out rect))
                        {
                            accuracy = iterator.GetConfidence(iteratorLevel);
                            text = iterator.GetText(iteratorLevel).Trim();
                            results.Add(new TesseractResult(index: index++, x: rect.X1, y: rect.Y1, width: rect.Width, height: rect.Height, accuracy: accuracy, text: text));
                        }

                    } while (iterator.Next(iteratorLevel));
                }
            }

            return results;
        }

        public static TesseractResults DetectTextsWithSymbol(string imagePath, string dataPath, string language)
        {
            TesseractResults results = new TesseractResults();

            using (TesseractEngine engine = new TesseractEngine(datapath: dataPath, language: language, engineMode: EngineMode.TesseractAndCube))
            {
                engine.DefaultPageSegMode = PageSegMode.SingleBlock;

                using (Bitmap bitmap = new Bitmap(imagePath))
                using (Page page = engine.Process(image: bitmap))
                using (ResultIterator iterator = page.GetIterator())
                {
                    PageIteratorLevel iteratorLevel = PageIteratorLevel.Symbol;

                    Rect rect;
                    int index = 0;
                    float accuracy;
                    string text;

                    iterator.Begin();

                    do
                    {
                        if (iterator.TryGetBoundingBox(level: iteratorLevel, bounds: out rect))
                        {
                            accuracy = iterator.GetConfidence(iteratorLevel);
                            text = iterator.GetText(iteratorLevel).Trim();
                            results.Add(new TesseractResult(index: index++, x: rect.X1, y: rect.Y1, width: rect.Width, height: rect.Height, accuracy: accuracy, text: text));
                        }

                    } while (iterator.Next(iteratorLevel));
                }
            }

            return results;
        }
    }
}
