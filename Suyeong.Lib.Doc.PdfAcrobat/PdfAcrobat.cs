using System;
using System.Reflection;
using System.Runtime.InteropServices;
using Acrobat;

namespace Suyeong.Lib.Doc.PdfAcrobat
{
    public static class PdfAcrobat
    {
        const string PD_DOC_PROG_ID = "AcroExch.PDDoc";
        const string JSO_GET_PAGE_NUM_WORDS = "getPageNumWords";
        const string JSO_GET_PAGE_NTH_WORDS = "getPageNthWord";
        const string JSO_GET_PAGE_NTH_WORD_QUADS = "getPageNthWordQuads";

        const double DISTANCE_SHORT = 1d;

        public static PdfPages GetRawText(string filePath)
        {
            PdfPages pages = new PdfPages();

            AcroAVDoc acroAvDoc = null;
            CAcroPDDoc acroPdDoc = null;

            try
            {
                acroAvDoc = new AcroAVDoc();
                acroAvDoc.Open(filePath, "");

                if (acroAvDoc.IsValid())
                {
                    acroPdDoc = acroAvDoc.GetPDDoc() as CAcroPDDoc;

                    int pageCount = acroPdDoc.GetNumPages();

                    if (pageCount > 0)
                    {
                        pages = GetPdfPages(pageCount: pageCount, filePath: filePath, acroPdDoc: ref acroPdDoc, acroAvDoc: ref acroAvDoc);
                    }

                    acroPdDoc.Close();
                }
                else
                {
                    throw new InvalidOperationException();
                }

                acroAvDoc.Close(1);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                ReleaseComObjects(new object[] { acroAvDoc, acroPdDoc, });
                GC.Collect();
            }

            return pages;
        }

        static PdfPages GetPdfPages(int pageCount, string filePath, ref CAcroPDDoc acroPdDoc, ref AcroAVDoc acroAvDoc)
        {
            PdfPages pdfPages = new PdfPages();

            CAcroPDPage acroPdPage;
            int width, height, rotate, indexX1, indexX2, indexY1, indexY2;
            PdfTexts pdfTexts;
            object jso = acroPdDoc.GetJSObject();

            for (int i = 0; i < pageCount; i++)
            {
                acroPdPage = acroPdDoc.AcquirePage(i);
                width = acroPdPage.GetSize().x;
                height = acroPdPage.GetSize().y;
                rotate = acroPdPage.GetRotate();
                FindPositionIndex(rotate: rotate, indexX1: out indexX1, indexX2: out indexX2, indexY1: out indexY1, indexY2: out indexY2);

                try
                {
                    pdfTexts = GetPdfTexts(pageIndex: i, indexX1: indexX1, indexX2: indexX2, indexY1: indexY1, indexY2: indexY2, jso: jso);
                }
                catch (Exception)
                {
                    // 페이지가 많으면 중간에 뻗는다. 다시 살리기 위한 코드.
                    acroAvDoc = new AcroAVDoc();
                    acroAvDoc.Open(filePath, "");
                    acroPdDoc = acroAvDoc.GetPDDoc() as CAcroPDDoc;
                    jso = acroPdDoc.GetJSObject();

                    // 살린 후에 해당 페이지를 다시 돌린다.
                    pdfTexts = GetPdfTexts(pageIndex: i, indexX1: indexX1, indexX2: indexX2, indexY1: indexY1, indexY2: indexY2, jso: jso);
                }

                pdfPages.Add(new PdfPage(index: i, width: width, height: height, rotate: rotate, pdfTexts: pdfTexts));
            }

            return pdfPages;
        }

