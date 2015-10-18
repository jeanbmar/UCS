using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using UCS.Logic;
using UCS.Helpers;
using UCS.GameFiles;
using UCS.Core;

namespace UCS.PacketProcessing
{
    //Commande 0x20B
    class ClaimAchievementRewardCommand : Command
    {
        public ClaimAchievementRewardCommand(BinaryReader br)
        {
            AchievementId = br.ReadUInt32WithEndian();//= achievementId - 0x015EF3C0;
            Unknown1 = br.ReadUInt32WithEndian();
        }

        //00 00 02 0B 01 5E F3 C6 00 00 06 53

        public uint AchievementId { get; set; } 
        public uint Unknown1 { get; set; }
    }
}
