using System;
using System.IO;

namespace SimplyBotUI
{
    internal static class Constants
    {
        internal const char CommandPrefix = '!';

        internal static readonly string DatabaseInfoPath =
            $"{Environment.CurrentDirectory}{Path.DirectorySeparatorChar}database.txt";

        internal static readonly string TokenPath =
            $"{Environment.CurrentDirectory}{Path.DirectorySeparatorChar}token.txt";

        public const ulong RankChannelId = 495170332840296448;
        internal const int TopRankCount = 5;
    }
}