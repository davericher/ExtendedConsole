using System;

namespace ExtendedConsole
{
    public class OldSchoolConsole : ExtConsole
    {

        public new void SetDefaults()
        {
            base.SetDefaults();
            RuleOnColor = ConsoleColor.Cyan;
            RuleOffColor = ConsoleColor.DarkCyan;
            RuleChar = '%';
            PrimaryColor = ConsoleColor.Cyan;
        }
    }
}