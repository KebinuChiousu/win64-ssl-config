using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ssl_config
{
    class Program
    {
        static void Main(string[] args)
        {

            string result = ExecuteCmd.Run("netsh http show sslcert");


            List<string> lines = result.Split(new[] { Environment.NewLine }, StringSplitOptions.None).ToList();

            List<string> list;
            Dictionary<string, List<string>> entries = new Dictionary<string, List<string>>();
            
            foreach (string line in lines)
            {
                Console.WriteLine(line);
            }

            Console.WriteLine("Press any key to close.");
            Console.ReadLine();
        }
    }
}
