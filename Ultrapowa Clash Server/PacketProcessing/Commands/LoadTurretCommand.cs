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
    //Commande 0x20D
    class LoadTurretCommand : Command
    {
        public LoadTurretCommand(BinaryReader br)
        {
            Unknown1 = br.ReadUInt32WithEndian();
            BuildingId = br.ReadUInt32WithEndian(); //buildingId - 0x1DCD6500;
            Unknown2 = br.ReadUInt32WithEndian();
        }

        //00 00 02 0D 00 00 00 01 1D CD 65 03 00 00 01 DF

        public uint Unknown1 { get; set; } 
        public uint BuildingId { get; set; } 
        public uint Unknown2 { get; set; }
        
    }
}
