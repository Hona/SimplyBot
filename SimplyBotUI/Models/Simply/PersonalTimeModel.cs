using System;

namespace SimplyBotUI.Models.Simply
{
    internal class PersonalTimeModel
    {
        public TimeSpan GetTimeSpan => new TimeSpan(0, 0, (int) Math.Truncate(RunTime),
            (int) (RunTime - (int) Math.Truncate(RunTime)));

        public string SteamId { get; set; }
        public string Name { get; set; }
        public double RunTime { get; set; }
        public string Map { get; set; }
        public double Timestamp { get; set; }
        public int Class { get; set; }
    }
}