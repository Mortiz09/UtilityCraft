using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RegexSolution
{
    class Program
    {
        static void Main(string[] args)
        {
            Regex rgx = new Regex("[，。“”！？—]");

            string result = rgx.Replace("ABC[D]EF!G", "");
        }
    }
}
