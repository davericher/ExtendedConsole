using System;
using System.Collections.Generic;
using System.Linq;

namespace ExtendedConsole
{
    public class ExtConsole
    {
        /// <summary>
        ///     The primary buffer,
        ///     It keeps track of the current line, and each line contains a dictionary of strings and colors
        /// </summary>
        private readonly Dictionary<int, Dictionary<string, ConsoleColor>> _tmpConsoleBuffer;

        // Constructors
        /// <summary>
        ///     Constructor requiring the title of the console app
        /// </summary>
        /// <param name="title">The title of the current application</param>
        public ExtConsole(string title)
        {
            _tmpConsoleBuffer = new Dictionary<int, Dictionary<string, ConsoleColor>>();
            SetDefaults();
            WindowTitle = title;
            InitWindow();
        }

        /// <summary>
        ///     No Paramater constructor which will set default Console title
        /// </summary>
        public ExtConsole() : this(DefaultWindowTitle)
        {
        }


        // Constants
        private const string DefaultWindowTitle = "Console Application";

        // Auto Properties

        #region Properties

        public ConsoleColor RuleOnColor { get; set; } // On color for Horizontal Line and header line
        public ConsoleColor RuleOffColor { get; set; } // Off color for Horizontal line and header line
        public ConsoleColor PrimaryColor { get; set; } // Primary text color
        public ConsoleColor BoldColor { get; set; } // Primary bold color
        public ConsoleColor ErrorColor { get; set; } // Primary error color
        public ConsoleColor SuccessColor { get; set; } // Primary success color
        public int WindowWidth { get; set; } // Window width
        public int WindowHeight { get; set; } // Window Height
        public char RuleChar { get; set; } // Horizontal line / header line display character
        public int ElementsPerLine { get; set; }
        // Amount of elements per line in PrintArray TODO this does not need to be here
        public string WindowTitle { get; set; } // Title of the window
        public string PausePrompt { get; set; } // Default pause prompt

        #endregion

        /// <summary>
        ///     Reinitialize the window, calling on SetWindowSize and SetWindowTitle
        /// </summary>
        public void InitWindow()
        {
            SetWindowSize();
            SetWindowTitle();
        }

        /// <summary>
        ///     Invert the current Rule colors
        /// </summary>
        public void InvertRuleColors()
        {
            var tmpOn = RuleOnColor;
            var tmpOff = RuleOffColor;

            RuleOffColor = tmpOn;
            RuleOnColor = tmpOff;
        }

        // Read From Console

        #region Read Functions

        /// <summary>
        ///     Read a non empty string from the console
        /// </summary>
        /// <param name="prompt">Display prompt</param>
        /// <param name="error">Optional error prompt default to Invalid Input</param>
        /// <returns></returns>
        public string ReadANonEmptyString(string prompt, string error = "Invalid Input")
        {
            string input = null;
            var looped = false;

            while (String.IsNullOrWhiteSpace(input))
            {
                if (looped && String.IsNullOrWhiteSpace(input))
                    WriteAErrorLine(error);
                Write(String.Format("{0} : ", prompt), BoldColor);
                Print();
                input = Console.ReadLine();
                looped = true;
            }
            return input;
        }

        /// <summary>
        ///     Read a vallid integer from the console
        /// </summary>
        /// <param name="prompt">Prompt to be displayed, defaults to "Please enter a number"</param>
        /// <param name="error">Error message if non number is entered, default sto "That was not a valid number"</param>
        /// <returns></returns>
        public int ReadAValidatedInt(
            String prompt = "Please enter a number: ",
            String error = "That was not a valid Number"
            )
        {
            int output;

            while (true)
            {
                Write(String.Format("{0}: ", prompt), BoldColor);
                Print();
                if (int.TryParse(Console.ReadLine(), out output))
                    break;
                WriteAErrorLine(error);
                Print();
            }
            return output;
        }

