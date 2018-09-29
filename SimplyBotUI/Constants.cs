using System;
using System.IO;

namespace SimplyBotUI
{
    internal static class Constants
    {
        internal const char CommandPrefix = '!';

        public const ulong RankChannelId = 495170332840296448;
        public const ulong UpdateChannelId = 377007504309092353;

        internal const int TopRankCount = 5;

        internal static readonly string DatabaseInfoPath =
            $"{Environment.CurrentDirectory}{Path.DirectorySeparatorChar}database.txt";

        internal static readonly string TokenPath =
            $"{Environment.CurrentDirectory}{Path.DirectorySeparatorChar}token.txt";
    }
}