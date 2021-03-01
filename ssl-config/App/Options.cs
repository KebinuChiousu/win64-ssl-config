using CommandLine;

namespace ssl_config.App
{
    public class Options
    {
        [Option('m', "partialmatch", Required = false, HelpText = "Enable partial match")]
        public bool PartialMatch { get; set; }
        
        [Option('c', "certhash", Required = false, HelpText = "Filter by certificate hash")]
        public string CertHash { get; set; }

        [Option('p', "port", Required = false, HelpText = "Filter by port number")]
        public string Port { get; set; }

        [Option('r', "read", Required = false, HelpText = "Return parsed values")]
        public bool Read { get; set; }

    }
}