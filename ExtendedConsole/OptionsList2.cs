using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace ExtendedConsole
{
    public class OptionsList2 : List<string>
    {


        public int ContinueOption
        {
            get { return Count + 1; }
        }
        public new OptionsList2 Add(string option)
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
