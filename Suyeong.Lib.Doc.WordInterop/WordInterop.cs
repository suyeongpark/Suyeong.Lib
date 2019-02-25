using System;
using System.Runtime.InteropServices;
using Word = Microsoft.Office.Interop.Word;

namespace Suyeong.Lib.Doc.WordInterop
{
    public static class WordInterop
    {
        public static bool ExportToPdf(string wordPath, string pdfPath)
        {
            bool result = false;

            Word.Application application = null;
            Word.Document document = null;

            try
            {
                application = new Word.Application();
                document = application.Documents.Open(FileName: wordPath, ReadOnly: true);
                document.ExportAsFixedFormat(OutputFileName: pdfPath, ExportFormat: Word.WdExportFormat.wdExportFormatPDF, DocStructureTags: false);

                document.Close(SaveChanges: false);
                application.Quit(SaveChanges: 0);

                result = true;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                ReleaseComObjects(new object[] { document, application, });
            }

            return result;
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
