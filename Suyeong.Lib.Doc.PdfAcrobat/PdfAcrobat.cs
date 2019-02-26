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

        public static PdfPages GetRawText(string filePath)
        {
            PdfPages pages = new PdfPages();

            AcroPDDoc acroPdDoc = null;

            try
            {
                acroPdDoc = Activator.CreateInstance(Type.GetTypeFromProgID(PD_DOC_PROG_ID)) as AcroPDDoc;

                if (acroPdDoc.Open(filePath))
                {
                    int pageCount = acroPdDoc.GetNumPages();

                    if (pageCount > 0)
                    {
                        pages = GetPdfPages(pageCount: pageCount, acroPdDoc: acroPdDoc);
                    }
                }

                acroPdDoc.Close();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                ReleaseComObjects(new object[] { acroPdDoc, });
            }

            return pages;
        }

        /// <summary>
        /// PDF 페이지 수가 많을 때 COM 오브젝트가 뻗는다. 강제로 돌리기 위한 메서드
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static PdfPages GetRawTextForce(string filePath)
        {
            PdfPages pages = new PdfPages();

            AcroPDDoc acroPdDoc = null;

            try
            {
                acroPdDoc = Activator.CreateInstance(Type.GetTypeFromProgID(PD_DOC_PROG_ID)) as AcroPDDoc;

                if (acroPdDoc.Open(filePath))
                {
                    int pageCount = acroPdDoc.GetNumPages();

                    if (pageCount > 0)
                    {
                        pages = GetPdfPagesForce(pageCount: pageCount, filePath: filePath, acroPdDoc: acroPdDoc);
                    }
                }

                acroPdDoc.Close();
            }
            catch (Exception)
            {
                // 강제로 할 때는 예외를 throw 하지 않는다.
            }
            finally
            {
                ReleaseComObjects(new object[] { acroPdDoc, });
            }

            return pages;
        }

        static PdfPages GetPdfPages(int pageCount, AcroPDDoc acroPdDoc)
        {
            PdfPages pdfPages = new PdfPages();

            AcroPDPage acroPdPage;
            int width, height, rotate;
            PdfTexts pdfTexts;
            object jso = acroPdDoc.GetJSObject();

            for (int i = 0; i < pageCount; i++)
            {
                acroPdPage = acroPdDoc.AcquirePage(i);
                width = acroPdPage.GetSize().x;
                height = acroPdPage.GetSize().y;
                rotate = acroPdPage.GetRotate();
                pdfTexts = GetPdfTexts(pageIndex: i, jso: jso);

                pdfPages.Add(new PdfPage(index: i, width: width, height: height, rotate: rotate, pdfTexts: pdfTexts));
            }

            return pdfPages;
        }

        static PdfPages GetPdfPagesForce(int pageCount, string filePath, AcroPDDoc acroPdDoc)
        {
            PdfPages pdfPages = new PdfPages();

            AcroPDPage acroPdPage;
            int width, height, rotate;
            PdfTexts pdfTexts;
            object jso = acroPdDoc.GetJSObject();

            for (int i = 0; i < pageCount; i++)
            {
                acroPdPage = acroPdDoc.AcquirePage(i);
                width = acroPdPage.GetSize().x;
                height = acroPdPage.GetSize().y;
                rotate = acroPdPage.GetRotate();

                try
                {
                    pdfTexts = GetPdfTexts(pageIndex: i, jso: jso);
                }
                catch (Exception)
                {
                    ReleaseComObjects(new object[] { acroPdDoc, });

                    // 페이지가 많으면 중간에 뻗는다. 다시 살리기 위한 코드.
                    acroPdDoc = new AcroPDDoc();
                    acroPdDoc = Activator.CreateInstance(Type.GetTypeFromProgID(PD_DOC_PROG_ID)) as AcroPDDoc;
                    acroPdDoc.Open(filePath);
                    jso = acroPdDoc.GetJSObject();

                    // 살린 후에 다시 돌린다.
                    pdfTexts = GetPdfTexts(pageIndex: i, jso: jso);
                }

                pdfPages.Add(new PdfPage(index: i, width: width, height: height, rotate: rotate, pdfTexts: pdfTexts));
            }

            return pdfPages;
        }

        static PdfTexts GetPdfTexts(int pageIndex, object jso)
        {
            PdfTexts pdfTexts = new PdfTexts();

            object jsNumWords, jsWord;
            object[] positionArr;
            dynamic jsQuads;
            int wordsCount;
            double leftX, rightX, topY, bottomY;
            string text;

            jsNumWords = jso.GetType().InvokeMember(JSO_GET_PAGE_NUM_WORDS, BindingFlags.InvokeMethod, null, jso, new object[] { pageIndex }, null);
            wordsCount = int.Parse(jsNumWords.ToString());

            for (int i = 0; i < wordsCount; i++)
            {
                jsWord = jso.GetType().InvokeMember(JSO_GET_PAGE_NTH_WORDS, BindingFlags.InvokeMethod, null, jso, new object[] { pageIndex, i, false }, null);
                text = jsWord.ToString().Trim();

                if (!string.IsNullOrWhiteSpace(text))
                {
                    jsQuads = jso.GetType().InvokeMember(JSO_GET_PAGE_NTH_WORD_QUADS, BindingFlags.InvokeMethod, null, jso, new object[] { pageIndex, i }, null);
                    positionArr = jsQuads[0];

                    leftX = double.Parse(positionArr[0].ToString());
                    topY = double.Parse(positionArr[1].ToString());
                    rightX = double.Parse(positionArr[2].ToString());
                    bottomY = double.Parse(positionArr[5].ToString());

                    // pdf는 좌하단이 0,0 이라서 x, y는 좌측, 하단을 기준으로 잡는다.
                    pdfTexts.Add(new PdfText(index: pdfTexts.Count, x: leftX, y: bottomY, width: rightX - leftX, height: topY - bottomY, text: text));
                }
            }

            return pdfTexts.Count > 0 ? UpdateBraket(oldTexts: pdfTexts) : pdfTexts;
        }

        // 시작 괄호 '(' 는 다음 글자와 분리되어 나오기 때문에 합치기 위한 로직
        static PdfTexts UpdateBraket(PdfTexts oldTexts)
        {
            PdfTexts newTexts = new PdfTexts();
            PdfText last, current;
            bool breketLeft = false;

            for (int i = 1; i < oldTexts.Count; i++)
            {
                last = oldTexts[i - 1];
                current = oldTexts[i];

                if (breketLeft)
                {
                    newTexts.Add(last + current);
                    breketLeft = false;
                }
                else if (char.Equals(current.Text[current.Text.Length - 1], '('))
                {
                    breketLeft = true;
                }
                else
                {
                    newTexts.Add(current);
                }
            }

            return newTexts;
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
