using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RepeatSectionSeeker
{
    class Program
    {
        static string filePath = @"C:\Dropbox\Novel\02 二字部\【廢土】黑天魔神Clear2.txt";
        static string outputPath = @"C:\Dropbox\Novel\02 二字部\【廢土】黑天魔神Clear2New.txt";

        public enum OutputChoice { CurrentFolder, SpecifyFolder }

        public struct DupItem
        {
            public int oglTop;
            public int oglEnd;
            public int rptTop;
            public int rptEnd;
        }

        static void Main(string[] args)
        {
            // Reference System.Configuration
            Regex rgx = new Regex(ConfigurationManager.AppSettings["IgnoreSymbol"]);

            List<string> oriContent = new List<string>();
            List<string> rgxContent = new List<string>();

            #region -- Read Line --
            StreamReader file = new StreamReader(filePath, Encoding.UTF8);

            string line = string.Empty;

            int index = 0;

            while ((line = file.ReadLine()) != null)
            {
                ++index;

                string rgxLine = rgx.Replace(line, "");

                rgxContent.Add(rgxLine);
            }
            #endregion

            //oriContent = File.ReadAllLines(filePath).ToList<string>();

            Dictionary<int, DupItem> dicDup = new Dictionary<int, DupItem>();

            var duplicates = rgxContent.Select((t, i) => new { Index = i, Text = t })
                                      .GroupBy(g => g.Text)
                                      .Where(g => g.Count() > 1)
                                      .OrderBy(g => g.FirstOrDefault().Index);

            int duplicateCount = duplicates.Count();

            for (int mainIndex = 0; mainIndex < duplicateCount; ++mainIndex)
            {
                if (dicDup.Count > 0 && mainIndex < dicDup.Last().Value.oglEnd)
                {
                    continue;
                }

                var item = duplicates.ElementAt(mainIndex);

                int fstIndex = item.FirstOrDefault().Index;
                int prvIndex = fstIndex;

                for (int subIndex = mainIndex + 1; subIndex < duplicateCount; ++subIndex)
                {
                    if (!(prvIndex.Equals(duplicates.ElementAt(subIndex).FirstOrDefault().Index - 1)))
                    {
                        continue;
                    }

                    if (dicDup.ContainsKey(fstIndex))
                    {
                        DupItem dup = dicDup[fstIndex];

                        dup.oglEnd = duplicates.ElementAt(subIndex).ElementAt(0).Index;
                        dup.rptEnd = duplicates.ElementAt(subIndex).ElementAt(1).Index;

                        dicDup[fstIndex] = dup;
                    }
                    else
                    {
                        DupItem dup = new DupItem();

                        dup.oglTop = duplicates.ElementAt(mainIndex).ElementAt(0).Index;
                        dup.rptTop = duplicates.ElementAt(mainIndex).ElementAt(1).Index;

                        dup.oglEnd = duplicates.ElementAt(subIndex).ElementAt(0).Index;
                        dup.rptEnd = duplicates.ElementAt(subIndex).ElementAt(1).Index;

                        dicDup[prvIndex] = dup;
                    }

                    ++prvIndex;
                }
            }

            #region -- Output --
            //StreamWriter newfile = new StreamWriter(CompleteOutputPath(OutputChoice.CurrentFolder, filePath));

            //newfile.WriteLine("\n");
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

        /// <summary>
        /// Remove White Space Line
        /// </summary>
        /// <param name="outputPath"></param>
        static void MakeClearFile(string outputPath)
        {
            List<string> oriContent = File.ReadAllLines(filePath).ToList<string>();

            List<string> result = oriContent.Where(s => !string.IsNullOrWhiteSpace(s)).ToList();

            File.WriteAllLines(outputPath, result);
        }
    }
}
