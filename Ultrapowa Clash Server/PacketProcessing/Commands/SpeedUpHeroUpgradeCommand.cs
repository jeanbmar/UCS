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
    //Commande 0x0210
    class SpeedUpHeroUpgradeCommand : Command
    {
        private int m_vBuildingId;

        public SpeedUpHeroUpgradeCommand(BinaryReader br)
        {
            m_vBuildingId = br.ReadInt32WithEndian(); //buildingId - 0x1DCD6500;
            br.ReadInt32WithEndian();
        }

        //00 00 02 10 1D CD 65 09 00 01 03 B7

        public override void Execute(Level level)
        {
            ClientAvatar ca = level.GetPlayerAvatar();
            GameObject go = level.GameObjectManager.GetGameObjectByID(m_vBuildingId);

            if (go != null)
            {
                Building b = (Building)go;
                HeroBaseComponent hbc = b.GetHeroBaseComponent();
                if (hbc != null)
                    hbc.SpeedUpUpgrade();
            }
        }
    }
}
