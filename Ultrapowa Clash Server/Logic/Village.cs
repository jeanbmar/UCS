using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Configuration;
using UCS.PacketProcessing;
using Newtonsoft.Json;

namespace UCS.Logic
{
    class Village
    {

        public Village()
        {
            //Deserialization
        }

        public Village(long playerId)
        {
            android_client = true;

            respawnVars = new RespawnVars()
            {
                secondsFromLastRespawn = 10,
                respawnSeed = 10,
                obstacleClearCounter = 10,
                time_to_gembox_drop = 10,
                time_in_gembox_period = 10
            };
            newShopBuildings = new uint[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            newShopTraps = new uint[] { 0, 0, 0, 0, 0, 0, 0, 0 };
            newShopDecos = new uint[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            last_league_rank = 0;
            last_league_shuffle = 0;
            last_news_seen = 10;
            edit_mode_shown = false;
            war_tutorials_seen = false;
            war_base = false;
        }

        public bool android_client { get; set; }

        public RespawnVars respawnVars { get; set; }
        public uint[] newShopBuildings { get; set; }
        public uint[] newShopTraps { get; set; }
        public uint[] newShopDecos { get; set; }
        public uint last_league_rank { get; set; }
        public uint last_league_shuffle { get; set; }
        public uint last_news_seen { get; set; }
        public bool edit_mode_shown { get; set; }
        public bool war_tutorials_seen { get; set; }
        public bool war_base { get; set; }
    }

    class RespawnVars
    {
        public RespawnVars() { }
        public uint secondsFromLastRespawn { get; set; }
        public uint respawnSeed { get; set; }
        public uint obstacleClearCounter { get; set; }
        public uint time_to_gembox_drop { get; set; }
        public uint time_in_gembox_period { get; set; }
    }
}
