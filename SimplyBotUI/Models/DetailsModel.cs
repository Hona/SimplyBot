namespace SimplyBotUI.Models
{
    internal class DetailsModel
    {
        public string Map { get; set; }
        public string Creator { get; set; }
        public string Class { get; set; }
        public int SoldierMinimumTier { get; set; }
        public int SoldierTier { get; set; }
        public int DemomanMinimumTier { get; set; }
        public int DemomanTier { get; set; }
        public int Jumps { get; set; }
        public int Bonus { get; set; }
        public int Played { get; set; }
        public int TimePlayed { get; set; }
        public string Team { get; set; }
        public string TimeLimit { get; set; }
    }
}