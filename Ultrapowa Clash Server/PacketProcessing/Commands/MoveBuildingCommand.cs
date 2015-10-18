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
    //Commande 0x1F5
    class MoveBuildingCommand : Command
    {
        public MoveBuildingCommand(BinaryReader br)
        {
            X = br.ReadInt32WithEndian();
            Y = br.ReadInt32WithEndian();
            BuildingId = br.ReadInt32WithEndian(); //buildingId - 0x1DCD6500;
            Unknown1 = br.ReadUInt32WithEndian();
        }

        //30/08/2014 18:51;S;14102(0);32;00 00 2D BE 01 EB 32 0C 00 00 00 01 00 00 01 F5 00 00 00 13 00 00 00 1F 1D CD 65 06 00 00 2D 7F

        public int X { get; set; } //00 00 00 13
        public int Y { get; set; } //00 00 00 1F
        public int BuildingId { get; set; } //1D CD 65 06 some unique id
        public uint Unknown1 { get; set; } //00 00 2D 7F some client tick

        public override void Execute(Level level)
        {
            GameObject go = level.GameObjectManager.GetGameObjectByID(BuildingId);
            go.SetPositionXY(X, Y);
        }
    }
}
