using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace CodeMaker
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, string> dicMap = new Dictionary<string, string>();

            dicMap["1"] = "One &lt; Two";
            dicMap["2"] = "Two &gt; One";
            dicMap["3"] = "Three &amp; Three";
            dicMap["4"] = "Four";
            dicMap["5"] = "Five";

            // *** Reference System.Web ***
            var temp = dicMap.ToDictionary(keyItem => keyItem.Key, valueItem => HttpUtility.HtmlDecode(valueItem.Value));

            foreach (var item in temp)
            {
                Console.WriteLine(item.Value);
            }

            Console.ReadKey();
        }
    }
}
