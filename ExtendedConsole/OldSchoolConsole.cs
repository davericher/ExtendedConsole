using System;

namespace ExtendedConsole
{
    public class OldSchoolConsole : ExtConsole
    {

        public override void LoadDefaults()
        {
            base.LoadDefaults();
            RuleOnColor = ConsoleColor.Cyan;
            RuleOffColor = ConsoleColor.DarkCyan;
            RuleChar = '%';
            PrimaryColor = ConsoleColor.Cyan;
        }
    }
}