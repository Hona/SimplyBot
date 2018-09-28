using Dapper.FluentMap.Mapping;
using SimplyBotUI.Models;

namespace SimplyBotUI.Data.Mapping
{
    internal class DetailsMap : EntityMap<DetailsModel>
    {
        public DetailsMap()
        {
            Map(details => details.Map).ToColumn("map");
            Map(details => details.Creator).ToColumn("creator");
            Map(details => details.Class).ToColumn("class");
            Map(details => details.SoldierMinimumTier).ToColumn("smin_tier");
            Map(details => details.SoldierTier).ToColumn("s_tier");
            Map(details => details.DemomanMinimumTier).ToColumn("dmin_tier");
            Map(details => details.DemomanTier).ToColumn("d_tier");
            Map(details => details.Jumps).ToColumn("jumps");
            Map(details => details.Bonus).ToColumn("bonus");
            Map(details => details.Played).ToColumn("played");
            Map(details => details.TimePlayed).ToColumn("timeplay");
            Map(details => details.Team).ToColumn("team");
            Map(details => details.TimeLimit).ToColumn("timelimit");
        }
    }
}