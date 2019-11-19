using System;
using System.Collections.Generic;
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
                    if (rotate == 90)
                    {
                        pdfTexts = GetPdfTexts90(pageIndex: i, pageRotate: rotate, pageHeight: height, jso: jso);
                    }
                    else if (rotate == 270)
                    {
                        pdfTexts = GetPdfTexts270(pageIndex: i, pageRotate: rotate, pageWidth: width, jso: jso);
                    }
                    else
                    {
                        pdfTexts = GetPdfTextsDefault(pageIndex: i, jso: jso);
                    }
                }
                catch (Exception)
                {
                    // 페이지가 많으면 중간에 뻗는다. 다시 살리기 위한 코드.
                    acroAvDoc = new AcroAVDoc();
                    acroAvDoc.Open(filePath, "");
                    acroPdDoc = acroAvDoc.GetPDDoc() as CAcroPDDoc;
                    jso = acroPdDoc.GetJSObject();

                    // 살린 후에 해당 페이지를 다시 돌린다.
                    if (rotate == 90)
                    {
                        pdfTexts = GetPdfTexts90(pageIndex: i, pageRotate: rotate, pageHeight: height, jso: jso);
                    }
                    else if (rotate == 270)
                    {
                        pdfTexts = GetPdfTexts270(pageIndex: i, pageRotate: rotate, pageWidth: width, jso: jso);
                    }
                    else
                    {
                        pdfTexts = GetPdfTextsDefault(pageIndex: i, jso: jso);
                    }
                }

                pdfPages.Add(new PdfPage(index: i, width: width, height: height, rotate: rotate, pdfTexts: pdfTexts));
            }

            return pdfPages;
        }

        static PdfTexts GetPdfTextsDefault(int pageIndex, object jso)
        {
            PdfTexts pdfTexts = new PdfTexts();

            Type type = jso.GetType();
            object jsNumWords = type.InvokeMember(JSO_GET_PAGE_NUM_WORDS, BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Instance, null, jso, new object[] { pageIndex }, null);
            int wordsCount = int.Parse(jsNumWords.ToString());

            object jsWord;
            object[] jsQuads;
            string text;
            double leftX, rightX, topY, bottomY;
            int rotate;

            for (int i = 0; i < wordsCount; i++)
            {
                jsWord = type.InvokeMember(JSO_GET_PAGE_NTH_WORDS, BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Instance, null, jso, new object[] { pageIndex, i, false }, null);
                text = jsWord.ToString().Trim();

                if (!string.IsNullOrWhiteSpace(text))
                {
                    jsQuads = type.InvokeMember(JSO_GET_PAGE_NTH_WORD_QUADS, BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Instance, null, jso, new object[] { pageIndex, i }, null) as object[];

                    FindPositionByHorizontal(jsQuads: jsQuads, leftX: out leftX, rightX: out rightX, topY: out topY, bottomY: out bottomY);
                    rotate = GetRotate(jsQuads: jsQuads, leftX: leftX, rightX: rightX, topY: topY, bottomY: bottomY, indexX: 0, indexY: 1);

                    pdfTexts.Add(new PdfText(index: pdfTexts.Count, rotate: rotate, leftX: leftX, rightX: rightX, topY: topY, bottomY: bottomY, text: text));
                }
            }

            return pdfTexts.Count > 0 ? UpdateBraket(oldTexts: pdfTexts) : pdfTexts;
        }

        static PdfTexts GetPdfTexts90(int pageIndex, int pageRotate, int pageHeight, object jso)
        {
            PdfTexts pdfTexts = new PdfTexts();

            Type type = jso.GetType();
            object jsNumWords = type.InvokeMember(JSO_GET_PAGE_NUM_WORDS, BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Instance, null, jso, new object[] { pageIndex }, null);
            int wordsCount = int.Parse(jsNumWords.ToString());

            object jsWord;
            object[] jsQuads;
            string text;
            double leftX, rightX, topY, bottomY, tempTopY, tempBottomY;
            int rotate;

            for (int i = 0; i < wordsCount; i++)
            {
                jsWord = type.InvokeMember(JSO_GET_PAGE_NTH_WORDS, BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Instance, null, jso, new object[] { pageIndex, i, false }, null);
                text = jsWord.ToString().Trim();

                if (!string.IsNullOrWhiteSpace(text))
                {
                    jsQuads = type.InvokeMember(JSO_GET_PAGE_NTH_WORD_QUADS, BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Instance, null, jso, new object[] { pageIndex, i }, null) as object[];

                    FindPositionByVertical(jsQuads: jsQuads, leftX: out leftX, rightX: out rightX, topY: out topY, bottomY: out bottomY);
                    rotate = GetRotate(jsQuads: jsQuads, leftX: leftX, rightX: rightX, topY: topY, bottomY: bottomY, indexX: 1, indexY: 0) - pageRotate;
                    rotate = rotate < 0 ? rotate + 360 : rotate;

                    // 90도 뒤집어진 경우엔 y축을 기준으로 뒤집는다.
                    tempTopY = pageHeight - topY;
                    tempBottomY = pageHeight - bottomY;
                    topY = tempBottomY;
                    bottomY = tempTopY;

                    // pdf는 좌하단이 (0, 0) 이므로 큰 x가 right, 큰 y가 top
                    pdfTexts.Add(new PdfText(index: pdfTexts.Count, rotate: rotate, leftX: leftX, rightX: rightX, topY: topY, bottomY: bottomY, text: text));
                }
            }

            return pdfTexts.Count > 0 ? UpdateBraket(oldTexts: pdfTexts) : pdfTexts;
        }

        static PdfTexts GetPdfTexts270(int pageIndex, int pageRotate, int pageWidth, object jso)
        {
            PdfTexts pdfTexts = new PdfTexts();

            Type type = jso.GetType();
            object jsNumWords = type.InvokeMember(JSO_GET_PAGE_NUM_WORDS, BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Instance, null, jso, new object[] { pageIndex }, null);
            int wordsCount = int.Parse(jsNumWords.ToString());

            object jsWord;
            object[] jsQuads;
            string text;
            double leftX, rightX, topY, bottomY, tempLeftX, tempRightX;
            int rotate;

            for (int i = 0; i < wordsCount; i++)
            {
                jsWord = type.InvokeMember(JSO_GET_PAGE_NTH_WORDS, BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Instance, null, jso, new object[] { pageIndex, i, false }, null);
                text = jsWord.ToString().Trim();

                if (!string.IsNullOrWhiteSpace(text))
                {
                    jsQuads = type.InvokeMember(JSO_GET_PAGE_NTH_WORD_QUADS, BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Instance, null, jso, new object[] { pageIndex, i }, null) as object[];

                    FindPositionByVertical(jsQuads: jsQuads, leftX: out leftX, rightX: out rightX, topY: out topY, bottomY: out bottomY);
                    rotate = GetRotate(jsQuads: jsQuads, leftX: leftX, rightX: rightX, topY: topY, bottomY: bottomY, indexX: 1, indexY: 0) - pageRotate;
                    rotate = rotate < 0 ? rotate + 360 : rotate;

                    // 270도 뒤집어진 경우엔 x축을 기준으로 뒤집는다.
                    tempLeftX = pageWidth - leftX;
                    tempRightX = pageWidth - rightX;
                    leftX = tempRightX;
                    rightX = tempLeftX;

                    // pdf는 좌하단이 (0, 0) 이므로 큰 x가 right, 큰 y가 top
                    pdfTexts.Add(new PdfText(index: pdfTexts.Count, rotate: rotate, leftX: leftX, rightX: rightX, topY: topY, bottomY: bottomY, text: text));
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
                    if (Math.Abs(current.BottomY - last.BottomY) < DISTANCE_SHORT && Math.Abs(current.LeftX - last.RightX) < DISTANCE_SHORT)
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

        static void FindPositionByHorizontal(object[] jsQuads, out double leftX, out double rightX, out double topY, out double bottomY)
        {
            leftX = double.MaxValue;
            rightX = double.MinValue;
            topY = double.MinValue;
            bottomY = double.MaxValue;

            int count;
            double value;

            // 세로인 경우 jsQuads가 여러개 나온다.
            foreach (object[] positionArr in jsQuads)
            {
                count = 0;

                foreach (object position in positionArr)
                {
                    value = double.Parse(position.ToString());

                    // 회전값이 0일때 짝수가 x, 홀수가 y
                    // 홀수
                    if (count == 1)
                    {
                        if (value < bottomY)
                        {
                            bottomY = value;
                        }

                        if (value > topY)
                        {
                            topY = value;
                        }

                        count = 0;
                    }
                    // 짝수
                    else
                    {
                        if (value < leftX)
                        {
                            leftX = value;
                        }

                        if (value > rightX)
                        {
                            rightX = value;
                        }

                        count++;
                    }
                }
            }
        }

        static void FindPositionByVertical(object[] jsQuads, out double leftX, out double rightX, out double topY, out double bottomY)
        {
            leftX = double.MaxValue;
            rightX = double.MinValue;
            topY = double.MinValue;
            bottomY = double.MaxValue;

            int count;
            double value;

            // 세로인 경우 jsQuads가 여러개 나온다.
            foreach (object[] positionArr in jsQuads)
            {
                count = 0;

                foreach (object position in positionArr)
                {
                    value = double.Parse(position.ToString());

                    // 회전값이 0일때 짝수가 y, 홀수가 x
                    // 홀수
                    if (count == 1)
                    {
                        if (value < leftX)
                        {
                            leftX = value;
                        }

                        if (value > rightX)
                        {
                            rightX = value;
                        }

                        count = 0;
                    }
                    // 짝수
                    else
                    {
                        if (value < bottomY)
                        {
                            bottomY = value;
                        }

                        if (value > topY)
                        {
                            topY = value;
                        }

                        count++;
                    }
                }
            }
        }

        static int GetRotate(object[] jsQuads, double leftX, double rightX, double topY, double bottomY, int indexX, int indexY)
        {
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

            object[] positionArr = jsQuads[0] as object[];
            double val0 = double.Parse(positionArr[0].ToString());
            double val2 = double.Parse(positionArr[2].ToString());
            double val6 = double.Parse(positionArr[6].ToString());

            // 0과 2가 같다는 것은 둘이 y이라는 뜻
            if (val0 == val2)
            {
                // 문서 자체가 회전된 경우에 잘 안맞는 것 같다. 위의 예시대로라면 val0이 val6 보다 크면 90도가 되어야 하는데 실제로는 안 그럼.
                if (val0 > val6)
                {
                    return 270;
                }
                else
                {
                    return 90;
                }
            }
            else
            {
                if (val0 > val2)
                {
                    return 180;
                }
                else
                {
                    return 0;
                }
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