        public int ReadFromOptions(string header, string prompt, string error, OptionsList options,
            bool optionToContinue = true)
        {
            PrintIfExists();
            WriteOptions(header, options, optionToContinue);
            int selection;
            var looped = false;
            do
            {
                if (looped)
                    WriteAErrorLine(error);
                selection = ReadAValidatedInt(prompt, error);
                looped = true;
            } while (!options.IsValid(selection) && (optionToContinue && selection != options.ContinueOption));
            return selection;
        }

        public int ReadValidInt(string prompt, string error)
        {
            PrintIfExists();
            var valid = true;
            do
            {
                if (!valid)
                    WriteAErrorLine(error);
                var input = ReadANonEmptyString(prompt, error);
                int selection;
                valid = int.TryParse(input, out selection);
                if (valid)
                    return selection;
            } while (true);
        }

        public double ReadValidDouble(string prompt, string error)
        {
            PrintIfExists();
            var valid = true;
            do
            {
                if (!valid)
                    WriteAErrorLine(error);
                var input = ReadANonEmptyString(prompt, error);
                double selection;
                valid = double.TryParse(input, out selection);
                if (valid)
                    return selection;
            } while (true);
        }

        /// <summary>
        ///     Get a yes or a no (y/n) from the user
        ///     will not break until at least one is pressed
        /// </summary>
        /// <param name="text">
        ///     The optional Display prompt
        ///     Default tp null and is appended by [y]es pr [n]o
        /// </param>
        /// <returns></returns>
        public bool ReadYesOrNo(String text)
        {
            ConsoleKeyInfo key;
            PrintIfExists();

            LineBreak();

            Write(text + (String.IsNullOrWhiteSpace(text) ? "" : " "), BoldColor);

            WriteYesOrNo();
            Print();

            do
                key = Console.ReadKey(true); while (!Helpers.KeyIs(key, 'y') && !Helpers.KeyIs(key, 'n'));

            LineBreak(2);

            return Helpers.KeyIs(key, 'y');
        }

        public bool ReadYesOrNo()
        {
            return ReadYesOrNo("");
        }

        /// <summary>
        ///     Prompt the user to press Any Key to continue, uses PausePrompt as default prompt
        /// </summary>
        public void Pause()
        {
            Pause(PausePrompt);
        }

        /// <summary>
        ///     See Pause
        /// </summary>
        /// <param name="text">The display prompt</param>
        public void Pause(string text)
        {
            PrintIfExists();
            LineBreak();
            WriteBold(text);
            Print();
            Console.ReadKey(true);
            LineBreak(2);
        }

        #endregion

        // Set Up Console

        #region Set Functions

        public void SetRuleFormatting(ConsoleColor ruleOn, ConsoleColor ruleOff, char ruleChar)
        {
            RuleOnColor = ruleOn;
            RuleOffColor = ruleOff;
            RuleChar = ruleChar;
        }

        /// <summary>
        ///     Initialize the defaults,
        ///     extend this class and override this method for different defaults
        ///     or change them pragmatically during execution
        /// </summary>
        public void SetDefaults()
        {
            RuleOnColor = ConsoleColor.Red;
            RuleOffColor = ConsoleColor.DarkRed;
            PrimaryColor = ConsoleColor.Gray;
            ErrorColor = ConsoleColor.Red;
            SuccessColor = ConsoleColor.Green;
            BoldColor = ConsoleColor.White;
            WindowWidth = 86;
            WindowHeight = 50;
            RuleChar = '*';
            ElementsPerLine = 10;
            PausePrompt = "[ Press Any Key to Continue ]";
        }

        /// <summary>
        ///     Adjust the console size specified by the properties WindowWidth and WindowHeight
        /// </summary>
        public void SetWindowSize()
        {
            Console.SetWindowSize(WindowWidth, WindowHeight);
        }

        /// <summary>
        ///     Set the window title based on the property WindowTitle
        /// </summary>
        public void SetWindowTitle()
        {
            Console.Title = WindowTitle;
        }

