using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography;
using System.Threading;
using CommandLine;
using ssl_config.App;

namespace ssl_config
{
    class Program
    {
        private static Application _app;
        
        static void Main(string[] args)
        {
            _app = new Application();

            var options = new Options();
            CommandLine.Parser.Default.ParseArguments<Options>(args)
                .WithParsed(RunOptions);

            _app.Run();

            Console.WriteLine("Press any key to close.");
            Console.ReadLine();
        }

        private static void RunOptions(Options opt)
        {
            if (!string.IsNullOrEmpty(opt.CertHash))
            {
                _app.Mode = FilterMode.CertHash;
            }

            if (_app.Mode == FilterMode.None)
            {
                if (!string.IsNullOrEmpty(opt.Port))
                {
                    _app.Mode = FilterMode.Port;
                }
            }

            _app.PartialMatch = opt.PartialMatch;
            _app.ReadOnly = opt.Read;

            switch (_app.Mode)
            {
                case FilterMode.CertHash:
                    _app.Value = opt.CertHash;
                    break;
                case FilterMode.Port:
                    _app.Value = opt.Port;
                    break;
                default:
                    _app.Value = "";
                    break;
            }
        }
    }
}
