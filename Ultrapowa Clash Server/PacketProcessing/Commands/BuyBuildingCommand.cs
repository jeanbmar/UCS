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
    //Commande 0x1F4
    class BuyBuildingCommand : Command
    {
        public BuyBuildingCommand(BinaryReader br)
        {
            X = br.ReadInt32WithEndian();
            Y = br.ReadInt32WithEndian();
            BuildingId = br.ReadInt32WithEndian();
            Unknown1 = br.ReadUInt32WithEndian();
        }

        public int X { get; set; } 
        public int Y { get; set; } 
        public int BuildingId { get; set; } 
        public uint Unknown1 { get; set; } //00 00 2D 7F some client tick

        public override void Execute(Level level)
        {
            ClientAvatar ca = level.GetPlayerAvatar();

            BuildingData bd = (BuildingData)ObjectManager.DataTables.GetDataById(BuildingId);
            Building b = new Building(bd, level);

            if (ca.HasEnoughResources(bd.GetBuildResource(0), bd.GetBuildCost(0)))
            {
                if (bd.IsWorkerBuilding() || level.HasFreeWorkers())
                {
                    //Ajouter un check sur le réservoir d'élixir noir
                    ResourceData rd = bd.GetBuildResource(0);
                    ca.CommodityCountChangeHelper(0, rd, -bd.GetBuildCost(0));

                    b.StartConstructing(X, Y);
                    level.GameObjectManager.AddGameObject(b);
                }
            }
        }
    }
}