        #endregion

        // Write To Buffer

        #region Write Functions

        /// <summary>
        ///     Write into the buffer
        /// </summary>
        /// <param name="text">The text to be written</param>
        /// <param name="color">The ConsoleColor you wish the text to be displayed in</param>
        public void Write(string text, ConsoleColor color)
        {
            var count = !_tmpConsoleBuffer.Any() ? 0 : _tmpConsoleBuffer.Count() + 1;
            var line = new Dictionary<string, ConsoleColor> {{text, color}};
            _tmpConsoleBuffer.Add(count, line);
        }

        /// <summary>
        ///     Write a formated yes or no
        /// </summary>
        private void WriteYesOrNo()
        {
            Write("[Y]", BoldColor);
            Write("es or ");
            Write("[N]", BoldColor);
            Write("o: ");
        }

        /// <summary>
        ///     header lines are used to help format WriteHeader
        ///     RuleChar(RuleCharOff) Padding text Padding RuleChar(RuleCharOn)
        /// </summary>
        /// <param name="text">Text to be formated</param>
        /// <param name="color">ConsoleColor of text</param>
        /// <param name="center">Should the text be centered? default is yes</param>
        public void WriteAHeaderString(string text, ConsoleColor color, bool center = true)
        {
            Write(RuleChar.ToString(), RuleOnColor);
            var output = center ? Helpers.CenteredString(text, -1) : text;
            Write(output, color);
            Write(RuleChar.ToString().PadLeft((Console.WindowWidth - output.Length) - 1), RuleOnColor);
        }

        /// <summary>
        ///     Combines WriteAHeaderString and WriteAHorizontalLine to
        ///     display a padded formated header
        /// </summary>
        /// <param name="text">The text to be formatted</param>
        public void WriteHeader(string text)
        {
            HorizontalString();
            WriteAHeaderString(text, BoldColor);
            HorizontalString();
            LineBreak();
        }

        /// <summary>
        ///     Write the current window title as a header
        /// </summary>
        public void WriteTitle()
        {
            WriteHeader(WindowTitle);
        }


        /// <summary>
        ///     Write a formated array, allows for generics
        /// </summary>
        /// <param name="arr">Array to be printed</param>
        public void WriteAnArray(IEnumerable<object> arr)
        {
            var counter = 0;

            foreach (var element in arr)
            {
                Write(String.Format("{0}\t", element), (++counter%2) == 0 ? PrimaryColor : BoldColor);
                if (counter != ElementsPerLine) continue;
                LineBreak();
                counter = 0;
            }
            LineBreak();
        }

        /// <summary>
        ///     Write a error line to the console formated
        ///     with line breaks and in ErrorColor
        ///     and of course beep
        /// </summary>
        /// <param name="text">Text to be formatted</param>
        public void WriteAErrorLine(string text)
        {
            LineBreak();
            WriteAnErrorString(text);
            LineBreak();
            Print();
            Console.Beep();
        }

        private void WriteAnErrorString(string text)
        {
            WriteLine(text, ErrorColor);
        }

        /// <summary>
        ///     Write a success line to the console formated
        ///     with line breaks and in SuccessColor
        ///     and of course a beep
        /// </summary>
        /// <param name="text"></param>
        public void WriteASuccessLine(string text)
        {
            LineBreak();
            WriteASuccessString(text);
            LineBreak();
            Print();
            Console.Beep(2600, 30);
        }

        private void WriteASuccessString(string text)
        {
            WriteLine(text, SuccessColor);
        }

