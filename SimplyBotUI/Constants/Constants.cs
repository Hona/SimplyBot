using System;
using System.IO;

namespace SimplyBotUI.Constants
{
    internal static class Constants
    {
        internal const char CommandPrefix = '!';

        public const ulong RankChannelId = 495170332840296448;
        public const ulong UpdateChannelId = 377007504309092353;
        public const ulong StatusChannelId = 495498722084651010;
        public const ulong SimplyGuildId = 126272899035168768;
        public const ulong SimplyMemberRoleId = 495928973767999490;

        internal const int TopRankCount = 5;

        internal static readonly string DatabaseInfoPath =
            $"{Environment.CurrentDirectory}{Path.DirectorySeparatorChar}database.txt";

        internal static readonly string TokenPath =
            $"{Environment.CurrentDirectory}{Path.DirectorySeparatorChar}token.txt";
    }
}