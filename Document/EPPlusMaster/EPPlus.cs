using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.IO;
using OfficeOpenXml;

namespace UtilityCraft.ExcelUtil
{
    public class EPPlusExcelUtil : IExcelUtil
    {
        private static EPPlusExcelUtil instance = new EPPlusExcelUtil();
        public static EPPlusExcelUtil GetInstance() { return instance; }
        public static EPPlusExcelUtil GetInstance(string name) { exportFilename = name; return instance; }

        public static string exportFilename = string.Empty;
        private static string firstSheet = string.Empty;

        /* Import to DataSet, DataTable */
        /// <summary>
        /// 讀取Excel中特定sheet to DataTable
        /// </summary>
        /// <param name="excelFileStream"></param>
        /// <param name="sheetName">excelFileStream</param>
        /// <returns></returns>
        public DataTable ReadToDataTable(string excelFilePath)   
        {
            return ReadToDataTable(excelFilePath, firstSheet);
        }
        public DataTable ReadToDataTable(Stream excelStream)     
        {
            return ReadToDataTable(excelStream, firstSheet);
        }
        public DataTable ReadToDataTable(byte[] excelBytes)      
        {
            return ReadToDataTable(excelBytes, firstSheet);
        }
        public DataTable ReadToDataTable(FileInfo excelFileInfo) 
        {
            return ReadToDataTable(excelFileInfo, firstSheet);
        }

        public DataTable ReadToDataTable(string excelFilePath, string sheetName)        
        {
            if (string.IsNullOrWhiteSpace(excelFilePath)) return null;

            if (File.Exists(excelFilePath))
            {
                DataTable result = new DataTable();

                using (FileStream fileStream = new FileStream(excelFilePath, FileMode.Open, FileAccess.Read))
                {
                    result = ReadToDataTable(fileStream, sheetName);
                }

                return result;
            }

            return null;
        }
        public DataTable ReadToDataTable(Stream excelStream, string specifiedSheetName) 
        {
            if (excelStream == null) return null;

            DataTable result = new DataTable();

            using (ExcelPackage package = new ExcelPackage(excelStream))
            {
                if (package.Workbook.Worksheets.Count > 0)
                {
                    ExcelWorksheet workSheet = null;

                    if (string.IsNullOrWhiteSpace(specifiedSheetName))
                    {
                        workSheet = package.Workbook.Worksheets.ElementAt(0);
                    }
                    else
                    {
                        //*
                        workSheet = (from sheet in package.Workbook.Worksheets
                                     where sheet.Name == specifiedSheetName
                                     select sheet
                                    ).SingleOrDefault();
                        /*/
                        foreach (ExcelWorksheet sheet in package.Workbook.Worksheets)
                        {
                            if (sheet.Name == specifiedSheetName)
                                workSheet = sheet;
                        }
                        //*/
                    }

                    int colCount = workSheet.Dimension.End.Column + 1;
                    int rowCount = workSheet.Dimension.End.Row + 1;

                    int colStartIndex = 1;
                    int rowStartIndex = 1;

                    // 建立 Columns
                    for (int colIndex = colStartIndex; colIndex < colCount; ++colIndex)
                    {
                        result.Columns.Add(new DataColumn(workSheet.Cells[colStartIndex, colIndex].Value.ToString()));
                    }

                    // 塞入資料
                    for (int rowIndex = rowStartIndex; rowIndex < rowCount; ++rowIndex)
                    {
                        DataRow dataRow = result.NewRow();

                        for (int colIndex = colStartIndex; colIndex < colCount; ++colIndex)
                        {
                            dataRow[colIndex - 1] = workSheet.Cells[rowIndex, colIndex].Value;
                        }

                        result.Rows.Add(dataRow);
                    }
                }
            }

            return result;
        }
        public DataTable ReadToDataTable(byte[] excelBytes, string sheetName)           
        {
            if (excelBytes == null || excelBytes.Length == 0) return null;

            MemoryStream excelMemoryStream = new MemoryStream();

            excelMemoryStream.Write(excelBytes, 0, excelBytes.Length);

            return ReadToDataTable(excelMemoryStream, sheetName);
        }
        public DataTable ReadToDataTable(FileInfo excelFileInfo, string sheetName)      
        {
            if (excelFileInfo == null) return null;

            return ReadToDataTable(excelFileInfo.OpenRead(), sheetName);
        }

        public DataSet ReadToDataSet(string excelFilePath, string[] sheetNames) 
        {
            if (File.Exists(excelFilePath))
            {
                DataSet result = new DataSet();

                using (FileStream fileStream = new FileStream(excelFilePath, FileMode.Open, FileAccess.Read))
                {
                    result = ReadToDataSet(fileStream, sheetNames);
                }

                return result;
            }

            return null;
        }
        public DataSet ReadToDataSet(Stream fileStream, string[] sheetNames)    
        {
            DataSet dataSet = new DataSet("ImportData");

            foreach (string sheet in sheetNames)
            {
                DataTable dtSheet = ReadToDataTable(fileStream, sheet);

                dataSet.Tables.Add(dtSheet.Copy());

                //DataTable dtTemp = dataSet.Tables.Add(sheet);

                //foreach (DataRow dr in dtSheet.Rows)
                //{
                //    dtTemp.ImportRow(dr);
                //}
            }

            return dataSet;
        }
        public DataSet ReadToDataSet(byte[] bytes, string[] sheetNames)         
        {
            if (bytes == null || bytes.Length == 0) return null;

            MemoryStream excelMemoryStream = new MemoryStream();

            excelMemoryStream.Write(bytes, 0, bytes.Length);

            return ReadToDataSet(excelMemoryStream, sheetNames);
        }
        public DataSet ReadToDataSet(FileInfo file, string[] sheetNames)        
        {
            if (file == null) return null;

            return ReadToDataSet(file.OpenRead(), sheetNames);
        }

