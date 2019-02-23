using System;
using System.Runtime.InteropServices;
using Microsoft.Office.Interop.Word;

namespace Suyeong.Lib.Doc.InteropWord
{
    public class DocWord
    {
        public static bool SaveAsWordToPdf(string wordPath, string pdfPath)
        {
            bool result = false;

            Application application = null;
            Document document = null;

            try
            {
                application = new Application();
                document = application.Documents.Open(FileName: wordPath, ReadOnly: true);
                document.ExportAsFixedFormat(OutputFileName: pdfPath, ExportFormat: WdExportFormat.wdExportFormatPDF, DocStructureTags: false);

                result = true;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (document != null)
                {
                    document.Close(SaveChanges: false);
                    ReleaseComObject(document);
                }

                if (application != null)
                {
                    application.Quit(SaveChanges: 0);
                    ReleaseComObject(application);
                }
            }

            return result;
        }

        static void ReleaseComObject(object obj)
        {
            try
            {
                if (obj != null)
                {
                    Marshal.ReleaseComObject(obj);
                    obj = null;
                }
            }
            catch (Exception)
            {
                obj = null;
                throw;
            }
            finally
            {
                GC.Collect();
            }
        }
    }
}
