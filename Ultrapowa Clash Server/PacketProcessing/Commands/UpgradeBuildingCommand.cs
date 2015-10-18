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
    //Commande 0x1F6
    class UpgradeBuildingCommand : Command
    {
        public UpgradeBuildingCommand(BinaryReader br)
        {
            BuildingId = br.ReadInt32WithEndian(); //buildingId - 0x1DCD6500;
            Unknown2 = br.ReadByte();
            Unknown1 = br.ReadUInt32WithEndian();
        }

        public int BuildingId { get; set; }
        public uint Unknown2 { get; set; } 
        public uint Unknown1 { get; set; }

        public override void Execute(Level level)
        {
            ClientAvatar ca = level.GetPlayerAvatar();
            GameObject go = level.GameObjectManager.GetGameObjectByID(BuildingId);

            ConstructionItem b = (ConstructionItem)go;
            if(b.CanUpgrade())
            {
                var bd = b.GetConstructionItemData();
                if (ca.HasEnoughResources(bd.GetBuildResource(b.GetUpgradeLevel() + 1), bd.GetBuildCost(b.GetUpgradeLevel() + 1)))
                {
                    if (level.HasFreeWorkers())
                    {
                        ResourceData rd = bd.GetBuildResource(b.GetUpgradeLevel() + 1);
                        ca.SetResourceCount(rd, ca.GetResourceCount(rd) - bd.GetBuildCost(b.GetUpgradeLevel() + 1));
                        b.StartUpgrading();
                    }
                }
            }
        }
    }
}
