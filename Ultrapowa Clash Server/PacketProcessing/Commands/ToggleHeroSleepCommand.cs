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
    //Commande 0x0211
    class ToggleHeroSleepCommand : Command
    {
        public ToggleHeroSleepCommand(BinaryReader br)
        {
            BuildingId = br.ReadUInt32WithEndian(); //buildingId - 0x1DCD6500;
            FlagSleep = br.ReadByte();
            Unknown1 = br.ReadUInt32WithEndian();
        }

        //00 00 02 11 1D CD 65 06 00 00 01 04 CA

        public uint BuildingId { get; set; }
        public byte FlagSleep { get; set; } 
        public uint Unknown1 { get; set; } 
    }
}
