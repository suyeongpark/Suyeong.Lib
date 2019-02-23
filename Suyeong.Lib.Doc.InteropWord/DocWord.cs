using System;
using System.Runtime.InteropServices;
using Microsoft.Office.Interop.Word;

namespace Suyeong.Lib.Doc.InteropWord
{
    public class DocWord
    {
        public static bool ExportToPdf(string wordPath, string pdfPath)
        {
            bool result = false;

            Application application = null;
            Document document = null;

            try
            {
                application = new Application();
                document = application.Documents.Open(FileName: wordPath, ReadOnly: true);
                document.ExportAsFixedFormat(OutputFileName: pdfPath, ExportFormat: WdExportFormat.wdExportFormatPDF, DocStructureTags: false);

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
                if (document != null)
                {
                    Marshal.ReleaseComObject(document);
                }

                if (application != null)
                {
                    Marshal.ReleaseComObject(application);
                }
            }

            return result;
        }
    }
}
