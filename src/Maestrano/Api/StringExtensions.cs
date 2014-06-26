using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Maestrano.Api
{
    internal static class StringExtensions
    {
        /// <summary>
        /// Add underscores to a pascal-cased string
        /// </summary>
        /// <param name="pascalCasedWord">String to convert</param>
        /// <returns>string</returns>
        public static string ToSnakeCase(this string pascalCasedWord)
        {
            return
                Regex.Replace(
                    Regex.Replace(Regex.Replace(pascalCasedWord, @"([A-Z]+)([A-Z][a-z])", "$1_$2"), @"([a-z\d])([A-Z])",
                        "$1_$2"), @"[-\s]", "_");
        }
    }
}
