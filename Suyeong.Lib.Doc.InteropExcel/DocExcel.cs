using System;
using System.Data;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;

namespace Suyeong.Lib.Doc.InteropExcel
{
    public static class DocExcel
    {
        public static bool SaveExcelFromDataSet(DataSet dataSet, string filePath)
        {
            bool result = false;

            Excel.Application application = null;
            Excel.Workbook workbook = null;
            Excel.Worksheet worksheet = null;

            try
            {
                application = new Excel.Application();
                workbook = application.Workbooks.Add();

                for (int i = 0; i < dataSet.Tables.Count; i++)
                {
                    worksheet = workbook.Worksheets.Add();
                    result = SetWorksheet(worksheet: worksheet, table: dataSet.Tables[i]);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (worksheet != null)
                {
                    ReleaseExcelObject(worksheet);
                }

                if (workbook != null)
                {
                    workbook.SaveAs(filePath, Excel.XlFileFormat.xlWorkbookNormal);
                    workbook.Close(SaveChanges: false);
                    ReleaseExcelObject(workbook);
                }

                if (application != null)
                {
                    application.Quit();
                    ReleaseExcelObject(application);
                }
            }

            return result;
        }

        public static bool SaveExcelFromDataTable(DataTable table, string filePath)
        {
            bool result = false;

            Excel.Application application = null;
            Excel.Workbook workbook = null;
            Excel.Worksheet worksheet = null;

            try
            {
                application = new Excel.Application();
                workbook = application.Workbooks.Add();
                worksheet = workbook.Worksheets.Add();

                result = SetWorksheet(worksheet: worksheet, table: table);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (worksheet != null)
                {
                    ReleaseExcelObject(worksheet);
                }

                if (workbook != null)
                {
                    workbook.SaveAs(filePath, Excel.XlFileFormat.xlWorkbookNormal);
                    workbook.Close(SaveChanges: false);
                    ReleaseExcelObject(workbook);
                }

                if (application != null)
                {
                    application.Quit();
                    ReleaseExcelObject(application);
                }
            }

            return result;
        }

        public static DataSet GetDataSetFromExcel(string filePath)
        {
            DataSet dataSet = new DataSet();

            Excel.Application application = null;
            Excel.Workbook workbook = null;

            try
            {
                application = new Excel.Application();
                workbook = application.Workbooks.Open(Filename: filePath, ReadOnly: true);

                foreach (Excel.Worksheet workSheet in workbook.Worksheets)
                {
                    dataSet.Tables.Add(GetDataTableFromSheet(workSheet: workSheet));
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (workbook != null)
                {
                    workbook.Close(SaveChanges: false);
                    ReleaseExcelObject(workbook);
                }

                if (application != null)
                {
                    application.Quit();
                    ReleaseExcelObject(application);
                }
            }

            return dataSet;
        }

        public static DataTable GetDataTableFromExcel(string filePath, string sheetName)
        {
            DataTable table = new DataTable();

            Excel.Application application = null;
            Excel.Workbook workbook = null;

            try
            {
                application = new Excel.Application();
                workbook = application.Workbooks.Open(Filename: filePath, ReadOnly: true);

                table = GetDataTableFromSheet(workSheet: workbook.Worksheets[sheetName]);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (workbook != null)
                {
                    workbook.Close(SaveChanges: false);
                    ReleaseExcelObject(workbook);
                }

                if (application != null)
                {
                    application.Quit();
                    ReleaseExcelObject(application);
                }
            }

            return table;
        }

        static DataTable GetDataTableFromSheet(Excel.Worksheet workSheet)
        {
            DataTable table = new DataTable();

            table.TableName = workSheet.Name;
            Excel.Range range = workSheet.UsedRange;

            string[] values;
            object[,] value = range.Value;
            int rowCount = value.GetLength(0);
            int columnCount = value.GetLength(1);

            for (int i = 0; i < columnCount; i++)
            {
                table.Columns.Add(new DataColumn() { AllowDBNull = true });
            }

            for (int row = 0; row < rowCount; row++)
            {
                values = new string[columnCount];

                for (int column = 0; column < columnCount; column++)
                {
                    values[column] = value[row + 1, column + 1]?.ToString();
                }

                table.Rows.Add(values);
            }

            return table;
        }

        static bool SetWorksheet(Excel.Worksheet worksheet, DataTable table)
        {
            bool result = false;

            try
            {
                int rowCount = table.Rows.Count;
                int columnCount = table.Columns.Count;
                Excel.Range range = worksheet.Range[worksheet.Cells[1, 1], worksheet.Cells[rowCount, columnCount]];

                object[,] data = new object[rowCount, columnCount];

                for (int row = 0; row < rowCount; row++)
                {
                    for (int column = 0; column < columnCount; column++)
                    {
                        data[row, column] = table.Rows[row].ItemArray[column];
                    }
                }

                range.Value = data;

                result = true;
            }
            catch (Exception ex)
            {
                throw;
            }

            return result;
        }

        static void ReleaseExcelObject(object obj)
        {
            try
            {
                if (obj != null)
                {
                    Marshal.ReleaseComObject(obj);
                    obj = null;
                }
            }
            catch (Exception ex)
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
