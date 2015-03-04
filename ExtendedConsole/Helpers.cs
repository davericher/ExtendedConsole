using System;
using System.Data.Entity.Design.PluralizationServices;
using System.Globalization;
using System.IO;
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
            return ValidateString(String.Format("{0," + (Console.WindowWidth / 2 - offset + (text.Length / 2)) + "}", text));
        }

        /// <summary>
        /// Pluralize a word, draws upon MVC framework
        /// </summary>
        /// <param name="input">Word to be pluralized</param>
        /// <returns>Plural string</returns>
        public static string Pluralize(string input)
        {
            return ValidateString(PluralizationService.CreateService(CultureInfo.GetCultureInfo(Lang)).Pluralize(input));
        }

        /// <summary>
        /// Make a plural world singular, draws upon MVC framework
        /// </summary>
        /// <param name="input">Word to make singular</param>
        /// <returns>Singular string</returns>
        public static string Singular(string input)
        {
            return ValidateString(PluralizationService.CreateService(CultureInfo.GetCultureInfo(Lang)).Singularize(input));
        }

        /// <summary>
        /// Split a camel case expression
        /// </summary>
        /// <param name="input">Camel Case string</param>
        /// <returns>Reformatted string</returns>
        public static string SplitCamelCase(string input)
        {
            return ValidateString(Regex.Replace(UppercaseFirst(input), "([A-Z])", " $1", RegexOptions.Compiled).Trim());
        }

        /// <summary>
        /// Make the forst letter in a string uppercase
        /// </summary>
        /// <param name="s">String</param>
        /// <returns>String with first letter upcased</returns>
        public static string UppercaseFirst(string s)
        {
            return !string.IsNullOrEmpty(s) ? ValidateString(char.ToUpper(s[0]) + s.Substring(1)) : String.Empty;
        }

        /// <summary>
        /// Validate a string has content or is not null, if it is return an empty string
        /// </summary>
        /// <param name="input">string to be validated</param>
        /// <returns>Results or Empty string</returns>
        public static string ValidateString(string input)
        {
            return string.IsNullOrEmpty(input) ? string.Empty : input;
        }

        /// <summary>
        /// Check if both the Directory or File exist
        /// </summary>
        /// <param name="dir">The Directory to check</param>
        /// <param name="fullPath">The full path to the file</param>
        /// <param name="createDirectory">Create directory if it does not exist</param>
        /// <returns></returns>
        public static bool FileOrDirectoryExists(string dir, string fullPath, bool createDirectory = false)
        {
            return (DirExists(dir,createDirectory) && FileExists(fullPath));
        }

        /// <summary>
        /// Check if  directory exists
        /// </summary>
        /// <param name="location">Directory location</param>
        /// <param name="create">Create the directory if it does not exist</param>
        /// <returns></returns>
        public static  bool DirExists(string location,bool create = false)
        {
            if (create)
                Directory.CreateDirectory(location);
            return Directory.Exists(location);
        }

        public static bool FileExists(string location)
        {
            return File.Exists(location);
        }
    }
}
