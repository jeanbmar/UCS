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
    //Commande 0x1FC 508
    class TrainUnitCommand : Command
    {
        public TrainUnitCommand(BinaryReader br)
        {
            BuildingId = br.ReadInt32WithEndian(); //buildingId - 0x1DCD6500;
            Unknown1 = br.ReadUInt32WithEndian();
            UnitType = br.ReadInt32WithEndian();
            Count = br.ReadInt32WithEndian();
            Unknown3 = br.ReadUInt32WithEndian();
        }

        public int BuildingId { get; set; }
        public uint Unknown1 { get; set; } //00 00 00 00
        public int UnitType { get; set; } //00 3D 09 03
        public int Count { get; set; } //00 00 00 01
        public uint Unknown3 { get; set; } //FF FF FF FF

        public override void Execute(Level level)
        {
            GameObject go = level.GameObjectManager.GetGameObjectByID(BuildingId);
            if(Count > 0)
            {
                Building b = (Building)go;
                UnitProductionComponent c = b.GetUnitProductionComponent();
                CombatItemData cid = (CombatItemData)ObjectManager.DataTables.GetDataById(UnitType);
                do
                {
                    if (!c.CanAddUnitToQueue(cid))
                        break;
                    ResourceData trainingResource = cid.GetTrainingResource();
                    ClientAvatar ca = level.GetHomeOwnerAvatar();
                    int trainingCost = cid.GetTrainingCost(ca.GetUnitUpgradeLevel(cid));
                    if (!ca.HasEnoughResources(trainingResource, trainingCost))
                        break;
                    ca.SetResourceCount(trainingResource, ca.GetResourceCount(trainingResource) - trainingCost);
                    c.AddUnitToProductionQueue(cid);
                    Count--;
                }
                while (Count > 0);
            }
        }

    }
}
