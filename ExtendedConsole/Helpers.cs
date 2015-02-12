using System;
using System.Xml;

namespace ExtendedConsole
{
    public static class Helpers
    {
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
            //TODO make this adjust on actual ives words and etc, borrow from php laravel?
            return input + "s";
        }

        public static string SplitCamelCase(string input)
        {
            return System.Text.RegularExpressions.Regex.Replace(input, "([A-Z])", " $1", System.Text.RegularExpressions.RegexOptions.Compiled).Trim();
        }
    }
}
