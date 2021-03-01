using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Security;
using System.Text;

namespace ssl_config.App
{
    public class Application
    {

        private IEnumerable<NameValueCollection> _settings; 
        
        #region "Properties"

        public bool PartialMatch { get; set; }
        
        public FilterMode Mode { get; set; }

        public string Value { get; set; } 

        public bool ReadOnly { get; set; }

        public string Domain {get; set; }

        public string User { get; set; }

        public SecureString Cred { get; set; }

        #endregion

        public void Run()
        {

            _settings = GetInfo();

            if (!ReadOnly)
                UpdateCerts();

            foreach (NameValueCollection nvc in _settings)
            {
                PrintCert(nvc);
            }
        }

        #region "Retrieve TLS Cert Info"

        private IEnumerable<NameValueCollection> GetInfo()
        {
            List<NameValueCollection> ret = new List<NameValueCollection>();
            IEnumerable<NameValueCollection> entries = GetCertInfo();

            foreach (NameValueCollection nvc in entries)
            {
                if (FilterValue(nvc))
                {
                    ret.Add(nvc);
                }
            }

            return ret;
        }

        private bool FilterValue(NameValueCollection nvc)
        {
            string parm;
            string[] values = Value.Split(new char[] { ',', ';', ':' });
            
            switch (Mode)
            {
                case FilterMode.CertHash:
                    parm = nvc[TlsCertParms.CertHash];
                    break;
                case FilterMode.Port:
                    parm = Util.GetPort(nvc);
                    break;
                default:
                    return true;
            }

            if (PartialMatch)
            {
                foreach(string val in values)
                {
                    if (parm.Contains(val))
                    {
                        return true;
                    }
                }
            }
            else
            {
                foreach (string val in values)
                {
                    if (parm == val)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private static IEnumerable<NameValueCollection> GetCertInfo()
        {
            string result = ExecuteCmd.Run("netsh http show sslcert");
            
            var lines = result.Split(new[] { Environment.NewLine }, StringSplitOptions.None).ToList();
            
            var entries = new List<NameValueCollection>();
            NameValueCollection entry = null;
            var newEntry = false;

            foreach (string line in lines)
            {
                var key = "";
                var value = "";

                if (line.Contains(":port"))
                {
                    newEntry = true;
                    int pos = line.IndexOf(":");
                    pos++;
                    pos = line.IndexOf(":", pos);
                    value = line.Substring(pos + 1).Trim();
                    key = line.Substring(0,pos - 1).Trim();

                    entry = new NameValueCollection
                    {
                        { key, value }
                    };
                }
                else 
                { 
                    if (newEntry)
                    {
                        if (line == "" || line.Contains("Extended Properties"))
                        {
                            entries.Add(entry);
                            newEntry = false;
                        } 
                        else
                        {
                            key = line.Split(':')[0].Trim();
                            value = line.Split(':')[1].Trim();
                            entry.Add(key, value);
                        }
                    }
                }
            }

            return entries;
        }

        #endregion

        private void UpdateCerts()
        {
            bool dirty = false;

            foreach (NameValueCollection nvc in _settings)
            {
                if (Util.GetValue(nvc[TlsCertParms.VerifyClientCertRevocation]).ToString() == "True")
                {
                    nvc[TlsCertParms.VerifyClientCertRevocation] = "False";
                    dirty = true;
                }

                if (dirty)
                    UpdateCert(nvc);

            }

            _settings = GetInfo();
        }

        private void UpdateCert(NameValueCollection nvc)
        {
            string host = nvc[0].Split(':')[0];

            UriHostNameType hostType = Uri.CheckHostName(host);

            bool abort = false;
            bool ip = false;

            switch (hostType)
            {
                case UriHostNameType.Dns:
                case UriHostNameType.Basic:
                    ip = false;
                    break;
                case UriHostNameType.IPv4:
                case UriHostNameType.IPv6:
                    ip = true;
                    break;
                default:
                    abort = true;
                    break;
            }

            if (abort)
                return;

            string ret;
            StringBuilder cmd = new StringBuilder();
            
            if (ip)
            {
                ret = ExecuteCmd.Run("netsh http delete sslcert ipport=" + nvc[0]);
                Console.WriteLine(ret);
                cmd.Append("netsh http add sslcert ipport=");
            }
            else
            {
                ret = ExecuteCmd.Run("netsh http delete sslcert hostnameport=" + nvc[0]);
                Console.WriteLine(ret);
                cmd.Append("netsh http add sslcert hostnameport=");
            }

            Console.WriteLine("");

            cmd.Append(nvc[0]);
            cmd.Append(" ");
            cmd.Append("certhash=");
            cmd.Append(nvc[TlsCertParms.CertHash]);
            cmd.Append(" ");
            cmd.Append("appid=");
            cmd.Append(nvc[TlsCertParms.ApplicationId]);
            cmd.Append(" ");
            cmd.Append("certstorename=");
            cmd.Append(nvc[TlsCertParms.CertStoreName]);
            cmd.Append(" ");
            cmd.Append("verifyclientcertrevocation=");

            cmd.Append(Util.GetEnabled(Util.GetValue(nvc[TlsCertParms.VerifyClientCertRevocation])));

            ret = ExecuteCmd.Run(cmd.ToString());

            Console.WriteLine(ret);
            Console.WriteLine("");
        }

        private void PrintCert(NameValueCollection nvc)
        {
            foreach (var key in nvc.AllKeys)
            {
                Console.WriteLine(key.PadRight(60) + ": " + Util.GetValue(nvc[key]));
            }
            Console.WriteLine("");
        }

    }
}
