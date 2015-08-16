using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WebCrawler
{
    class WebCrawler
    {
        // WebProxy proxy = new WebProxy("http://proxy.internal:3128/", true);
        List<string> urlList = new List<string>();
        // Dictionary<string, string> 

        public static void Main(string[] args)
        {
            WebCrawler crawler = new WebCrawler();
            crawler.urlList.Add("http://tw.msn.com/");
            crawler.craw();
        }

        public void craw()
        {
            int urlIdx = 0;
            while (urlIdx < urlList.Count)
            {
                try
                {
                    string url = urlList[urlIdx];
                    string fileName = "data/" + toFileName(url);
                    Console.WriteLine(urlIdx + ":url=" + url + " file=" + fileName);
                    urlToFile(url, fileName);
                    string html = fileToText(fileName);
                    foreach (string childUrl in matches("\\shref\\s*=\\s*\"(.*?)\"", html, 1))
                    {
                        Console.WriteLine(childUrl);
                        urlList.Add(childUrl);
                    }
                }
                catch
                {
                    Console.WriteLine("Error:" + urlList[urlIdx] + " fail!");
                }
                urlIdx++;
            }
        }

        public static IEnumerable matches(string pPattern, string pText, int pGroupId)
        {
            Regex r = new Regex(pPattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
            for (Match m = r.Match(pText); m.Success; m = m.NextMatch())
                yield return m.Groups[pGroupId].Value;
        }

        public static string fileToText(string filePath)
        {
            StreamReader file = new StreamReader(filePath);
            string text = file.ReadToEnd();
            file.Close();
            return text;
        }

        public void urlToFile(string url, string file)
        {
            WebClient webclient = new WebClient();
            //        webclient.Proxy = proxy;
            webclient.DownloadFile(url, file);
        }

        public static string toFileName(string url)
        {
            string fileName = url.Replace('?', '_');
            fileName = fileName.Replace('/', '_');
            fileName = fileName.Replace('&', '_');
            fileName = fileName.Replace(':', '_');
            fileName = fileName.ToLower();
            if (!fileName.EndsWith(".htm") && !fileName.EndsWith(".html"))
                fileName = fileName + ".htm";
            return fileName;
        }
    }
}
