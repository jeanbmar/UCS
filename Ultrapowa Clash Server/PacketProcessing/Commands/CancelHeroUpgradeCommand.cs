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
    //Commande 0x0213
    class CancelHeroUpgradeCommand : Command
    {
        int m_vBuildingId;

        public CancelHeroUpgradeCommand(BinaryReader br)
        {
            m_vBuildingId = br.ReadInt32WithEndian(); 
            br.ReadInt32WithEndian();
        }

        //00 00 02 13 1D CD 65 06 00 01 8B 0F
        public override void Execute(Level level)
        {
            GameObject go = level.GameObjectManager.GetGameObjectByID(m_vBuildingId);
            if (go != null)
            {
                if (go.ClassId == 0)
                {
                    var b = (Building)go;
                    HeroBaseComponent hbc = b.GetHeroBaseComponent();
                    if(hbc != null)
                    {
                        hbc.CancelUpgrade();
                    }
                }
            }
        }

    }
}
