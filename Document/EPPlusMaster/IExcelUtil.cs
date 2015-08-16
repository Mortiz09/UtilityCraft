using System;
using System.IO;
using System.Data;

namespace UtilityCraft.ExcelUtil
{
    public interface IExcelUtil
    {
        //byte[] ExportToBytes(DataSet ds);
        //byte[] ExportToBytes(DataTable dt);
        //MemoryStream ExportToMemoryStream(DataSet ds);
        //MemoryStream ExportToMemoryStream(DataTable dt);

        DataSet ReadToDataSet(byte[] bytes);
        DataSet ReadToDataSet(FileInfo file);
        DataSet ReadToDataSet(Stream fileStream);

        DataSet ReadToDataSet(byte[] excelBytes, string[] sheetNames);
        DataSet ReadToDataSet(FileInfo excelFile, string[] sheetNames);
        DataSet ReadToDataSet(Stream excelFileStream, string[] sheetNames);

        DataTable ReadToDataTable(byte[] excelBytes, string sheetName);
        DataTable ReadToDataTable(FileInfo excelFile, string sheetName);
        DataTable ReadToDataTable(Stream excelFileStream, string sheetName);
    }
}
