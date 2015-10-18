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
    //Commande 0x020F
    class UpgradeHeroCommand : Command
    {
        public UpgradeHeroCommand(BinaryReader br)
        {
            BuildingId = br.ReadInt32WithEndian();
            Unknown1 = br.ReadUInt32WithEndian();
        }

        //00 00 02 0F 1D CD 65 09 00 01 03 63

        public int BuildingId { get; set; } 
        public uint Unknown1 { get; set; }

        public override void Execute(Level level)
        {
            ClientAvatar ca = level.GetPlayerAvatar();
            GameObject go = level.GameObjectManager.GetGameObjectByID(BuildingId);
            if(go != null)
            {
                Building b = (Building)go;
                HeroBaseComponent hbc = b.GetHeroBaseComponent();
                if(hbc != null)
                {
                    if(hbc.CanStartUpgrading())
                    {
                        HeroData hd = ObjectManager.DataTables.GetHeroByName(b.GetBuildingData().HeroType);
                        int currentLevel = ca.GetUnitUpgradeLevel(hd);
                        ResourceData rd = hd.GetUpgradeResource(currentLevel);
                        int cost = hd.GetUpgradeCost(currentLevel);
                        if(ca.HasEnoughResources(rd, cost))
                        {
                            if(level.HasFreeWorkers())
                            {
                                hbc.StartUpgrading();
                            }
                        }
                    }
                }
            }
        }
    }
}
