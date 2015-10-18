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
    //Commande 0x1FE
    class BuyTrapCommand : Command
    {
        public BuyTrapCommand(BinaryReader br)
        {
            X = br.ReadInt32WithEndian();
            Y = br.ReadInt32WithEndian();
            TrapId = br.ReadInt32WithEndian();
            Unknown1 = br.ReadUInt32WithEndian();
        }

        //00 00 01 FE 00 00 00 02 00 00 00 28 00 B7 1B 02 00 00 06 56

        public int X { get; set; } 
        public int Y { get; set; } 
        public int TrapId { get; set; } 
        public uint Unknown1 { get; set; }

        public override void Execute(Level level)
        {
            ClientAvatar ca = level.GetPlayerAvatar();

            TrapData td = (TrapData)ObjectManager.DataTables.GetDataById(TrapId);
            Trap t = new Trap(td, level);

            if (ca.HasEnoughResources(td.GetBuildResource(0), td.GetBuildCost(0)))
            {
                if (level.HasFreeWorkers())
                {
                    ResourceData rd = td.GetBuildResource(0);
                    ca.CommodityCountChangeHelper(0, rd, -td.GetBuildCost(0));

                    t.StartConstructing(X, Y);
                    level.GameObjectManager.AddGameObject(t);
                }
            }
        }
    }
}
