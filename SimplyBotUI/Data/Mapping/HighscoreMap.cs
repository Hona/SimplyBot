using Dapper.FluentMap.Mapping;

namespace SimplyBotUI.Data.Mapping
{
    internal class HighscoreMap : EntityMap<HighscoreModel>
    {
        public HighscoreMap()
        {
            Map(highscore => highscore.Position).ToColumn("pos");
            Map(highscore => highscore.SteamId).ToColumn("steam_id");
            Map(highscore => highscore.Name).ToColumn("name");
            Map(highscore => highscore.RunTime).ToColumn("runtime");
            Map(highscore => highscore.Map).ToColumn("map");
            Map(highscore => highscore.Timestamp).ToColumn("timestamp");
            Map(highscore => highscore.Class).ToColumn("class");
        }
    }
}