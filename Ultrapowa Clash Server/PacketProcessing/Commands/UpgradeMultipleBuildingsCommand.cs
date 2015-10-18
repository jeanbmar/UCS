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
    //Commande 0x225
    class UpgradeMultipleBuildingsCommand : Command
    {
        private List<int> m_vBuildingIdList;
        private byte m_vIsAltResource;

        public UpgradeMultipleBuildingsCommand(BinaryReader br)
        {
            m_vIsAltResource = br.ReadByte();
            m_vBuildingIdList = new List<int>();
            int buildingCount = br.ReadInt32WithEndian();
            for (int i = 0; i < buildingCount; i++)
            {
                int buildingId = br.ReadInt32WithEndian();//= buildingId - 0x1DCD6500;
                m_vBuildingIdList.Add(buildingId);
            }
            br.ReadInt32WithEndian();
        }

        //00 00 02 25 00 00 00 00 07 1D CD 65 0A 1D CD 65 09 1D CD 65 0B 1D CD 65 08 1D CD 65 0C 1D CD 65 07 1D CD 65 06 00 00 1B 07
        //public uint Unknown1 { get; set; } //00 00 2D 7F some client tick

        public override void Execute(Level level)
        {
            ClientAvatar ca = level.GetPlayerAvatar();

            foreach(var buildingId in m_vBuildingIdList)
            {
                Building b = (Building)level.GameObjectManager.GetGameObjectByID(buildingId);
                if (b.CanUpgrade())
                {
                    BuildingData bd = b.GetBuildingData();
                    int cost = bd.GetBuildCost(b.GetUpgradeLevel() + 1);
                    ResourceData rd;
                    if(m_vIsAltResource == 0)
                        rd = bd.GetBuildResource(b.GetUpgradeLevel() + 1);
                    else
                        rd = bd.GetAltBuildResource(b.GetUpgradeLevel() + 1);
                    if (ca.HasEnoughResources(rd, cost))
                    {
                        if (level.HasFreeWorkers())
                        {
                            ca.SetResourceCount(rd, ca.GetResourceCount(rd) - cost);
                            b.StartUpgrading();
                        }
                    }
                }
            }
        }
    }
}
