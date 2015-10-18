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
    //Commande 0x209
    class FreeWorkerCommand : Command
    {
        public int m_vTimeLeftSeconds;
        private byte m_vIsCommandEmbedded;
        private object m_vCommand;

        public FreeWorkerCommand(BinaryReader br)
        {
            m_vTimeLeftSeconds = br.ReadInt32WithEndian();
            m_vIsCommandEmbedded = br.ReadByte();
            if (m_vIsCommandEmbedded >= 0x01)
            {
                m_vCommand = CommandFactory.Read(br);
            }
        }

        public override void Execute(Level level)
        {
            if (level.WorkerManager.GetFreeWorkers() == 0)
            {
                level.WorkerManager.FinishTaskOfOneWorker();
                if (m_vIsCommandEmbedded >= 1)
                    ((Command)m_vCommand).Execute(level);
            }  
        }
    }
}
