using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Security.AccessControl;
using System.Text;

namespace SimplyBotUI.Models
{
    internal class HightowerPlayerModel
    {
        public string SteamId { get; set; }
        public string Nickname { get; set; }
        public decimal Points { get; set; }
        public int Seen { get; set; }
        public int Deaths { get; set; }
        public int Kills { get; set; }
        public int Assists { get; set; }
        public int Backstabs { get; set; }
        public int Headshots { get; set; }
        public int Feigns { get; set; }
        public float Playtime { get; set; }
        public int FlagCaptures { get; set; }
        public int FlagDefends { get; set; }
        public int CapCaptures { get; set; }
        public int CapDefends { get; set; }
        public int RoundsPlaged { get; set; }
        public int DominationsGood { get; set; }
        public int DominationsBad { get; set; }
        public int Deflects { get; set; }
        public int Streak { get; set; }

        
    }
}
