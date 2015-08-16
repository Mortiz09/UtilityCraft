using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileReader
{
    class Program
    {
        static string filePath = @"";
        static string outputPath = @"";
        public enum OutputChoice { CurrentFolder, SpecifyFolder }

        static void Main(string[] args)
        {
            string articleString = string.Empty;

            using (StreamReader file = new StreamReader(filePath, Encoding.Default))
            {
                #region -- Read by Line --
                string line = null;

                while ((line = file.ReadLine()) != null)
                {
                    Console.WriteLine(line);
                }
                #endregion

                #region -- Reade All Line --
                articleString = file.ReadToEnd();

                Console.WriteLine(articleString);
                #endregion
            }

            #region -- Output --
            StreamWriter newfile = new StreamWriter(CompleteOutputPath(OutputChoice.CurrentFolder, filePath));

            newfile.Write(articleString);
            newfile.WriteLine("\n");
            #endregion
        }

        static string CompleteOutputPath(OutputChoice outputType, string filePath)
        {
            #region -- Declare --
            string result = string.Empty;

            string fileName = Path.GetFileName(filePath);
            #endregion

            switch (outputType)
            {
                case OutputChoice.CurrentFolder:
                    result = fileName + "_New";
                    break;
                case OutputChoice.SpecifyFolder:
                    result = outputPath + "\\" + fileName;
                    break;
            }

            return result;
        }
    }
}