        public DataSet ReadToDataSet(string filePath)  
        {
            if (!File.Exists(filePath)) return null;

            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            
            DataSet ds = ReadToDataSet(fileStream);
            
            fileStream.Close();

            return ds;
        }
        public DataSet ReadToDataSet(Stream fileStream)
        {
            DataSet dataSet = new DataSet("ImportData");

            using (ExcelPackage package = new ExcelPackage(fileStream))
            {

                if (package != null) return null;

                ExcelWorkbook workBook = package.Workbook;

                foreach (ExcelWorksheet sheet in workBook.Worksheets)
                {
                    if (sheet != null)
                    {
                        // 用sheet名稱建立 datatable
                        DataTable dtSheet = ReadToDataTable(fileStream, sheet.Name);

                        DataTable dtTemp = dataSet.Tables.Add(sheet.Name);

                        foreach (DataRow dr in dtSheet.Rows)
                        {
                            dtTemp.ImportRow(dr);
                        }
                    }
                }

                return dataSet;
            }
        }
        public DataSet ReadToDataSet(byte[] bytes)     
        {
            if (bytes.Length == 0) return null;

            MemoryStream theMemStream = new MemoryStream();

            theMemStream.Write(bytes, 0, bytes.Length);

            return ReadToDataSet(theMemStream);
        }
        public DataSet ReadToDataSet(FileInfo file)    
        {
            if (file == null) return null;

            return ReadToDataSet(file.OpenRead());
        }

        


        /* Export to Excel */
        public byte[] ExportDataTableToExcelBytes(DataTable dt)             
        {
            return ExportDataTableToExcelMemoryStream(dt).ToArray();
        }
        public MemoryStream ExportDataTableToExcelMemoryStream(DataTable dt)
        {
            DataSet ds = new DataSet();
            
            ds.Tables.Add(dt);

            return ExportDataSetToExcelMemoryStream(ds);
        }
        public byte[] ExportDataSetToExcelBytes(DataSet ds)                 
        {
            return ExportDataSetToExcelMemoryStream(ds).ToArray();
        }
        public MemoryStream ExportDataSetToExcelMemoryStream(DataSet ds)    
        {
            if (ds == null) return null;

            using (ExcelPackage package = new ExcelPackage())
            {
                foreach (DataTable dt in ds.Tables)
                {
                    ExcelWorksheet sheet = package.Workbook.Worksheets.Add(dt.TableName);

                    int colCount = dt.Columns.Count;

                    // 標題列
                    for (int col = 0; col < colCount; ++col)
                    {
                        sheet.Cells[1, col + 1].Value = dt.Columns[col].Caption;
                    }

                    // 資料內容
                    int rowNum = 2;
                    foreach (DataRow row in dt.Rows)
                    {
                        for (int col = 0; col < colCount; ++col)
                        {
                            if (row[col].GetType().Name == "DateTime")
                            {
                                sheet.Cells[rowNum, col + 1].Value = row[col].ToString();
                            }
                            else
                            {
                                sheet.Cells[rowNum, col + 1].Value = row[col];
                            }
                        }
                        ++rowNum;
                    }
                }

                MemoryStream ms = new MemoryStream();
                package.SaveAs(ms);

                return ms;
            } // using
        }

        public MemoryStream ExportSqlCommandToExcelMemoryStream(string sqlCommand)
        {
            DataSet ds = GetDataSet(sqlCommand);

            return ExportDataSetToExcelMemoryStream(ds);
        }
        public MemoryStream ExportToExcelMemoryStream(string[] sqlCommands, string[] sheetNames)
        {
            if (sqlCommands == null || sheetNames == null) return null;

            if (sqlCommands.Length != sheetNames.Length) return null;

            string filaname = (string.IsNullOrEmpty(exportFilename)) ? "ExportData" + DateTime.Now.ToString("_yyyy-MM-dd") : exportFilename;
            DataSet dataSet = new DataSet(filaname);

            int i = 0;
            for (i = 0; i < sqlCommands.Length; ++i)
            {
                // To Do
                //Database db = new SqlDatabase(connectionString);
                //DataTable dt = db.ExecuteDataSet(db.GetSqlStringCommand(sqlCommands[i])).Tables[0]; //.Copy();

                //dataSet.Tables.Add(dt.Copy());

                //dataSet.Tables[i].TableName = sheetNames[i];
            }

            return ExportDataSetToExcelMemoryStream(dataSet);
        }

        private string TransDateTimeToString(string datetime)
        {
            //先準備好一個 1900/1/1 的 DateTime
            DateTime dtDate = new DateTime(1900, 1, 1);

            //將數字字串以小數點做分隔符號, 拆成整數與小數
            string[] aryDateTime = datetime.Split('.');

            int intDays = int.Parse(aryDateTime[0]);

            //設定天數 (因為 1 表示 1900/1/1, 所以記得要減 1, 不然會多算一天)
            dtDate = dtDate.AddDays(intDays - 1);

            double dblSecond = 0d;

            if (aryDateTime.Length == 2)
            {
                float fltTime = float.Parse("0." + aryDateTime[1]);

                //如果要精準到毫秒, 可用 86400000, 不過後續要用 AddMilliseconds()
                //同理, 用 24 小時去計算時, 後續要用 AddHours()
                dblSecond = 86400 * fltTime;
                dtDate = dtDate.AddSeconds(dblSecond);
            }

            //以指定格式進行輸出
            return dtDate.ToString("yyyy/MM/dd HH:mm:ss");
        }

        // To Do
        private DataSet GetDataSet(string sqlCommand)
        {
            return new DataSet();
        }
    }
}
