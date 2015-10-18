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
    //Commande 0x202
    class SpeedUpClearingCommand : Command
    {
        private int m_vObstacleId;

        public SpeedUpClearingCommand(BinaryReader br)
        {
            m_vObstacleId = br.ReadInt32WithEndian();
            br.ReadInt32WithEndian();
        }

        public override void Execute(Level level)
        {
            GameObject go = level.GameObjectManager.GetGameObjectByID(m_vObstacleId);
            if(go != null)
            {
                if(go.ClassId == 3)
                    ((Obstacle)go).SpeedUpClearing();
            }
        }
    }
}
