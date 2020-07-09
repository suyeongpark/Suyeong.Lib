using System;
using System.Runtime.InteropServices;
using System.Globalization;
using System.Reflection;
using System.Collections.Generic;
using Acrobat;
using Suyeong.Lib.Type;

namespace Suyeong.Lib.Doc.PdfSDK
{
    public static class PdfSDK
    {
        const string PD_DOC_PROG_ID = "AcroExch.PDDoc";
        const string JSO_GET_PAGE_NUM_WORDS = "getPageNumWords";
        const string JSO_GET_PAGE_NTH_WORDS = "getPageNthWord";
        const string JSO_GET_PAGE_NTH_WORD_QUADS = "getPageNthWordQuads";

        public static PdfPageCollection GetPdfPages(string filePath)
        {
            PdfPageCollection pages = new PdfPageCollection();

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
                        pages = GetPageTexts(pageCount: pageCount, filePath: filePath, acroPdDoc: ref acroPdDoc, acroAvDoc: ref acroAvDoc);
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

        public static PdfTextCollection RotatePdfTexts(
            IEnumerable<PdfText> pdfTexts,
            double pageWidth,
            double pageHeight,
            double pageRotate
        )
        {
            PdfTextCollection rotates = new PdfTextCollection();

            // XY축 대칭 + Y축 대칭
            if (pageRotate == 90d)
            {
                foreach (PdfText pdfText in pdfTexts)
                {
                    rotates.Add(new PdfText(
                        index: pdfText.Index,
                        groupIndex: pdfText.GroupIndex,
                        rotate: pdfText.Rotate,
                        orientation: pdfText.Orientation,
                        ltX: pdfText.LTY,
                        ltY: pageWidth - pdfText.LTX,
                        lbX: pdfText.LBY,
                        lbY: pageWidth - pdfText.LBX,
                        rtX: pdfText.RTY,
                        rtY: pageWidth - pdfText.RTX,
                        rbX: pdfText.RBY,
                        rbY: pageWidth - pdfText.RBX,
                        text: pdfText.Text
                    ));
                }
            }
            // X축 대칭 + Y축 대칭
            else if (pageRotate == 180d)
            {
                foreach (PdfText pdfText in pdfTexts)
                {
                    rotates.Add(new PdfText(
                        index: pdfText.Index,
                        groupIndex: pdfText.GroupIndex,
                        rotate: pdfText.Rotate,
                        orientation: pdfText.Orientation,
                        ltX: pageHeight - pdfText.LTX,
                        ltY: pageWidth - pdfText.LTY,
                        lbX: pageHeight - pdfText.LBX,
                        lbY: pageWidth - pdfText.LBY,
                        rtX: pageHeight - pdfText.RTX,
                        rtY: pageWidth - pdfText.RTY,
                        rbX: pageHeight - pdfText.RBX,
                        rbY: pageWidth - pdfText.RBY,
                        text: pdfText.Text
                    ));
                }
            }
            // XY축 대칭 + X축 대칭
            else if (pageRotate == 270d)
            {
                foreach (PdfText pdfText in pdfTexts)
                {
                    rotates.Add(new PdfText(
                        index: pdfText.Index,
                        groupIndex: pdfText.GroupIndex,
                        rotate: pdfText.Rotate,
                        orientation: pdfText.Orientation,
                        ltX: pageHeight - pdfText.LTY,
                        ltY: pdfText.LTX,
                        lbX: pageHeight - pdfText.LBY,
                        lbY: pdfText.LBX,
                        rtX: pageHeight - pdfText.RTY,
                        rtY: pdfText.RTX,
                        rbX: pageHeight - pdfText.RBY,
                        rbY: pdfText.RBX,
                        text: pdfText.Text
                    ));
                }
            }
            else
            {
                rotates = new PdfTextCollection(pdfTexts);
            }

            return rotates;
        }

        static PdfPageCollection GetPageTexts(int pageCount, string filePath, ref CAcroPDDoc acroPdDoc, ref AcroAVDoc acroAvDoc)
        {
            PdfPageCollection pdfPages = new PdfPageCollection();

            CAcroPDPage acroPdPage;
            int pageWidth, pageHeight, pageRotate;
            PdfTextCollection pdfTexts;
            object jso = acroPdDoc.GetJSObject();

            for (int i = 0; i < pageCount; i++)
            {
                acroPdPage = acroPdDoc.AcquirePage(i);
                pageWidth = acroPdPage.GetSize().x;
                pageHeight = acroPdPage.GetSize().y;
                pageRotate = acroPdPage.GetRotate();

                try
                {
                    pdfTexts = GetPdfTexts(pageIndex: i, jso: jso);
                }
                catch (Exception)
                {
                    // 페이지가 많으면 중간에 뻗는다. 다시 살리기 위한 코드.
                    acroAvDoc = new AcroAVDoc();
                    acroAvDoc.Open(filePath, "");
                    acroPdDoc = acroAvDoc.GetPDDoc() as CAcroPDDoc;
                    jso = acroPdDoc.GetJSObject();

                    // 살린 후에 해당 페이지를 다시 돌린다.
                    pdfTexts = GetPdfTexts(pageIndex: i, jso: jso);
                }

                pdfPages.Add(new PdfPage(index: i, width: pageWidth, height: pageHeight, rotate: pageRotate, pdfTexts: pdfTexts));
            }

            return pdfPages;
        }

        static PdfTextCollection GetPdfTexts(int pageIndex, object jso)
        {
            PdfTextCollection pdfTexts = new PdfTextCollection();

            System.Type type = jso.GetType();
            object jsNumWords = type.InvokeMember(JSO_GET_PAGE_NUM_WORDS, BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Instance, null, jso, new object[] { pageIndex }, null);
            int wordsCount = int.Parse(jsNumWords.ToString(), CultureInfo.InvariantCulture);

            object jsWord;
            object[] jsQuads;
            string text;
            int rotate, groupIndex = 0, index = 0;
            Orientation orientation;
            PdfTextRectCollection textRects;

            for (int i = 0; i < wordsCount; i++)
            {
                jsWord = type.InvokeMember(JSO_GET_PAGE_NTH_WORDS, BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Instance, null, jso, new object[] { pageIndex, i, false }, null);
                text = jsWord.ToString().Trim();

                if (!string.IsNullOrWhiteSpace(text))
                {
                    jsQuads = type.InvokeMember(JSO_GET_PAGE_NTH_WORD_QUADS, BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Instance, null, jso, new object[] { pageIndex, i }, null) as object[];

                    textRects = GetPdfTextRects(jsQuads: jsQuads);
                    rotate = GetRotate(textRects: textRects);
                    orientation = rotate == 0 ? Orientation.Horizontal : Orientation.Vertical;

                    foreach (PdfTextRect textRect in textRects)
                    {
                        pdfTexts.Add(new PdfText(
                            index: index++,
                            groupIndex: groupIndex,
                            rotate: rotate,
                            orientation: orientation,
                            ltX: textRect.LTX,
                            ltY: textRect.LTY,
                            lbX: textRect.LBX,
                            lbY: textRect.LBY,
                            rtX: textRect.RTX,
                            rtY: textRect.RTY,
                            rbX: textRect.RBX,
                            rbY: textRect.RBY,
                            text: text
                        ));
                    }

                    groupIndex++;
                }
            }

            return pdfTexts;
        }

        static PdfTextRectCollection GetPdfTextRects(object[] jsQuads)
        {
            PdfTextRectCollection textRects = new PdfTextRectCollection();

            foreach (object[] positionArr in jsQuads)
            {
                textRects.Add(new PdfTextRect(
                    ltX: double.Parse(positionArr[0].ToString(), CultureInfo.InvariantCulture),
                    ltY: double.Parse(positionArr[1].ToString(), CultureInfo.InvariantCulture),
                    lbX: double.Parse(positionArr[2].ToString(), CultureInfo.InvariantCulture),
                    lbY: double.Parse(positionArr[3].ToString(), CultureInfo.InvariantCulture),
                    rtX: double.Parse(positionArr[4].ToString(), CultureInfo.InvariantCulture),
                    rtY: double.Parse(positionArr[5].ToString(), CultureInfo.InvariantCulture),
                    rbX: double.Parse(positionArr[6].ToString(), CultureInfo.InvariantCulture),
                    rbY: double.Parse(positionArr[7].ToString(), CultureInfo.InvariantCulture)
                ));
            }

            return textRects;
        }

        static int GetRotate(PdfTextRectCollection textRects)
        {
            if (textRects.Count > 1)
            {
                PdfTextRect start = textRects[0];
                PdfTextRect end = textRects[textRects.Count - 1];

                double width = end.RTX - start.LTX;
                double height = end.LTY - start.LBY;

                // vertical --아주 극단적으로 가면 아닐 수도 있는데 일단 무시
                if (width < height)
                {
                    // 시작 글자가 아래에 
                    if (start.CenterY < end.CenterY)
                    {
                        return 90;
                    }
                    // 시작 글자가 위에 
                    else
                    {
                        return 270;
                    }
                }
            }

            return 0;
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
