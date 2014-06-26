using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;
using System.IO;

namespace Maestrano.Tests
{
    public class Helpers
    {
        // Read a reponse from the support/saml folder
        public static string ReadSamlSupportFiles(string responseName)
        {
            // Build path
            string responseFolder = "../../Support/Saml";
            string responsePath = responseFolder + "/" + responseName;

            // Read the file
            StringBuilder sb = new StringBuilder();
            using (StreamReader sr = new StreamReader(responsePath))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    sb.AppendLine(line);
                }
            }
            string result = sb.ToString();

            // Return as string
            return sb.ToString();
        }
    }
}
