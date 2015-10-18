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
    //Commande 0x208
    class UnlockBuildingCommand : Command
    {
        public UnlockBuildingCommand(BinaryReader br)
        {
            BuildingId = br.ReadUInt32WithEndian(); //buildingId - 0x1DCD6500;
            Unknown1 = br.ReadUInt32WithEndian();
        }

        public uint BuildingId { get; set; } 
        public uint Unknown1 { get; set; } 

        public override void Execute(Level l)
        {

        }
    }
}
