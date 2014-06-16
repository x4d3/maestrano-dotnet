using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maestrano.Configuration
{
    public class Sso
    {
        // Is Single Sign-On enabled - useful for debugging
        private bool? _enabled;
        public bool Enabled
        { 
            get
            {
                if(!_enabled.HasValue) {
                    return true;
                }
                return (bool)_enabled;
            }

            set{ _enabled = value; }
        }

        // SSO user creation mode: 'real' or 'virtual'
        private string _creationmode;
        public string CreationMode
        { 
            get
            {
                if(string.IsNullOrEmpty(_creationmode)) {
                    return "virtual";
                }
                return _creationmode;
            }

            set{ _creationmode = value; }
        }

        // Path to init action
        private string _initpath;
        public string InitPath
        { 
            get
            {
                if(string.IsNullOrEmpty(_initpath)) {
                    return "/maestrano/auth/saml/init";
                }
                return _initpath;
            }

            set{ _initpath = value; }
        }

        // Path to consume action
        private string _consumepath;
        public string ConsumePath
        { 
            get
            {
                if(string.IsNullOrEmpty(_consumepath)) {
                    return "/maestrano/auth/saml/consume";
                }
                return _consumepath;
            }

            set{ _consumepath = value; }
        }

        // Address of the identity manager (for your application)
        private string _idm;
        public string Idm
        {
            get
            {
                if (string.IsNullOrEmpty(_idm))
                {
                    if (!string.IsNullOrEmpty(Maestrano.App.Host))
                    {
                        return Maestrano.App.Host;
                    }
                    return "localhost";
                }
                return _idm;
            }

            set { _idm = value; }
        }

        // Address of the identity provider
        private string _idp;
        public string Idp
        { 
            get
            {
                if(string.IsNullOrEmpty(_idp)) {
                    if (Maestrano.Environment.Equals("production")) {
                        return "https://maestrano.com";
                    } else {
                        return "http://api-sandbox.maestrano.io";
                    }
                }
                return _idp;
            }

            set{ _idp = value; }
        }

        // The nameid format for SAML handshake
        private string _nameidformat;
        public string NameIdFormat 
        { 
            get
            {
                if(string.IsNullOrEmpty(_nameidformat)) {
                    return "urn:oasis:names:tc:SAML:2.0:nameid-format:persistent";
                }
                return _nameidformat;
            } 
            
            set{ _nameidformat = value; } 
        }

        // Fingerprint of x509 certificate used for SAML
        private string _x509fingerprint;
        public string X509Fingerprint
        { 
            get
            {
                if(string.IsNullOrEmpty(_x509fingerprint)) {
                    if (Maestrano.Environment.Equals("production")) {
                        return "2f:57:71:e4:40:19:57:37:a6:2c:f0:c5:82:52:2f:2e:41:b7:9d:7e";
                    } else {
                        return "01:06:15:89:25:7d:78:12:28:a6:69:c7:de:63:ed:74:21:f9:f5:36";
                    }
                }
                return _x509fingerprint;
            }

            set{ _x509fingerprint = value; }
        }

        // Actual x509 certificate
        private string _509certificate;
        public string X509Certificate
        { 
            get
            {
                if (string.IsNullOrEmpty(_509certificate))
                {
                    if (Maestrano.Environment.Equals("production")) {
                        return "-----BEGIN CERTIFICATE-----\nMIIDezCCAuSgAwIBAgIJAPFpcH2rW0pyMA0GCSqGSIb3DQEBBQUAMIGGMQswCQYD\nVQQGEwJBVTEMMAoGA1UECBMDTlNXMQ8wDQYDVQQHEwZTeWRuZXkxGjAYBgNVBAoT\nEU1hZXN0cmFubyBQdHkgTHRkMRYwFAYDVQQDEw1tYWVzdHJhbm8uY29tMSQwIgYJ\nKoZIhvcNAQkBFhVzdXBwb3J0QG1hZXN0cmFuby5jb20wHhcNMTQwMTA0MDUyNDEw\nWhcNMzMxMjMwMDUyNDEwWjCBhjELMAkGA1UEBhMCQVUxDDAKBgNVBAgTA05TVzEP\nMA0GA1UEBxMGU3lkbmV5MRowGAYDVQQKExFNYWVzdHJhbm8gUHR5IEx0ZDEWMBQG\nA1UEAxMNbWFlc3RyYW5vLmNvbTEkMCIGCSqGSIb3DQEJARYVc3VwcG9ydEBtYWVz\ndHJhbm8uY29tMIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQD3feNNn2xfEz5/\nQvkBIu2keh9NNhobpre8U4r1qC7h7OeInTldmxGL4cLHw4ZAqKbJVrlFWqNevM5V\nZBkDe4mjuVkK6rYK1ZK7eVk59BicRksVKRmdhXbANk/C5sESUsQv1wLZyrF5Iq8m\na9Oy4oYrIsEF2uHzCouTKM5n+O4DkwIDAQABo4HuMIHrMB0GA1UdDgQWBBSd/X0L\n/Pq+ZkHvItMtLnxMCAMdhjCBuwYDVR0jBIGzMIGwgBSd/X0L/Pq+ZkHvItMtLnxM\nCAMdhqGBjKSBiTCBhjELMAkGA1UEBhMCQVUxDDAKBgNVBAgTA05TVzEPMA0GA1UE\nBxMGU3lkbmV5MRowGAYDVQQKExFNYWVzdHJhbm8gUHR5IEx0ZDEWMBQGA1UEAxMN\nbWFlc3RyYW5vLmNvbTEkMCIGCSqGSIb3DQEJARYVc3VwcG9ydEBtYWVzdHJhbm8u\nY29tggkA8WlwfatbSnIwDAYDVR0TBAUwAwEB/zANBgkqhkiG9w0BAQUFAAOBgQDE\nhe/18oRh8EqIhOl0bPk6BG49AkjhZZezrRJkCFp4dZxaBjwZTddwo8O5KHwkFGdy\nyLiPV326dtvXoKa9RFJvoJiSTQLEn5mO1NzWYnBMLtrDWojOe6Ltvn3x0HVo/iHh\nJShjAn6ZYX43Tjl1YXDd1H9O+7/VgEWAQQ32v8p5lA==\n-----END CERTIFICATE-----";
                    } else {
                        return "-----BEGIN CERTIFICATE-----\nMIIDezCCAuSgAwIBAgIJAOehBr+YIrhjMA0GCSqGSIb3DQEBBQUAMIGGMQswCQYD\nVQQGEwJBVTEMMAoGA1UECBMDTlNXMQ8wDQYDVQQHEwZTeWRuZXkxGjAYBgNVBAoT\nEU1hZXN0cmFubyBQdHkgTHRkMRYwFAYDVQQDEw1tYWVzdHJhbm8uY29tMSQwIgYJ\nKoZIhvcNAQkBFhVzdXBwb3J0QG1hZXN0cmFuby5jb20wHhcNMTQwMTA0MDUyMjM5\nWhcNMzMxMjMwMDUyMjM5WjCBhjELMAkGA1UEBhMCQVUxDDAKBgNVBAgTA05TVzEP\nMA0GA1UEBxMGU3lkbmV5MRowGAYDVQQKExFNYWVzdHJhbm8gUHR5IEx0ZDEWMBQG\nA1UEAxMNbWFlc3RyYW5vLmNvbTEkMCIGCSqGSIb3DQEJARYVc3VwcG9ydEBtYWVz\ndHJhbm8uY29tMIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQDVkIqo5t5Paflu\nP2zbSbzxn29n6HxKnTcsubycLBEs0jkTkdG7seF1LPqnXl8jFM9NGPiBFkiaR15I\n5w482IW6mC7s8T2CbZEL3qqQEAzztEPnxQg0twswyIZWNyuHYzf9fw0AnohBhGu2\n28EZWaezzT2F333FOVGSsTn1+u6tFwIDAQABo4HuMIHrMB0GA1UdDgQWBBSvrNxo\neHDm9nhKnkdpe0lZjYD1GzCBuwYDVR0jBIGzMIGwgBSvrNxoeHDm9nhKnkdpe0lZ\njYD1G6GBjKSBiTCBhjELMAkGA1UEBhMCQVUxDDAKBgNVBAgTA05TVzEPMA0GA1UE\nBxMGU3lkbmV5MRowGAYDVQQKExFNYWVzdHJhbm8gUHR5IEx0ZDEWMBQGA1UEAxMN\nbWFlc3RyYW5vLmNvbTEkMCIGCSqGSIb3DQEJARYVc3VwcG9ydEBtYWVzdHJhbm8u\nY29tggkA56EGv5giuGMwDAYDVR0TBAUwAwEB/zANBgkqhkiG9w0BAQUFAAOBgQCc\nMPgV0CpumKRMulOeZwdpnyLQI/NTr3VVHhDDxxCzcB0zlZ2xyDACGnIG2cQJJxfc\n2GcsFnb0BMw48K6TEhAaV92Q7bt1/TYRvprvhxUNMX2N8PHaYELFG2nWfQ4vqxES\nRkjkjqy+H7vir/MOF3rlFjiv5twAbDKYHXDT7v1YCg==\n-----END CERTIFICATE-----";
                    }
                }
                return _509certificate;
            }

            set { _509certificate = value; }
        }

    }
}
