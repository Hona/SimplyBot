using Dapper.FluentMap.Mapping;
using SimplyBotUI.Models.Simply;

namespace SimplyBotUI.Data.Mapping
{
    internal class HightowerPlayerMap : EntityMap<HightowerPlayerModel>
    {
        public HightowerPlayerMap()
        {
            Map(x => x.SteamId).ToColumn("steamid");
            Map(x => x.Nickname).ToColumn("nickname");
            Map(x => x.Points).ToColumn("points");
            Map(x => x.Seen).ToColumn("seen");
            Map(x => x.Deaths).ToColumn("deaths");
            Map(x => x.Kills).ToColumn("kills");
            Map(x => x.Assists).ToColumn("assists");
            Map(x => x.Backstabs).ToColumn("backstabs");
            Map(x => x.Headshots).ToColumn("headshots");
            Map(x => x.Feigns).ToColumn("feigns");
            Map(x => x.Playtime).ToColumn("playtime");
            Map(x => x.FlagCaptures).ToColumn("flagcaptures");
            Map(x => x.FlagDefends).ToColumn("flagdefends");
            Map(x => x.CapCaptures).ToColumn("capcaptures");
            Map(x => x.CapDefends).ToColumn("capdefends");
            Map(x => x.RoundsPlaged).ToColumn("roundsplayed");
            Map(x => x.DominationsGood).ToColumn("dominationsgood");
            Map(x => x.DominationsBad).ToColumn("dominationsbad");
            Map(x => x.Deflects).ToColumn("deflects");
            Map(x => x.Streak).ToColumn("streak");
        }
    }
}