using System;
using System.Collections.Generic;
using System.Text;
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

        }
    }
}
