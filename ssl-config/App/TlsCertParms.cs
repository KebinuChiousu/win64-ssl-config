using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace ssl_config.App
{
    public static class TlsCertParms
    {
        public const string IpPort = "IP:port";
        public const string CertHash = "Certificate Hash";
        public const string ApplicationId = "Application ID";
        public const string CertStoreName = "Certificate Store Name";
        public const string VerifyClientCertRevocation = "Verify Client Certificate Revocation";
        public const string VerifyClientCertRevocationUsingCache = "Verify Revocation Using Cached Client Certificate Only";
        public const string UsageCheck = "Usage Check";
        public const string RevocationFreshnessTime = "Revocation Freshness Time";
        public const string UrlRetrievalTimeout = "URL Retrieval Timeout";
        public const string CtlIdentifier = "Ctl Identifier";
        public const string CtlStoreName = "Ctl Store Name";
        public const string DsMapperUsage = "DS Mapper Usage";
        public const string NegotiateClientCert = "Negotiate Client Certificate";
        public const string RejectConnection = "Reject Connections";
        public const string DisableHttp2 = "Disable HTTP2";
        public const string DiableQuic = "Disable QUIC";
        public const string DisableTls12 = "Disable TLS1.2";
        public const string DisableTls13 = "Disable TLS1.3";
        public const string DisableOcspStapling = "Disable OCSP Stapling";
        public const string EnableTokenBinding = "Enable Token Binding";
        public const string LogExtendedEvents = "Log Extended Events";
        public const string DisableLegacyTlsVersions = "Disable Legacy TLS Versions";
        public const string EnableSessionTicket = "Enable Session Ticket";
    }
}
