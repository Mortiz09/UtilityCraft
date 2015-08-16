using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppSettingReader
{
    class Program
    {
        static void Main(string[] args)
        {
            // *** Reference System.Configuration ***
            string test = ConfigurationManager.AppSettings["Test"].ToString();
        }
    }
}