        static PdfTexts GetPdfTexts(int pageIndex, int indexX1, int indexX2, int indexY1, int indexY2, object jso)
        {
            PdfTexts pdfTexts = new PdfTexts();

            Type type = jso.GetType();
            object jsNumWords, jsWord;
            object[] jsQuads, positionArr;
            string text;
            double x1, x2, y1, y2;
            int wordsCount;

            jsNumWords = type.InvokeMember(JSO_GET_PAGE_NUM_WORDS, BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Instance, null, jso, new object[] { pageIndex }, null);
            wordsCount = int.Parse(jsNumWords.ToString());

            for (int i = 0; i < wordsCount; i++)
            {
                jsWord = type.InvokeMember(JSO_GET_PAGE_NTH_WORDS, BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Instance, null, jso, new object[] { pageIndex, i, false }, null);
                text = jsWord.ToString().Trim();

                if (!string.IsNullOrWhiteSpace(text))
                {
                    jsQuads = type.InvokeMember(JSO_GET_PAGE_NTH_WORD_QUADS, BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Instance, null, jso, new object[] { pageIndex, i }, null) as object[];
                    positionArr = jsQuads[0] as object[];

                    // 텍스트 자체가 회전된 경우가 있는데, 당장은 처리 못 함.
                    x1 = double.Parse(positionArr[indexX1].ToString());
                    x2 = double.Parse(positionArr[indexX2].ToString());
                    y1 = double.Parse(positionArr[indexY1].ToString());
                    y2 = double.Parse(positionArr[indexY2].ToString());

                    // pdf는 좌하단이 (0, 0) 이므로 큰 x가 right, 큰 y가 top
                    pdfTexts.Add(new PdfText(index: pdfTexts.Count, x1: x1, x2: x2, y1: y1, y2: y2, text: text));
                }
            }

            return pdfTexts.Count > 0 ? UpdateBraket(oldTexts: pdfTexts) : pdfTexts;
        }

        // 간격이 좁은 단어는 하나로 합친다.
        static PdfTexts UpdateBraket(PdfTexts oldTexts)
        {
            PdfTexts newTexts = new PdfTexts();
            PdfText last, current;

            if (oldTexts.Count > 0)
            {
                // 첫 인덱스 처리
                newTexts.Add(oldTexts[0]);

                for (int i = 1; i < oldTexts.Count; i++)
                {
                    current = oldTexts[i];
                    last = newTexts[newTexts.Count - 1];

                    // 같은 줄이고, 간격이 좁으면
                    if (Math.Abs(current.BottomY - last.BottomY) < DISTANCE_SHORT && current.LeftX - last.RightX < DISTANCE_SHORT)
                    {
                        // 마지막 것을 덮어쓴다.
                        newTexts[newTexts.Count - 1] = last + current;
                    }
                    else
                    {
                        newTexts.Add(current);
                    }
                }
            }

            return newTexts;
        }

        static void FindPositionIndex(int rotate, out int indexX1, out int indexX2, out int indexY1, out int indexY2)
        {
            indexX1 = indexX2 = indexY1 = indexY2 = -1;

            // 회전값 0일 때
            // 0 - LT X
            // 1 - LT Y
            // 2 - RT X
            // 3 - RT Y
            // 4 - LB X
            // 5 - LB Y
            // 6 - RB X
            // 7 - RB Y

            // 회전값 90 일 때                    
            // 0 - LT Y
            // 1 - LT X
            // 2 - RT Y
            // 3 - RT X
            // 4 - LB Y
            // 5 - LB X
            // 6 - RB Y
            // 7 - RB X

            // 회전값 180일 때
            // 0 - RB X
            // 1 - RB Y
            // 2 - LB X
            // 3 - LB Y
            // 4 - RT X
            // 5 - RT Y
            // 6 - LT X
            // 7 - LT Y

            // 회전값 270 일 때                    
            // 0 - RB Y
            // 1 - RB X
            // 2 - LB Y
            // 3 - LB X
            // 4 - RT Y
            // 5 - RT X
            // 6 - LT Y
            // 7 - LT X

            if (rotate == 90)
            {
                indexX1 = 1;
                indexX2 = 7;
                indexY1 = 0;
                indexY2 = 6;
            }
            else if (rotate == 180 || rotate == -180)
            {
                indexX1 = 6;
                indexX2 = 0;
                indexY1 = 7;
                indexY2 = 1;
            }
            else if (rotate == 270 || rotate == -90)
            {
                indexX1 = 7;
                indexX2 = 1;
                indexY1 = 6;
                indexY2 = 0;
            }
            else  // 0
            {
                indexX1 = 0;
                indexX2 = 6;
                indexY1 = 1;
                indexY2 = 7;
            }
        }

        static bool ReleaseComObjects(object[] objects)
        {
            bool result = false;

            try
            {
                foreach (object obj in objects)
                {
                    if (obj != null)
                    {
                        Marshal.ReleaseComObject(obj);
                    }
                }

                result = true;
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }
    }
}