        // TODO: This function should be expanded, there should be an options class that can be created and validated against
        public void WriteOptions(string header, OptionsList options, bool optionToContinue = true)
        {
            HorizontalString();
            WriteAHeaderString(header, BoldColor);
            HorizontalString();
            for (var x = 0; x <= options.Count - 1; x++)
            {
                Write(RuleChar + " ", RuleOnColor);
                WriteBold((x + 1) + " ");
                Write(RuleChar + " ", RuleOffColor);
                WriteBold("\t" + options[x] + "\n");
            }
            if (optionToContinue)
            {
                Write(RuleChar + " ", RuleOnColor);
                WriteBold(options.ContinueOption + " ");
                Write(RuleChar + " ", RuleOffColor);
                Write("\t" + "To Continue" + "\n", SuccessColor);
            }
            HorizontalLine();
        }

        /// <summary>
        ///     See Above, overloaded for one argument with default color set
        /// </summary>
        /// <param name="text">The text to be written</param>
        public void Write(string text)
        {
            Write(text, PrimaryColor);
        }

        /// <summary>
        ///     See Write, add a linebreak
        /// </summary>
        /// <param name="text">Text to be written</param>
        /// <param name="color">ConsoleColor of text</param>
        public void WriteLine(string text, ConsoleColor color)
        {
            Write(text + Environment.NewLine, color);
        }

        /// <summary>
        ///     See Write
        /// </summary>
        /// <param name="text">Text to be written in default color</param>
        public void WriteLine(string text)
        {
            WriteLine(text, PrimaryColor);
        }

        /// <summary>
        ///     See Write
        /// </summary>
        /// <param name="text">Text to be written in bold color</param>
        public void WriteBold(string text)
        {
            Write(text, BoldColor);
        }

        /// <summary>
        ///     See WriteLine
        /// </summary>
        /// <param name="text">Line to be written in bold</param>
        public void WriteLineBold(string text)
        {
            WriteLine(text, BoldColor);
        }

        /// <summary>
        ///     Clear the current buffer
        /// </summary>
        public void ClearBuffer()
        {
            _tmpConsoleBuffer.Clear();
        }

        /// <summary>
        ///     Insert a linebreak into the buffer
        /// </summary>
        /// <param name="total">The amount of linesbreaks you want to insert, defaults to 1</param>
        public void LineBreak(int total = 1)
        {
            for (var counter = 0; counter < total; counter++)
                Write(Environment.NewLine);
        }

        /// <summary>
        ///     Inser a horizontal line into the buffer
        ///     works by alternating betten RuleOnColor and RuleOffColor with RuleChar as the character
        ///     if you want consistent color set both to same ConsoleColor value
        ///     TODO Optional switch to disable color alteration and default on if it should be on or off
        /// </summary>
        public void HorizontalLine()
        {
            HorizontalString();
            LineBreak();
        }

        public void HorizontalLine(char start, char middle, char end)
        {
            HorizontalString(start, middle, end);
        }

        /// <summary>
        ///     Write a horizontal formated line to the buffer
        /// </summary>
        public void HorizontalString()
        {
            for (var x = 1; x <= Console.WindowWidth; x++)
                Write(RuleChar.ToString(), (x%2).Equals(0) ? RuleOffColor : RuleOnColor);
        }

        public void HorizontalString(char start, char middle, char end)
        {
            Write(start.ToString(), RuleOnColor);
            for (var x = 1; x <= Console.WindowWidth - 2; x++)
                Write(middle.ToString(), (x%2).Equals(0) ? RuleOffColor : RuleOnColor);
            Write(end.ToString(), RuleOffColor);
        }

        #endregion

        // Print To Console

        #region Print Functions

        /// <summary>
        ///     Print the current buffer to the console, then clear it
        /// </summary>
        public void Print()
        {
            foreach (var result in _tmpConsoleBuffer.SelectMany(results => results.Value))
            {
                Console.ForegroundColor = result.Value;
                Console.Write(result.Key);
                Console.ResetColor();
            }
            _tmpConsoleBuffer.Clear();
        }

        /// <summary>
        ///     Check to see if there is anything in the buffer
        ///     If there are contents, dumps it to the console
        /// </summary>
        public void PrintIfExists()
        {
            if (_tmpConsoleBuffer.Any())
                Print();
        }

        #endregion
    }
}