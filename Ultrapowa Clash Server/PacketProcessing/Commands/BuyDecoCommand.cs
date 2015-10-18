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
    //Commande 0x200
    class BuyDecoCommand : Command
    {
        public BuyDecoCommand(BinaryReader br)
        {
            X = br.ReadInt32WithEndian();
            Y = br.ReadInt32WithEndian();
            DecoId = br.ReadInt32WithEndian();
            Unknown1 = br.ReadUInt32WithEndian();
        }

        //00 00 02 00 00 00 00 22 00 00 00 1C 01 12 A8 81 00 00 0C 4F

        public int X { get; set; } 
        public int Y { get; set; }
        public int DecoId { get; set; } //01 12 A8 81
        public uint Unknown1 { get; set; }

        public override void Execute(Level level)
        {
            ClientAvatar ca = level.GetPlayerAvatar();

            DecoData dd = (DecoData)ObjectManager.DataTables.GetDataById(DecoId);

            if (ca.HasEnoughResources(dd.GetBuildResource(), dd.GetBuildCost()))
            {
                ResourceData rd = dd.GetBuildResource();
                ca.CommodityCountChangeHelper(0, rd, -dd.GetBuildCost());

                Deco d = new Deco(dd, level);
                d.SetPositionXY(X, Y);
                level.GameObjectManager.AddGameObject(d);
            }
        }
    }
}
