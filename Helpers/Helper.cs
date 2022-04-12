using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BobaFileManager.Helpers
{
    public static class Helper
    {
        public static string GenerateRandomNumber(int length)
        {
            var rnd = new Random();
            return rnd.Next(Convert.ToInt32(Math.Pow(10, length - 1)), Convert.ToInt32(Math.Pow(10, length)) - 1).ToString();
        }

        public static string ShortenAddress(this string address)
        {
            if (address.Length < 20)
                return address;

            return address.Substring(0, 6) + "..." + address.Substring(address.Length - 4, 4);
        }
    }
}
