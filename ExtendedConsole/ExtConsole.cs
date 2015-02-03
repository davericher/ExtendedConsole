using System;
using System.Collections.Generic;
using System.Linq;

namespace ExtendedConsole
{
    public class ExtConsole
    {
        private readonly Dictionary<int, Dictionary<string, ConsoleColor>> _tmpConsoleBuffer;
        // Constructors
        public ExtConsole(string title)
        {
            _tmpConsoleBuffer = new Dictionary<int, Dictionary<string, ConsoleColor>>();
            LoadDefaults();
            WindowTitle = title;
            InitWindow();
        }

        public ExtConsole()
        {
            InitWindow();
        }

        // Auto Properties
        public ConsoleColor RuleOnColor { get; set; }
        public ConsoleColor RuleOffColor { get; set; }
        public ConsoleColor PrimaryColor { get; set; }
        public ConsoleColor BoldColor { get; set; }
        public ConsoleColor ErrorColor { get; set; }
        public ConsoleColor SuccessColor { get; set; }
        public int WindowWidth { get; set; }
        public int WindowHeight { get; set; }
        public char RuleChar { get; set; }
        public int ElementsPerLine { get; set; }
        public string WindowTitle { get; set; }

        public void LoadDefaults()
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
        }

        public void SetWindowSize()
        {
            Console.SetWindowSize(WindowWidth, WindowHeight);
        }

        public void SetWindowTitle()
        {
            Console.Title = WindowTitle;
        }

        public void InitWindow()
        {
            SetWindowSize();
            SetWindowTitle();
        }

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

        public void PrintIfExists()
        {
            if (_tmpConsoleBuffer.Any())
                Print();
        }

        public void Write(string text, ConsoleColor color)
        {
            var count = !_tmpConsoleBuffer.Any() ? 0 : _tmpConsoleBuffer.Count() + 1;
            var line = new Dictionary<string, ConsoleColor> {{text, color}};
            _tmpConsoleBuffer.Add(count, line);
        }

        public void Write(string text)
        {
            Write(text, PrimaryColor);
        }

        public void WriteLine(string text, ConsoleColor color)
        {
            Write(text + "\n", color);
        }

        public void WriteLine(string text)
        {
            WriteLine(text, PrimaryColor);
        }

        public void WriteBold(string text)
        {
            Write(text, BoldColor);
        }

        public void WriteLineBold(string text)
        {
            WriteLine(text, BoldColor);
        }

        public void ClearBuffer()
        {
            _tmpConsoleBuffer.Clear();
        }

        public void LineBreak(int total = 1)
        {
            for (var counter = 0; counter < total; counter++)
                Write("\n");
        }

        public void HorizontalLine()
        {
            for (var x = 1; x <= Console.WindowWidth - 1; x++)
                Write(RuleChar.ToString(), (x%2).Equals(0) ? RuleOffColor : RuleOnColor);
            LineBreak();
        }

        public void Pause()
        {
            Pause("[ Press Any Key to Continue ]");
        }

        public void Pause(string text)
        {
            PrintIfExists();
            LineBreak();
            WriteBold(text);
            Print();
            Console.ReadKey(true);
            LineBreak(2);
        }

        public bool ReadYesOrNo(String text = null)
        {
            ConsoleKeyInfo key;
            PrintIfExists();

            LineBreak();

            if (text != null)
                Write(text + " ", BoldColor);

            Write("[Y]", BoldColor);
            Write("es or ");
            Write("[N]", BoldColor);
            Write("o: ");
            Print();

            do
                key = Console.ReadKey(true); 
            while (!key.KeyChar.ToString().ToLower().Equals("y") && !key.KeyChar.ToString().ToLower().Equals("n"));

            LineBreak(2);

            return key.KeyChar.ToString().ToLower().Equals("y");
        }

        public void WriteAHeaderLine(string text, ConsoleColor color)
        {
            Write(RuleChar.ToString(), RuleOnColor);
            var output = CenteredString(text, -1);
            Write(output, color);
            Write(RuleChar.ToString().PadLeft((Console.WindowWidth - 1) - output.Length - 1), RuleOnColor);
            LineBreak();
        }

        public void WriteHeader(string text)
        {
            HorizontalLine();
            WriteAHeaderLine(text, BoldColor);
            HorizontalLine();
            LineBreak();
        }

        public void WriteTitle()
        {
            WriteHeader(WindowTitle);
        }

        public String CenteredString(string text, int offset = 0)
        {
            return String.Format("{0," + (Console.WindowWidth/2 - offset + (text.Length/2)) + "}", text);
        }

        public void WriteAnIntArray(IEnumerable<int> arr)
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

        public void WriteAErrorLine(string text)
        {
            LineBreak();
            WriteLine(text, ErrorColor);
            LineBreak();
            Print();
            Console.Beep();
        }

        public void WriteASuccess(string text)
        {
            LineBreak();
            WriteLine(text, SuccessColor);
            LineBreak();
            Print();
            Console.Beep(2600, 30);
        }

        public string ReadANonEmptyString(string prompt, string error = "Invalid Input")
        {
            string input = null;
            bool looped = false;

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
    }
}