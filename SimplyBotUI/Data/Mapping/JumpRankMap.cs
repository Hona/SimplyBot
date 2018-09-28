using Dapper.FluentMap.Mapping;
using SimplyBotUI.Models;

namespace SimplyBotUI.Data.Mapping
{
    internal class JumpRankMap : EntityMap<JumpRankModel>
    {
        public JumpRankMap()
        {
            Map(rank => rank.SteamId).ToColumn("steam_id");
            Map(rank => rank.Name).ToColumn("name");
            Map(rank => rank.SoldierRank).ToColumn("sol");
            Map(rank => rank.DemomanRank).ToColumn("dem");
            Map(rank => rank.ConcRank).ToColumn("conc");
            Map(rank => rank.EngineerRank).ToColumn("eng");
            Map(rank => rank.PyroRank).ToColumn("pyro");
            Map(rank => rank.OverallRank).ToColumn("general");
        }
    }
}