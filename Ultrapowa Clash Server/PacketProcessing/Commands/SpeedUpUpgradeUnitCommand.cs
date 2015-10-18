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
    //Commande 0x205
    class SpeedUpUpgradeUnitCommand : Command
    {
        private int m_vBuildingId;

        public SpeedUpUpgradeUnitCommand(BinaryReader br)
        {
            m_vBuildingId = br.ReadInt32WithEndian(); //buildingId - 0x1DCD6500;
            br.ReadInt32WithEndian();
        }

        //00 00 02 05 1D CD 65 13 00 00 53 8F

        public override void Execute(Level level)
        {
            ClientAvatar ca = level.GetPlayerAvatar();
            GameObject go = level.GameObjectManager.GetGameObjectByID(m_vBuildingId);
            if(go != null)
            {
                if(go.ClassId == 0)
                {
                    Building b = (Building)go;
                    UnitUpgradeComponent uuc = b.GetUnitUpgradeComponent();
                    if(uuc != null)
                    {
                        if (uuc.GetCurrentlyUpgradedUnit() != null)
                        {
                            uuc.SpeedUp();
                        }
                    }
                }
            } 
        }
    }
}
