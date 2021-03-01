using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace ssl_config.App
{
    public static class Util
    {
        public static bool IsIPv4(string value)
        {
            var octets = value.Split('.');

            // if we do not have 4 octets, return false
            if (octets.Length!=4) return false;

            // for each octet
            foreach(var octet in octets) 
            {
                int q;
                // if parse fails 
                // or length of parsed int != length of octet string (i.e.; '1' vs '001')
                // or parsed int < 0
                // or parsed int > 255
                // return false
                if (!Int32.TryParse(octet, out q) 
                    || !q.ToString().Length.Equals(octet.Length) 
                    || q < 0 
                    || q > 255) { return false; }

            }

            return true;
        }

        public static object GetValue(string value)
        {

            switch (value)
            {
                case "Disabled":
                case "Not Set":
                    return false;
                case "Enabled":
                    return true;
                default:
                    return value;
            }
            
        }

        public static string GetEnabled(object value)
        {
            bool.TryParse(value.ToString(), out bool enable);
            string ret = enable ? "enable" : "disable";
            return ret;
        }

        public static string GetPort(NameValueCollection nvc)
        {
            string parm = nvc.GetKey(0);

            if (parm.Contains(":port"))
            {
                parm = nvc[0].Split(':')[1];
            }

            return parm;
        }

        public static SecureString GetPassword()
        {
            ConsoleKeyInfo keyInfo;
            var password = "";
            var ss = new SecureString();

            do
            {
                keyInfo = Console.ReadKey(true);
                // Skip if Backspace or Enter is Pressed
                if (keyInfo.Key != ConsoleKey.Backspace && keyInfo.Key != ConsoleKey.Enter)
                {
                    password += keyInfo.KeyChar;
                    Console.Write("*");
                }
                else
                {
                    if (keyInfo.Key == ConsoleKey.Backspace && password.Length > 0)
                    {
                        // Remove last charcter if Backspace is Pressed
                        password = password.Substring(0, (password.Length - 1));
                        Console.Write("\b \b");
                    }
                }
            }
            while (keyInfo.Key != ConsoleKey.Enter);

            Console.WriteLine("");

            foreach (char c in password)
            {
                ss.AppendChar(c);
            }

            return ss;
        }
    }
}
