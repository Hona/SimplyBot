using System.Linq;
using System.Threading.Tasks;
using Discord.Commands;
using SimplyBotUI.Data;

namespace SimplyBotUI.Modules
{
    [Group("tempus")]
    public class TempusModule : ExtraModuleBase
    {
        public TempusDataAccess TempusDataAccess { get; set; }

        [Command("dwr")]
        public async Task GetDemoRecord(string map)
        {
            var result = await TempusDataAccess.GetFullMapOverView(map);
            var demoRecord = result.DemomanRuns.OrderBy(x => x.Duration).First();
            await ReplyNewEmbed(
                $"**Demo WR** - {result.MapInfo.Name} - {demoRecord.Name} - {demoRecord.FormattedDuration}");
        }

        [Command("dtime")]
        public async Task GetDemoTime(string map, int place)
        {
            var result = await TempusDataAccess.GetFullMapOverView(map);
            var demoRecord = result.DemomanRuns.OrderBy(x => x.Duration).Skip(place - 1).First();
            if (demoRecord != null)
                await ReplyNewEmbed(
                    $"**Demo #{place}** - {result.MapInfo.Name} - {demoRecord.Name} - {demoRecord.FormattedDuration}");
            else
                await ReplyNewEmbed("Time not found");
        }

        [Command("swr")]
        public async Task GetSoldierRecord(string map)
        {
            var result = await TempusDataAccess.GetFullMapOverView(map);
            var demoRecord = result.SoldierRuns.OrderBy(x => x.Duration).First();
            await ReplyNewEmbed(
                $"**Solly WR** - {result.MapInfo.Name} - {demoRecord.Name} - {demoRecord.FormattedDuration}");
        }

        [Command("stime")]
        public async Task GetSoldierTime(string map, int place)
        {
            var result = await TempusDataAccess.GetFullMapOverView(map);
            var demoRecord = result.SoldierRuns.OrderBy(x => x.Duration).Skip(place - 1).First();
            if (demoRecord != null)
                await ReplyNewEmbed(
                    $"**Solly #{place}** - {result.MapInfo.Name} - {demoRecord.Name} - {demoRecord.FormattedDuration}");
            else
                await ReplyNewEmbed("Time not found");
        }
    }
}