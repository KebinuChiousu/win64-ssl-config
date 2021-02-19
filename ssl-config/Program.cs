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

            bool newEntry = false;
            string key = "";
            List<string> entry = new List<string>();
            Dictionary<string, List<string>> entries = new Dictionary<string, List<string>>();

            foreach (string line in lines)
            {
                if (line.Contains(":port"))
                {
                    newEntry = true;
                    int pos = line.IndexOf(":");
                    pos++;
                    key = line.Substring(pos).Split(':')[1];
                    entry.Clear();
                    entry.Add(line);
                }
                else 
                { 
                    if (newEntry)
                    {
                        if (line == "")
                        {
                            entries.Add(key, entry);
                            newEntry = false;
                        } 
                        else
                        {
                            entry.Add(line);
                        }
                    }
                }
                
                Console.WriteLine(line);
            }

            Console.WriteLine("Press any key to close.");
            Console.ReadLine();
        }
    }
}
