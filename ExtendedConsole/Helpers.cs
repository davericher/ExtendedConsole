using System;
using System.Data.Entity.Design.PluralizationServices;
using System.Globalization;
using System.Text.RegularExpressions;

namespace ExtendedConsole
{
    public static class Helpers
    {
        // National for Pluralize and Singular string functions
        private const string Lang = "en-us";
        

        /// <summary>
        /// Check the current read key for the specified character
        /// </summary>
        /// <param name="press">ConsoleKeyInfo - read by Console.ReadKey()</param>
        /// <param name="key">The character you are checking for</param>
        /// <returns>True if matched, False if not</returns>
        public static bool KeyIs(ConsoleKeyInfo press, char key)
        {
            return press.KeyChar.ToString().ToLower().Equals(key.ToString());
        }
       
        /// <summary>
        /// Right pad a line of text so it is centered on the console and takes into account 
        /// current resolution of said console
        /// </summary>
        /// <param name="text">The text to be centered</param>
        /// <param name="offset">The optional negative offset, defaults to 0</param>
        /// <returns>A reformatted string</returns>
        public static String CenteredString(string text, int offset = 0)
        {
            return String.Format("{0," + (Console.WindowWidth / 2 - offset + (text.Length / 2)) + "}", text);
        }


        public static string Pluralize(string input)
        {
            return PluralizationService.CreateService(CultureInfo.GetCultureInfo(Lang)).Pluralize(input);
        }

        public static string Singular(string input)
        {
            return PluralizationService.CreateService(CultureInfo.GetCultureInfo(Lang)).Singularize(input);
        }

        public static string SplitCamelCase(string input)
        {
            return Regex.Replace(UppercaseFirst(input), "([A-Z])", " $1", RegexOptions.Compiled).Trim();
        }

        public static string UppercaseFirst(string s)
        {
            // Check for empty string.
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            // Return char and concat substring.
            return char.ToUpper(s[0]) + s.Substring(1);
        }
    }
}
