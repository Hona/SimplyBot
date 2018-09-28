using System;
using Discord;

namespace SimplyBotUI.Data
{
    internal static class ClassConstants
    {
        internal const int Soldier = 0;
        internal const int Demoman = 1;
        internal const int Conc = 2;
        internal const int Engineer = 3;
        internal const int Pyro = 4;
        internal const int CombinedRank = 5;

        internal static string ToString(int value)
        {
            switch (value)
            {
                case 0: return "Soldier";
                case 1: return "Demoman";
                case 2: return "Conc";
                case 3: return "Engineer";
                case 4: return "Pyro";
                default: return "Unknown";
            }
        }
    }
}