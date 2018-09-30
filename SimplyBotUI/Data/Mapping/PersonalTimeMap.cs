using Dapper.FluentMap.Mapping;
using SimplyBotUI.Models.Simply;

namespace SimplyBotUI.Data.Mapping
{
    internal class PersonalTimeMap : EntityMap<PersonalTimeModel>
    {
        public PersonalTimeMap()
        {
            Map(personal => personal.SteamId).ToColumn("steam_id");
            Map(personal => personal.Name).ToColumn("name");
            Map(personal => personal.RunTime).ToColumn("runtime");
            Map(personal => personal.Map).ToColumn("map");
            Map(personal => personal.Timestamp).ToColumn("timestamp");
            Map(personal => personal.Class).ToColumn("class");
        }
    }
}