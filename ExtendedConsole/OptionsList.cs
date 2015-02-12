using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ExtendedConsole
{
    public class OptionsList : List<string>
    {
        public int ContinueOption
        {
            get { return Count + 1; }
        }
        public  new OptionsList Add(string option)
        {
            base.Add(option);
            return this;
        }

        public int Index(int current)
        {
            return current - 1;
        }
        public bool IsValid(int option)
        {
            return (option <= Count && option > 0);
        }

    }
}
