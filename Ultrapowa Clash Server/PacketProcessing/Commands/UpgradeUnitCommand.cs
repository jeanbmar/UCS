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
    //Commande 0x204
    class UpgradeUnitCommand : Command
    {
        public UpgradeUnitCommand(BinaryReader br)
        {
            BuildingId = br.ReadInt32WithEndian(); //buildingId - 0x1DCD6500;
            Unknown1 = br.ReadUInt32WithEndian();
            UnitData = (CombatItemData)br.ReadDataReference();//.ReadInt32WithEndian();
            Unknown2 = br.ReadUInt32WithEndian();
        }

        //00 00 02 04 1D CD 65 13 00 00 00 00 00 3D 09 00 00 00 51 E9

        public int BuildingId { get; set; }
        public uint Unknown1 { get; set; }//00 00 00 00 
        public CombatItemData UnitData { get; set; }//00 3D 09 00
        public uint Unknown2 { get; set; }

        public override void Execute(Level level)
        {
            ClientAvatar ca = level.GetPlayerAvatar();
            GameObject go = level.GameObjectManager.GetGameObjectByID(BuildingId);
            Building b = (Building)go;
            UnitUpgradeComponent uuc = b.GetUnitUpgradeComponent();
            int unitLevel = ca.GetUnitUpgradeLevel(this.UnitData);
            if(uuc.CanStartUpgrading(this.UnitData))
            {
                int cost = this.UnitData.GetUpgradeCost(unitLevel);
                ResourceData rd = this.UnitData.GetUpgradeResource(unitLevel);
                if(ca.HasEnoughResources(rd,cost))
                {
                    ca.SetResourceCount(rd, ca.GetResourceCount(rd) - cost);
                    uuc.StartUpgrading(this.UnitData);
                }
            }
        }
    }
}
