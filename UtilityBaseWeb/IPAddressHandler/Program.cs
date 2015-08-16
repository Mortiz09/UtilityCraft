using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace IPAddressHandler
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(IsInRange("1.1.1.10").ToString());
        }

        public static bool IsInRange(string address)
        {
            string startIpAddr = "1.1.1.1";
            string endIpAddr = "1.1.1.10";

            long ipStart = BitConverter.ToInt32(IPAddress.Parse(startIpAddr).GetAddressBytes().Reverse().ToArray(), 0);
            long ipEnd = BitConverter.ToInt32(IPAddress.Parse(endIpAddr).GetAddressBytes().Reverse().ToArray(), 0);
            long ip = BitConverter.ToInt32(IPAddress.Parse(address).GetAddressBytes().Reverse().ToArray(), 0);
            return ip >= ipStart && ip <= ipEnd;
        }

        public static bool IsInRange2(string address)
        {
            string startIpAddr = "1.1.1.1";
            string endIpAddr = "1.1.1.10";

            IPAddress ipStart = IPAddress.Parse(startIpAddr);
            IPAddress ipEnd = IPAddress.Parse(endIpAddr);
            IPAddress ip = IPAddress.Parse(address);

            return ip.Address >= ipStart.Address && ip.Address <= ipEnd.Address;
        }
    }
}
